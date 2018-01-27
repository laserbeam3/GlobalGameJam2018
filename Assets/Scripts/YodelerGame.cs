using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YodelerGame : MonoBehaviour
{
    public Camera yodelerCamera;
    public Transform yodelerGameArena;

    public float bpm = 120.0f;
    public float yodelerGameStartTime;
    public float nextBeatBarTime;
    public float noteTravelTime = 3f;
    public float noteSpawnY = - 7;
    public float notePerfectY = 3;
    public float noteDespawnY = 20;
    public float hitRange = 0.1f;
    public float noteChance = 0.4f;
    public float arenaMinX = -4;
    public float arenaMaxX = 4;

    public Note[] notePrefabs;
    public Note beatBarPrefab;
    public List<Note> activeNotes;

    bool isRunning = false;

    void Start()
    {
        EndYodelerGame();
    }

    public void StartYodelerGame()
    {
        yodelerGameStartTime = Time.time;
        nextBeatBarTime = 0;
        isRunning = true;
        yodelerCamera.enabled = true;
    }

    public void EndYodelerGame()
    {
        for (int i = 0; i < activeNotes.Count; ++i) {
            Destroy(activeNotes[i].gameObject);
        }
        activeNotes.Clear();
        isRunning = false;
        yodelerCamera.enabled = false;
    }

    void Update ()
    {
        if (!isRunning) return;

        float t = Time.time - yodelerGameStartTime;

        if (t >= nextBeatBarTime) {
            var b = Instantiate(beatBarPrefab, yodelerGameArena);
            b.transform.localPosition = new Vector3(0, noteSpawnY, 0);
            b.spawnTime = nextBeatBarTime;
            activeNotes.Add(b);

            if (Random.value < noteChance) {
                var note = Instantiate(notePrefabs[Random.Range(0, notePrefabs.Length)], yodelerGameArena);
                note.transform.localPosition = new Vector3(Random.Range(arenaMinX, arenaMaxX), noteSpawnY, 0);
                note.spawnTime = nextBeatBarTime;
                activeNotes.Add(note);
            }

            nextBeatBarTime += 60.0f / bpm;
        }

        for (int i = 0; i < activeNotes.Count; ++i) {
            var note = activeNotes[i];
            float ellapsed = t - note.spawnTime;
            float d = (ellapsed / noteTravelTime) * (notePerfectY - noteSpawnY);
            float dist = Mathf.Abs(noteTravelTime - ellapsed);
            var scale = note.transform.localScale;
            if (dist < hitRange) {
                if (note.interactible && Input.GetKeyDown("space")) note.Activate();
                scale.x = 1 + (hitRange - dist) / hitRange;
            } else {
                scale.x = 1;
            }
            note.transform.localScale = scale;
            note.transform.localPosition = new Vector3(note.transform.localPosition.x, noteSpawnY + d, 0);

            if (note.interactible) {
                if (ellapsed > noteTravelTime + hitRange) note.Miss();
            }

            if (d > noteDespawnY) {
                activeNotes[i] = activeNotes[activeNotes.Count-1];
                i--;
                Destroy(note.gameObject);
            }
        }
    }
}
