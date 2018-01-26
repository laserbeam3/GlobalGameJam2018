using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InclinedPlanePosition))]
public class EnemyTest : MonoBehaviour
{
    public float speed = 0f;

    private InclinedPlanePosition inclined;
    public Vector2 Pos {
        get { return inclined.Pos; }
        set { inclined.Pos = value; }
    }

    void Awake()
    {
        inclined = GetComponent<InclinedPlanePosition>();
    }

    void Update()
    {
        inclined.Pos -= new Vector2(speed * Time.deltaTime, 0);
    }
}
