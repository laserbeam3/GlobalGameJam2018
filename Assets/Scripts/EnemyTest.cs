using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InclinedPlanePosition))]
public class EnemyTest : MonoBehaviour
{
    public float speed = 0f;

    public InclinedPlanePosition inclined { get; private set; }
    public Vector2 pos {
        get { return inclined.pos; }
        set { inclined.pos = value; }
    }

    void Awake()
    {
        inclined = GetComponent<InclinedPlanePosition>();
    }

    void Update()
    {
        if (AppManager.currentState != AppState.CLIMB_GAME) return;

        inclined.pos -= new Vector2(speed * Time.deltaTime, 0);
    }
}
