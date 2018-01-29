using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alien : MonoBehaviour
{
    public void OnBeamStartCapture()
    {
        AppManager.instance.player.renderer.enabled = false;
        AppManager.instance.mainCamera.AnimateUpExit();
        AppManager.CallWithDelay(() => {
            AppManager.SwitchState(AppState.MENU);
            AppManager.instance.mainCamera.JumpToMenuState();
        }, 4f);
    }
}
