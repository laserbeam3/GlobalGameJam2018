using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InclinedPlanePosition : MonoBehaviour {

    private Vector2 pos = Vector2.zero;
    public bool limitForward = false;
    public bool limitSideways = true;

    // (forward, sideways)
    public Vector2 Pos {
        get { return pos; }
        set {
            pos = value;
            if (limitForward)
                pos.x = Mathf.Clamp(pos.x, World.instance.minForward,
                                           World.instance.maxForward);
            if (limitSideways)
                pos.y = Mathf.Clamp(pos.y, World.instance.minSideways,
                                           World.instance.maxSideways);

            var p = World.instance.logicalPlane.transform.position;
            p.z = 0;

            transform.position = p + World.instance.forward  * pos.x +
                                     World.instance.sideways * pos.y;
        }
    }
}
