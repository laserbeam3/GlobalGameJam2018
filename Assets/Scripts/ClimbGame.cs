using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbGame : MonoBehaviour
{
    public Player player;
    public GameObject logicalPlane;
    public EnemyTest[] obstaclePrefabs;
    public ScrollingMidground midGround;

    public static ClimbGame instance { get; private set; }

    public float           nextEnemySpawnTime = 0;
    public float           globalSpeed        = 2.0f;
    public float           minEmenySpawnDelta = 1.0f;
    public float           maxEnemySpawnDelta = 4.0f;
    public List<EnemyTest> enemies            = new List<EnemyTest>(32);

    private bool isRunning = false;
    public bool allowPlayerMovement = false;

    public Vector3 forward
    {
        get { return logicalPlane.transform.right; }
    }

    public Vector3 sideways
    {
        get { return logicalPlane.transform.up; }
    }

    public float minForward
    {
        get { return logicalPlane.transform.localScale.x * -0.5f; }
    }

    public float maxForward
    {
        get { return logicalPlane.transform.localScale.x * 0.5f; }
    }

    public float minSideways
    {
        get { return logicalPlane.transform.localScale.y * -0.5f; }
    }

    public float maxSideways
    {
        get { return logicalPlane.transform.localScale.y * 0.5f; }
    }

    public float widthSideways
    {
        get { return logicalPlane.transform.localScale.y * 1f; }
    }

    public float spawnForward
    {
        get { return maxForward * 2; }
    }

    public float despawnForward
    {
        get { return minForward * 2; }
    }

    public int laneCount = 3;
 
    public void StartGame()
    {
        instance.player.health = instance.player.maxHP;
        KillAllEnemies();
        nextEnemySpawnTime = Time.time;
        //AppManager.CallWithDelay(() => {PrepareEnd();}, 2f);
        allowPlayerMovement = true;
    }

    private float timeToRun = 3f;
    public void PrepareEnd()
    {
        float distance = midGround.StopTilingAndGetDistanceToEnd();
        float timeToEnd = distance / globalSpeed;
        Debug.Log(distance + " " + timeToEnd);
        float timeToStopInput = timeToEnd - timeToRun;
        AppManager.CallWithDelay(() => {StopInputAndMoveToYolderPose();}, timeToStopInput);
    }

    public void StopInputAndMoveToYolderPose()
    {
        allowPlayerMovement = false;
        iTween.MoveTo(player.gameObject, iTween.Hash("position", new Vector3(6.7f, 2f, 0f),
                                                     "easeType", "linear",
                                                     "loopType", "none",
                                                     "time", timeToRun));
        AppManager.instance.mainCamera.AnimateToYodelerPos();

        // iTween.ScaleTo(player.gameObject, iTween.Hash("scale", new Vector3(0.5f, 0.5f, 0.5f),
        //                                               "easeType", "linear",
        //                                               "loopType", "none",
        //                                               "time", timeToRun));

    }

    public void SetAppState(AppState newState) {
        isRunning = (newState == AppState.CLIMB_GAME);
        if (isRunning) StartGame();
    }

    public void Start()
    {
        if (instance == null) instance = this;
        float rot = Mathf.Atan2(forward.y, forward.x) * Mathf.Rad2Deg;
        midGround.transform.eulerAngles = new Vector3(0, 0, rot);
    }

    public void KillAllEnemies() {
        for (int i = 0; i < enemies.Count; ++i) {
            Destroy(enemies[i].gameObject);
        }
        enemies.Clear();
    }

    public void Update()
    {
        if (AppManager.currentState != AppState.CLIMB_GAME) return;

        float delta = Time.deltaTime;

        if (Time.time > nextEnemySpawnTime) {
            var prefab = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];
            var e = Instantiate(prefab).GetComponent<EnemyTest>();
            enemies.Add(e);
            float laneBorder = widthSideways / (float) (laneCount * 2f);

            float side = minSideways + laneBorder + Random.Range(0, laneCount) * laneBorder * 2.0f;
            e.pos = new Vector2(spawnForward, Random.Range(-1, 2) * side);

            nextEnemySpawnTime += Random.Range(minEmenySpawnDelta, maxEnemySpawnDelta);
        }

        for (int i = 0; i < enemies.Count; ++i) {
            var e = enemies[i];
            e.pos -= new Vector2(globalSpeed * delta, 0);

            if (e.pos.x < despawnForward) {
                enemies[i] = enemies[enemies.Count-1];
                enemies.RemoveAt(enemies.Count-1);
                i--;
                Destroy(e.gameObject);
            }

            if (e.inclined.CollidesWith(player.inclined)) {
                // Do collision logic
            }
        }

        midGround.Move(-globalSpeed * delta);
    }
}
