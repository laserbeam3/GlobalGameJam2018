using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InclinedPlanePosition))]
public class Player : MonoBehaviour
{
    private InclinedPlanePosition inclined;
    public float fwdSpeed = 4.0f;
    public float sideSpeed = 6.0f;

    public float f;
    public float s;

    public Vector2 Pos {
        get { return inclined.Pos; }
        set { inclined.Pos = value; }
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

        Pos += new Vector2(fwdInput, sideInput);
    }
}
