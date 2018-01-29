using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YodelerGame : MonoBehaviour
{
    public Camera yodelerCamera;
    public Transform yodelerGameArena;

    public float bpm = 123.0f;
    public float yodelerGameStartTime;
    public float nextBeatBarTime;
    public float noteTravelTime = 2f;
    public float noteSpawnY = - 7;
    public float notePerfectY = 3;
    public float noteDespawnY = 20;
    public float hitRange = 0.1f;
    public float noteChance = 0.4f;
    public float arenaMinX = -4;
    public float arenaMaxX = 4;
    private float endGameTime;

    public Note[] notePrefabs;
    public Note beatBarPrefab;
    public List<Note> activeNotes;
    public float totalGameTime = 45f;

    bool yodelerIsRunning = false;

    void Start()
    {
        for (int i = 0; i < activeNotes.Count; ++i) {
            Destroy(activeNotes[i].gameObject);
        }
        activeNotes.Clear();
        yodelerIsRunning = false;
        yodelerCamera.enabled = false;
        AppManager.instance.mainCamera.FadeOutAndStop(1.0f);

    }

    public void StartYodelerGame()
    {
        yodelerGameStartTime = Time.time;
        nextBeatBarTime = 45.0f / bpm;
        yodelerIsRunning = true;
        endGameTime = totalGameTime + noteTravelTime * 2;
        yodelerCamera.enabled = true;
        AppManager.instance.mainCamera.SetVolume(1.0f);
        AppManager.instance.mainCamera.PlayOneShot(AppManager.instance.mainCamera.singMusic);
    }

    public void EndYodelerGame()
    {
        Debug.Log("End Yodeler Game");
        for (int i = 0; i < activeNotes.Count; ++i) {
            Destroy(activeNotes[i].gameObject);
        }
        activeNotes.Clear(); 
        yodelerIsRunning = false;
        yodelerCamera.enabled = false;
        AppManager.instance.mainCamera.FadeOutAndStop(1.0f);
        AppManager.SwitchState(AppState.YODELER_TO_CLIMB_TRANSITION);
    }

    void Update ()
    {
        if (!yodelerIsRunning) return;

        float t = Time.time - yodelerGameStartTime;

        if (t >= endGameTime) {
            EndYodelerGame();
            // Debug.Log("EndYodelerGame???");
        }

        if (t >= nextBeatBarTime) {
            // var b = Instantiate(beatBarPrefab, yodelerGameArena);
            // b.transform.localPosition = new Vector3(0, noteSpawnY, 0);
            // b.spawnTime = nextBeatBarTime;
            // activeNotes.Add(b);

            if (t <= totalGameTime && Random.value < noteChance) {
                var note = Instantiate(notePrefabs[Random.Range(0, notePrefabs.Length)], yodelerGameArena);
                note.transform.localPosition = new Vector3(Random.Range(arenaMinX, arenaMaxX), noteSpawnY, 0);
                note.spawnTime = nextBeatBarTime;
                activeNotes.Add(note);
            }

            nextBeatBarTime += 60.0f / bpm;
        }

        bool[] keys       = { false, false, false, false };
        bool[] keyCorrect = { false, false, false, false };
        keys[(int)Note.Key.up]    = Input.GetKeyDown("up")    || Input.GetKeyDown("w");
        keys[(int)Note.Key.down]  = Input.GetKeyDown("down")  || Input.GetKeyDown("s");
        keys[(int)Note.Key.left]  = Input.GetKeyDown("left")  || Input.GetKeyDown("a");
        keys[(int)Note.Key.right] = Input.GetKeyDown("right") || Input.GetKeyDown("d");

        for (int i = 0; i < activeNotes.Count; ++i) {
            var note = activeNotes[i];
            float ellapsed = t - note.spawnTime;
            float d = (ellapsed / noteTravelTime) * (notePerfectY - noteSpawnY);
            float dist = Mathf.Abs(noteTravelTime - ellapsed);
            var scale = note.transform.localScale;

            if (dist < hitRange) {
                keyCorrect[(int)note.key] = true;
                if (note.interactible && keys[(int)note.key]) note.Activate();
                scale.x = 1 + (hitRange - dist) / hitRange;
            } else {
                scale.x = 1;
            }
            note.transform.localScale = scale;
            note.transform.localPosition = new Vector3(note.transform.localPosition.x, noteSpawnY + d, 0);

            if (note.interactible) {
                if (ellapsed > noteTravelTime + hitRange) note.Miss();
            }

            // if (d > noteDespawnY) {
            //     activeNotes[i] = activeNotes[activeNotes.Count-1];
            //     i--;
            //     Destroy(note.gameObject);
            // }
        }
    }
}
