using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InclinedPlanePosition : MonoBehaviour {

    private Vector2 _pos = Vector2.zero;
    public bool limitForward = false;
    public bool limitSideways = true;
    public float halfWidth = 1.25f;

    // (forward, sideways)
    public Vector2 pos {
        get { return _pos; }
        set {
            _pos = value;
            if (limitForward)
                _pos.x = Mathf.Clamp(_pos.x, ClimbGame.instance.minForward,
                                             ClimbGame.instance.maxForward);
            if (limitSideways)
                _pos.y = Mathf.Clamp(_pos.y, ClimbGame.instance.minSideways,
                                             ClimbGame.instance.maxSideways);

            var p = ClimbGame.instance.logicalPlane.transform.position;
            p.z = 0;

            transform.position = p + ClimbGame.instance.forward  * _pos.x +
                                     ClimbGame.instance.sideways * _pos.y;
        }
    }

    public int lane {
        get {
            return (int)((pos.y - ClimbGame.instance.minSideways) *
                         ClimbGame.instance.laneCount / ClimbGame.instance.widthSideways);
        }
    }

    public bool CollidesWith(InclinedPlanePosition other)
    {
        if (Mathf.Abs(pos.x - other.pos.x) > (halfWidth + other.halfWidth)) return false;
        return (lane == other.lane);
    }
}
