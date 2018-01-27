using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Note : MonoBehaviour
{
    public float spawnTime;
    public string key;
    public bool interactible;
    public bool wasActivated { get; private set; }
    public bool wasMissed { get; private set; }

    public void Activate() {
        if (wasMissed) return;
        wasActivated = true;
        GetComponent<SpriteRenderer>().color = Color.green;
    }

    public void Miss() {
        if (wasActivated) return;
        wasMissed = true;
        GetComponent<SpriteRenderer>().color = Color.red;
    }
}
