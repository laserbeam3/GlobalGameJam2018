using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    public GameObject player;
    public GameObject logicalPlane;
    public GameObject[] obstaclePrefabs;

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
    public ClimbGameState climbState;

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
 

    public void Start()
    {
        if (instance == null) instance = this;
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

    public void MoveTransformOnInclinedPlane(Transform t,
                                             float forwardDelta, float sidewaysDelta,
                                             bool limitForward = false, bool limitSideways = true)
    {
        var p = t.position;
        var plane = logicalPlane.transform.position;
        plane.z = 0;

        p -= plane;
        float f = Vector3.Project(p, forward).magnitude;
        if (f == 0) f = -Vector3.Project(p, -forward).magnitude;
        float s = Vector3.Project(p, sideways).magnitude;
        if (s == 0) s = -Vector3.Project(p, -sideways).magnitude;

        f += forwardDelta;
        s += sidewaysDelta;
        player.GetComponent<Player>().f = f;
        player.GetComponent<Player>().s = s;

        float minf = minForward;
        float maxf = maxForward;
        float mins = minSideways;
        float maxs = maxSideways;

        // if (limitForward)
        //     f = Mathf.Clamp(f, minf, maxf);

        // if (limitSideways)
        //     s = Mathf.Clamp(s, mins, maxs);

        t.position = plane + f * forward + s * sideways;
    }

    private void ClimbGameUpdate()
    {
        for (int i = 0; i < climbState.enemies.Count; ++i) {

        }
    }
}

public class ClimbGameState
{
    public float nextEnemySpawnTime;
    public float globalSpeed;
    public List<EnemyTest> enemies = new List<EnemyTest>(32);
}
