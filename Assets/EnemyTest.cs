using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InclinedPlanePosition))]
public class EnemyTest : MonoBehaviour
{
    public float speed = 0f;
    private InclinedPlanePosition inclined;

    void Start ()
    {
        inclined = GetComponent<InclinedPlanePosition>();
    }

    void Update ()
    {
        inclined.Pos -= new Vector2(speed * Time.deltaTime, 0);
    }
}
