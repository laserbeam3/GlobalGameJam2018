using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public float xSpeed = 4.0f;
    public float ySpeed = 4.0f;

    void Start ()
    {

    }

    // Update is called once per frame
    void Update ()
    {
        float delta = Time.deltaTime;

        float xInput = Input.GetAxisRaw("Horizontal") * xSpeed * delta;
        float yInput = Input.GetAxisRaw("Vertical")   * ySpeed * delta;

        Transform logicalPlane = World.instance.logicalPlane.transform;
        Vector3 forward = logicalPlane.right;
        Vector3 sideways = logicalPlane.up;

        Vector3 movement = forward * xInput + sideways * yInput;

        transform.localPosition += movement;

    }
}
