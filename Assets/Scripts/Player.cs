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

    public int maxHP = 3;
    private int _health;
    public int health {
        get { return _health; }
        set { _health = Mathf.Clamp(value, 0, maxHP); }
    }

    void Awake()
    {
        inclined = GetComponent<InclinedPlanePosition>();
        inclined.limitForward = true;
    }

    void Start()
    {
        pos = pos;
    }

    void Update()
    {
        if (!ClimbGame.instance.allowPlayerMovement) return;

        float delta = Time.deltaTime;

        float fwdInput  = Input.GetAxisRaw("Horizontal") * fwdSpeed  * delta;
        float sideInput = Input.GetAxisRaw("Vertical")   * sideSpeed * delta;

        pos += new Vector2(fwdInput, sideInput);
    }

}
