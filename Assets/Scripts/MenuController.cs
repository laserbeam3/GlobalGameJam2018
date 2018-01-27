using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class MenuController : MonoBehaviour {

    private Vector2 MenuVisiblePos;
    private Vector2 MenuInvisiblePos;

    public RectTransform menu;
    private Canvas canvas;
    private Vector2 canvasSize;

    public void Start()
    {
        canvas = GetComponent<Canvas>();
        canvasSize = canvas.GetComponent<RectTransform>().rect.size;
        menu.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, canvasSize.x);
        menu.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, canvasSize.y);

        MenuVisiblePos = Vector2.zero;
        MenuInvisiblePos = new Vector2(0, canvasSize.y);
    }

    public void AnimateTransition(AppState newState)
    {
        iTween.Stop(menu.gameObject);

        switch (newState) {
            case AppState.MENU: {
                if (menu.anchoredPosition != MenuVisiblePos)
                    iTween.ValueTo(menu.gameObject, iTween.Hash("from", menu.anchoredPosition,
                                                                "to", MenuVisiblePos,
                                                                "easeType", "easeOutExpo",
                                                                "loopType", "none",
                                                                "time", 1.8f,
                                                                "onupdatetarget", gameObject,
                                                                "onupdate", "MoveMenu"));
            } break;

            case AppState.START_GAME_TRANSITION:
            case AppState.CLIMB_DEATH:
            case AppState.CLIMB_GAME:
            case AppState.YODELER_TO_CLIMB_TRANSITION:
            case AppState.CLIMB_TO_YODELER_TRANSITION:
            case AppState.YODELER_GAME: {
                if (menu.anchoredPosition != MenuInvisiblePos)
                    iTween.ValueTo(menu.gameObject, iTween.Hash("from", menu.anchoredPosition,
                                                                "to", MenuInvisiblePos,
                                                                "easeType", "easeInOutExpo",
                                                                "loopType", "none",
                                                                "time", 3f,
                                                                "onupdatetarget", gameObject,
                                                                "onupdate", "MoveMenu"));
            } break;
        }
    }

    public void MoveMenu(Vector2 position){
        menu.anchoredPosition = position;
    }
}
