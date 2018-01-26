using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    public Player player;
    public GameObject logicalPlane;
    public EnemyTest[] obstaclePrefabs;

    public static World instance { get; private set; }

    public enum AppState {
        MENU,
        START_GAME_TRANSITION,
        CLIMB_GAME,
        CLIMB_TO_YODELER_TRANSITION,
        YODELER_GAME,
        YODELER_TO_CLIMB_TRANSITION,
    }

    public AppState currentState;
    public ClimbGameState climbState = new ClimbGameState();

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
 

    public void Start()
    {
        if (instance == null) instance = this;
        climbState.nextEnemySpawnTime = Time.time;
    }

    public void Update()
    {
        switch (currentState) {
            case AppState.MENU: {
            } break;

            case AppState.START_GAME_TRANSITION:  {
            } break;

            case AppState.CLIMB_GAME: {
                ClimbGameUpdate();
            } break;

            case AppState.CLIMB_TO_YODELER_TRANSITION: {
            } break;

            case AppState.YODELER_GAME: {
            } break;

            case AppState.YODELER_TO_CLIMB_TRANSITION: {
            } break;

        }
    }

    private void ClimbGameUpdate()
    {
        float delta = Time.deltaTime;

        if (Time.time > climbState.nextEnemySpawnTime) {
            var prefab = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];
            var e = Instantiate(prefab).GetComponent<EnemyTest>();
            climbState.enemies.Add(e);
            float laneBorder = widthSideways / (float) (laneCount * 2f);

            float side = minSideways + laneBorder +
                         Random.Range(0, laneCount) * laneBorder * 2.0f;
            e.pos = new Vector2(spawnForward, Random.Range(-1, 2) * side);

            climbState.nextEnemySpawnTime += Random.Range(climbState.minEmenySpawnDelta,
                                                          climbState.maxEnemySpawnDelta);
        }

        for (int i = 0; i < climbState.enemies.Count; ++i) {
            var e = climbState.enemies[i];
            e.pos -= new Vector2(climbState.globalSpeed * delta, 0);

            if (e.pos.x < despawnForward) {
                climbState.enemies[i] = climbState.enemies[climbState.enemies.Count-1];
                climbState.enemies.RemoveAt(climbState.enemies.Count-1);
                i--;
                Destroy(e.gameObject);
            }

            if (e.inclined.CollidesWith(player.inclined)) {
                // Do collision logic
            }
        }
    }
}

public class ClimbGameState
{
    public float nextEnemySpawnTime = 0;
    public float globalSpeed = 2.0f;
    public float minEmenySpawnDelta = 1.0f;
    public float maxEnemySpawnDelta = 4.0f;
    public List<EnemyTest> enemies = new List<EnemyTest>(32);
}
