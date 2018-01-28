using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AppState {
    MENU,
    START_GAME_TRANSITION,
    CLIMB_GAME,
    CLIMB_DEATH,
    CLIMB_TO_YODELER_TRANSITION,
    YODELER_GAME,
    YODELER_TO_CLIMB_TRANSITION,
    NUM_STATES
}

public class AppManager : MonoBehaviour
{
    public Player player;
    public CameraController mainCamera;
    public MenuController menuController;
    public ClimbGame climbGame;
    public YodelerGame yodelerGame;
    public bool debugEnabled = true;

    public static AppManager instance { get; private set; }
    public AppState startingState = AppState.MENU;
    public static AppState currentState { get; private set; }

    public void Start()
    {
        if (instance == null) instance = this;
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
        if (Input.GetKeyUp("x")) SwitchToNextState();

        switch (currentState) {
            case AppState.MENU: {
                if (Input.GetButtonUp("Submit")) {
                    SwitchState(AppState.START_GAME_TRANSITION);
                }
            } break;

            case AppState.START_GAME_TRANSITION: {
            } break;

            case AppState.CLIMB_GAME: {
            } break;

            case AppState.CLIMB_TO_YODELER_TRANSITION: {
            } break;

            case AppState.YODELER_GAME: {
            } break;

            case AppState.YODELER_TO_CLIMB_TRANSITION: {
            } break;
        }
    }

    public void OnGUI()
    {
        if (!debugEnabled) return;

        GUILayout.Label(currentState.ToString());
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

        instance.mainCamera.AnimateTransition(newState);
        instance.menuController.AnimateTransition(newState);
        instance.climbGame.SetAppState(newState);

        if (newState == AppState.YODELER_GAME)
            instance.yodelerGame.StartYodelerGame();
        else
            instance.yodelerGame.EndYodelerGame();

        switch (newState) {
            case AppState.MENU: {
                // Play game button
            } break;

            case AppState.START_GAME_TRANSITION: {
                instance.climbGame.ResetWorld();
                CallWithDelay(() => { SwitchState(AppState.CLIMB_GAME); }, 5.0f);
                // Start running
                // Start spawning enemies.
            } break;

            case AppState.CLIMB_GAME: {
                // Run climb game (spawn enemies, move enemies).
                // Paralax backgrounds.
                // Move terrain.
            } break;

            case AppState.CLIMB_TO_YODELER_TRANSITION: {
                CallWithDelay(() => { SwitchState(AppState.YODELER_GAME); }, 5.0f);
                // TODO: Stop spawning enemies.
                // Animate terrain change.
                // Move player to position.
                // Zoom out.
                // Countdown to Yodeler game.
            } break;

            case AppState.YODELER_GAME: {
                instance.climbGame.KillAllEnemies();
                // Scroll arrows
                // Measure acuracy of pressing said arrows
                //
            } break;

            case AppState.YODELER_TO_CLIMB_TRANSITION: {
                //
            } break;
        }

        currentState = newState;
    }
}
