using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InclinedPlanePosition))]
public class Player : MonoBehaviour
{
    public InclinedPlanePosition inclined { get; private set; }
    public float fwdSpeed = 4.0f;
    public float sideSpeed = 6.0f;

    public float f;
    public float s;

    public Vector2 pos {
        get { return inclined.pos; }
        set { inclined.pos = value; }
    }

    void Awake()
    {
        inclined = GetComponent<InclinedPlanePosition>();
        inclined.limitForward = true;
    }

    void Update()
    {
        float delta = Time.deltaTime;

        float fwdInput  = Input.GetAxisRaw("Horizontal") * fwdSpeed  * delta;
        float sideInput = Input.GetAxisRaw("Vertical")   * sideSpeed * delta;

        pos += new Vector2(fwdInput, sideInput);
    }
}
