using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    public GameObject player;
    public GameObject logicalPlane;

    public static World instance { get; private set; }

    public void Start()
    {
        if (instance == null) instance = this;
    }
}
