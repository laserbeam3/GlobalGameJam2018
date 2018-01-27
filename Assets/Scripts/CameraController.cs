using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour {

    public Vector3 GameCameraPos;
    public float   GameCameraSize = 5;
    public Vector3 MainMenuCameraPos;
    public float   MainMenuCameraSize = 5;
    public Vector3 YodelerCameraPos;
    public float   YodelerCameraSize = 10;

    private Camera cam;

    void Start() {
        cam = GetComponent<Camera>();
    }

    public void AnimateToYodelerPos()
    {
        iTween.Stop(gameObject);
        if (transform.position != YodelerCameraPos)
            iTween.MoveTo(gameObject, iTween.Hash("position", YodelerCameraPos,
                                                  "easeType", "easeInOutExpo",
                                                  "loopType", "none",
                                                  "time", 3f));
        if (cam.orthographicSize != YodelerCameraSize)
            iTween.ValueTo(gameObject, iTween.Hash("from", cam.orthographicSize,
                                                   "to", YodelerCameraSize,
                                                   "onupdate", "UpdateCamSize",
                                                   "easeType", "easeInOutExpo",
                                                   "loopType", "none",
                                                   "time", 3f));
    }

    public void AnimateTransition(AppState newState)
    {
        iTween.Stop(gameObject);

        Debug.Log("Animate camera to: " + newState);
        switch (newState) {
            case AppState.MENU: {
                if (transform.position != MainMenuCameraPos)
                    iTween.MoveTo(gameObject, iTween.Hash("position", MainMenuCameraPos,
                                                          "easeType", "easeInOutExpo",
                                                          "loopType", "none",
                                                          "time", 3f));
                if (cam.orthographicSize != MainMenuCameraSize)
                    iTween.ValueTo(gameObject, iTween.Hash("from", cam.orthographicSize,
                                                           "to", MainMenuCameraSize,
                                                           "onupdate", "UpdateCamSize",
                                                           "easeType", "easeInOutExpo",
                                                           "loopType", "none",
                                                           "time", 3f));
            } break;

            case AppState.START_GAME_TRANSITION:
            case AppState.CLIMB_DEATH:
            case AppState.CLIMB_GAME:
            case AppState.YODELER_TO_CLIMB_TRANSITION: {
                if (transform.position != GameCameraPos)
                    iTween.MoveTo(gameObject, iTween.Hash("position", GameCameraPos,
                                                          "easeType", "easeInOutExpo",
                                                          "loopType", "none",
                                                          "time", 3f));
                if (cam.orthographicSize != GameCameraSize)
                    iTween.ValueTo(gameObject, iTween.Hash("from", cam.orthographicSize,
                                                           "to", GameCameraSize,
                                                           "onupdate", "UpdateCamSize",
                                                           "easeType", "easeInOutExpo",
                                                           "loopType", "none",
                                                           "time", 3f));
            } break;

            case AppState.CLIMB_TO_YODELER_TRANSITION:
            case AppState.YODELER_GAME: {
                AnimateToYodelerPos();
            } break;
        }
    }

    public void UpdateCamSize(float size) {
        cam.orthographicSize = size;
    }
}
