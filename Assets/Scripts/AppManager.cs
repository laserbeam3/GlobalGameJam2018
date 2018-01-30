using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AppState {
    MENU,
    START_GAME_TRANSITION,
    CLIMB_GAME,
    CLIMB_DEATH,
    YODELER_GAME,
    YODELER_TO_CLIMB_TRANSITION,
    CREDITS,
    NUM_STATES
}

public class AppManager : MonoBehaviour
{
    public Player player;
    public SoundEffects effects;
    public CameraController mainCamera;
    public MenuController menuController;
    public ClimbGame climbGame;
    public YodelerGame yodelerGame;
    public bool debugEnabled = false;

    public static AppManager instance { get; private set; }
    public AppState startingState = AppState.MENU;
    public static AppState currentState { get; private set; }

    public void Awake()
    {
        if (instance == null) instance = this;
        effects = GetComponent<SoundEffects>();
        currentState = startingState;
        SwitchState(startingState);
    }

    public static void CallWithDelay(System.Action method, float delay)
    {
        instance.StartCoroutine(instance.CallWithDelayRoutine(method, delay));
    }

    private IEnumerator CallWithDelayRoutine(System.Action method, float delay)
    {
        yield return new WaitForSeconds(delay);
        method();
    }

    public void Update()
    {
        // if (Input.GetKeyUp("x")) SwitchToNextState();

        switch (currentState) {
            case AppState.MENU: {
                if (Input.GetButtonUp("Submit")) {
                    SwitchState(AppState.START_GAME_TRANSITION);
                } else if (Input.GetKeyUp(KeyCode.Escape)) {
                    Application.Quit();
                }
            } break;

            case AppState.START_GAME_TRANSITION: {
            } break;

            case AppState.CLIMB_GAME: {
            } break;

            case AppState.CLIMB_DEATH: {
                if (Input.GetButtonUp("Submit")) {
                    instance.menuController.SetDeathScreenAlpha(0);
                    instance.menuController.MoveMenu(instance.menuController.MenuVisiblePos);
                    SwitchState(AppState.MENU);
                }
            } break;

            case AppState.YODELER_GAME: {
            } break;

            case AppState.YODELER_TO_CLIMB_TRANSITION: {
            } break;

            case AppState.CREDITS: {
                if (Input.GetButtonUp("Submit")) {
                    SwitchState(AppState.MENU);
                } else if (Input.GetKeyUp(KeyCode.Escape)) {
                    Application.Quit();
                }
            } break;
        }
    }

    public void OnGUI()
    {
        if (!debugEnabled) return;

        // GUILayout.Label(currentState.ToString());
    }

    public static void SwitchToNextState()
    {
        int state = (int) currentState;
        state++;
        if (state == (int) AppState.NUM_STATES) state = 0;

        SwitchState((AppState) state);
    }

    public static void SwitchState(AppState newState) {
        if (newState == currentState) return;

        Debug.Log("Switch state to: " + newState);

        instance.mainCamera.AnimateTransition(newState);
        instance.menuController.AnimateTransition(newState);
        instance.climbGame.SetAppState(newState);
        instance.menuController.SetCreditsAlpha(0);

        if (newState == AppState.YODELER_GAME)
            instance.yodelerGame.StartYodelerGame();

        switch (newState) {
            case AppState.MENU: {
                instance.climbGame.ResetWorld();
            } break;

            case AppState.START_GAME_TRANSITION: {
                instance.climbGame.ResetWorld();
                CallWithDelay(() => { SwitchState(AppState.CLIMB_GAME); }, 2.5f);
            } break;

            case AppState.CLIMB_GAME: {
            } break;

            case AppState.YODELER_GAME: {
                instance.climbGame.player.anim.SetTrigger("sing");
                instance.climbGame.KillAllEnemies();
                // Scroll arrows
                // Measure acuracy of pressing said arrows
                //
            } break;

            case AppState.YODELER_TO_CLIMB_TRANSITION: {
                // instance.menuController.SetCreditsAlpha(1);
                AppManager.CallWithDelay(() => {
                    AppManager.instance.climbGame.alien.SetTrigger("run");
                }, 2f);
            } break;

            case AppState.CREDITS: {
                instance.menuController.SetCreditsAlpha(1);
                } break;
        }

        currentState = newState;
    }
}
