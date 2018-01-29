using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
[RequireComponent(typeof(AudioSource))]
public class CameraController : MonoBehaviour {

    public Vector3 GameCameraPos;
    public float   GameCameraSize = 5;
    public Vector3 MainMenuCameraPos;
    public float   MainMenuCameraSize = 5;
    public Vector3 YodelerCameraPos;
    public float   YodelerCameraSize = 10;
    public Vector3 AlienCameraPos;
    public float   AlienCameraSize = 10;

    public AudioClip climbMusic;
    public AudioClip singMusic;

    public AudioSource audio;
    private Camera cam;

    public void SetVolume(float volume)
    {
        audio.volume = Mathf.Clamp(volume, 0f, 1f);
    }

    public void PlayOneShot(AudioClip clip)
    {
        audio.PlayOneShot(clip);
    }

    public void FadeOutAndStop(float time)
    {
        FadeVolume(0f, time);
        AppManager.CallWithDelay(() => audio.Stop(), time);
    }

    public void AnimateUpExit()
    {
        if (transform.position != YodelerCameraPos)
            iTween.MoveTo(gameObject, iTween.Hash("position", AlienCameraPos + new Vector3(0, 10, 0),
                                                  "easeType", "easeInOutExpo",
                                                  "loopType", "none",
                                                  "time", 3f));
    }

    public void FadeVolume(float to, float time)
    {
        iTween.StopByName("volFade");
        iTween.ValueTo(gameObject, iTween.Hash("name", "volFade",
                                               "from", audio.volume,
                                               "to", to,
                                               "onupdate", "SetVolume",
                                               "easeType", "linear",
                                               "loopType", "none",
                                               "time", time));
    }

    void Start() {
        cam = GetComponent<Camera>();
        audio = GetComponent<AudioSource>();
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

    public void JumpToMenuState()
    {
        iTween.Stop(gameObject);
        cam.orthographicSize = MainMenuCameraSize;
        transform.position = MainMenuCameraPos;
    }

    public void AnimateTransition(AppState newState)
    {
        iTween.Stop(gameObject);

        switch (newState) {
            case AppState.CLIMB_DEATH:
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
            case AppState.CLIMB_GAME: {
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

            case AppState.YODELER_GAME: {
                AnimateToYodelerPos();
            } break;

            case AppState.YODELER_TO_CLIMB_TRANSITION: {
                if (transform.position != AlienCameraPos)
                    iTween.MoveTo(gameObject, iTween.Hash("position", AlienCameraPos,
                                                          "easeType", "easeInOutExpo",
                                                          "loopType", "none",
                                                          "time", 3f));
                if (cam.orthographicSize != AlienCameraSize)
                    iTween.ValueTo(gameObject, iTween.Hash("from", cam.orthographicSize,
                                                           "to", AlienCameraSize,
                                                           "onupdate", "UpdateCamSize",
                                                           "easeType", "easeInOutExpo",
                                                           "loopType", "none",
                                                           "time", 3f));
            } break;
        }
    }

    public void UpdateCamSize(float size) {
        cam.orthographicSize = size;
    }
}
