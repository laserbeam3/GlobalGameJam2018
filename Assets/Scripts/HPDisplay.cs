using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPDisplay : MonoBehaviour
{
    private int oldHealth = 3;

    public Sprite hpOn;
    public Sprite hpOff;

    public Image[] healthIcons;

    void Update ()
    {
        if (ClimbGame.instance.player.health != oldHealth) {
            oldHealth = ClimbGame.instance.player.health;

            for (int i = 0; i < oldHealth && i < healthIcons.Length; ++i) {
                healthIcons[i].sprite = hpOn;
            }
            for (int i = oldHealth; i < healthIcons.Length; ++i) {
                healthIcons[i].sprite = hpOff;
            }
        }
    }
}
