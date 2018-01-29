using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbGame : MonoBehaviour
{
    public Animator alien;
    public Player player;
    public GameObject logicalPlane;
    public EnemyTest[] obstaclePrefabs;
    public ScrollingMidground midGround;

    public static ClimbGame instance { get; private set; }

    public  float           nextEnemySpawnTime    = 0;
    public  float           baseGlobalSpeed       = 3.0f;
    public  float           globalSpeed           = 3.0f;
    public  float           minEmenySpawnDelta    = 1.0f;
    public  float           maxEnemySpawnDelta    = 4.0f;
    public  List<EnemyTest> enemies               = new List<EnemyTest>(32);

    private bool            isRunning             = false;
    public  bool            allowPlayerMovement   = false;
    private float           gameStartTime;
    public  float           estimatedClimbTime    = 90.0f;
    private float           timeToStopSpawning    = 9999.0f;
    private float           timeToStopInput       = 9999.0f;
    private float           timeToExitGameState   = 9999.0f;
    private float           timeToStartCameraAnim = 9999.0f;

    public  float           walkToYodelerTime     = 3.5f;
    public  float           cameraToYodelerTime   = 3f;

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
        get { return maxForward * 2.5f; }
    }

    public float despawnForward
    {
        get { return minForward * 4f; }
    }

    public int laneCount = 3;
 
    public void ResetWorld()
    {
        instance.player.health = instance.player.maxHP;
        instance.player.anim.SetTrigger("reset");
        AppManager.instance.mainCamera.SetVolume(1f);
        midGround.Initialize();
        globalSpeed = baseGlobalSpeed;
        KillAllEnemies();
    }

    public void StartGame()
    {
        ResetWorld();
        nextEnemySpawnTime = Time.time;
        allowPlayerMovement = true;
        gameStartTime = Time.time;

        timeToStopSpawning    = Time.time + 9999.0f;
        timeToStopInput       = Time.time + 9999.0f;
        timeToStartCameraAnim = Time.time + 9999.0f;
        timeToExitGameState   = Time.time + 9999.0f;
        AppManager.instance.mainCamera.PlayOneShot(AppManager.instance.mainCamera.climbMusic);
    }

    private float timeToRun = 3f;
    public void PrepareEnd()
    {
        float distance = midGround.StopTilingAndGetDistanceToEnd();
        float timeToEnd = distance / globalSpeed;
        Debug.Log(distance + " " + timeToEnd);

        timeToStopSpawning    = Time.time + timeToEnd - 10f;
        timeToStopInput       = Time.time + timeToEnd - walkToYodelerTime;
        timeToStartCameraAnim = Time.time + timeToEnd - cameraToYodelerTime;
        timeToExitGameState   = Time.time + timeToEnd;
    }

    public void AnimateCamToYodelerPose()
    {
        AppManager.instance.mainCamera.AnimateToYodelerPos();
    }

    public void StopPlayerInput()
    {
        allowPlayerMovement = false;
        iTween.MoveTo(player.gameObject, iTween.Hash("position", new Vector3(6.7f, 2f, 0f),
                                                     "easeType", "linear",
                                                     "loopType", "none",
                                                     "time", timeToRun));
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

    public void KillAllEnemies()
    {
        for (int i = 0; i < enemies.Count; ++i) {
            Destroy(enemies[i].gameObject);
        }
        enemies.Clear();
    }

    public void LoseGame()
    {
        timeToStopSpawning    = Time.time + 9999.0f;
        timeToStopInput       = Time.time + 9999.0f;
        timeToStartCameraAnim = Time.time + 9999.0f;
        timeToExitGameState   = Time.time + 9999.0f;

        allowPlayerMovement = false;
        globalSpeed = 0;

        AppManager.CallWithDelay(() => PrepareLossScreen(), 2f);
        // iTween.ValueTo(gameObject, iTween.Hash("from", globalSpeed,
        //                                        "to", 0,
        //                                        "onupdate", "UpdateWorldSpeed",
        //                                        "oncomplete", "PrepareLossScreen",
        //                                        "easeType", "easeOutExpo",
        //                                        "loopType", "none",
        //                                        "time", 1f));
    }

    // public void UpdateWorldSpeed(float speed)
    // {
    //     globalSpeed = speed;

    // }

    public void PrepareLossScreen()
    {
        iTween.ValueTo(gameObject, iTween.Hash("from", 0.0f,
                                               "to", 1.0f,
                                               "onupdate", "SetDeathScreenAlpha",
                                               "onupdatetarget", AppManager.instance.menuController.gameObject,
                                               "easeType", "linear",
                                               "loopType", "none",
                                               "delay", 2f,
                                               "time", 1f));
        AppManager.instance.mainCamera.FadeOutAndStop(2f);
        AppManager.CallWithDelay(() => {
            ResetWorld();
            AppManager.instance.mainCamera.JumpToMenuState();
            AppManager.SwitchState(AppState.CLIMB_DEATH);
        }, 3f);
    }

    public void Update()
    {
        if (AppManager.currentState != AppState.CLIMB_GAME) return;

        float delta = Time.deltaTime;
        float gameTime = Time.time - gameStartTime;

        if (Time.time >= timeToStopInput) {
            StopPlayerInput();
            timeToStopInput += 999999f;
        }

        if (Time.time >= timeToStartCameraAnim) {
            AnimateCamToYodelerPose();
            timeToStartCameraAnim += 999999f;
        }

        if (Time.time >= timeToExitGameState) {
            AppManager.SwitchState(AppState.YODELER_GAME);
        }

        if (Time.time >= nextEnemySpawnTime && Time.time < timeToStopSpawning) {
            var prefab = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];
            var e = Instantiate(prefab).GetComponent<EnemyTest>();
            enemies.Add(e);
            float laneBorder = widthSideways / (float) (laneCount * 2f);

            float side = minSideways + laneBorder + Random.Range(0, laneCount) * laneBorder * 2.0f;
            e.pos = new Vector2(spawnForward, Random.Range(-1, 2) * side);

            nextEnemySpawnTime += Random.Range(minEmenySpawnDelta, maxEnemySpawnDelta);

            if (midGround.GetDistanceToEnd() / globalSpeed + gameTime > estimatedClimbTime) {
                Debug.Log("now!");
                PrepareEnd();
            }
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

            if (allowPlayerMovement && !e.alreadyHit && e.inclined.CollidesWith(player.inclined)) {
                e.alreadyHit = true;
                player.TakeDamage();
            }
        }

        midGround.Move(-globalSpeed * delta);
    }
}
