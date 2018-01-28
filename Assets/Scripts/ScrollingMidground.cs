using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingMidground : MonoBehaviour
{
    public bool useWidth = true;
    public float grassWidth;
    public float roadWidth;
    SpriteRenderer sr;

    public Transform[] grassTiles;
    public Transform[] roadTiles;
    public Transform cliff;

    private bool cliffPlaced = false;
    private bool jumpTiles = true;
    private bool stopMovement = false;
    public float cliffStop = 22.8f;

    public void Initialize()
    {
        cliffPlaced = false;
        grassWidth = grassTiles[0].GetComponent<SpriteRenderer>().sprite.bounds.size.x;
        roadWidth  = roadTiles[0].GetComponent<SpriteRenderer>().sprite.bounds.size.y;
        for (int i = 0; i < grassTiles.Length; ++i) {
            grassTiles[i].localPosition = new Vector3(i * grassWidth, 0, 30);
        }

        for (int i = 0; i < roadTiles.Length; ++i) {
            roadTiles[i].localPosition = new Vector3(i * roadWidth, 0, 29);
        }
        cliff.localPosition = new Vector3(100, 0, 28);
    }

    public void Move(float distance)
    {
        if (stopMovement) return;

        for (int i = 0; i < grassTiles.Length; ++i) {
            var tile = grassTiles[i];

            tile.localPosition += new Vector3(distance, 0, 0);

            if (jumpTiles && tile.localPosition.x < 0) {
                float d = grassWidth * grassTiles.Length;
                tile.localPosition += new Vector3(d, 0, 0);
            }
        }

        for (int i = 0; i < roadTiles.Length; ++i) {
            var tile = roadTiles[i];

            tile.localPosition += new Vector3(distance, 0, 0);

            if (tile.localPosition.x < 0) {
                float d = roadWidth * roadTiles.Length;
                float y = (tile.localPosition.x + d < cliff.localPosition.x - roadTiles.Length) ? 0 : 100;
                tile.localPosition += new Vector3(d, y, 0);
            }
        }

        if (!jumpTiles) {
            cliff.localPosition += new Vector3(distance, 0, 0);
            if (cliff.localPosition.x < cliffStop) {
                stopMovement = true;
            }
        }
    }

    public float GetDistanceToEnd() {
        float maxX = 0;

        for (int i = 0; i < grassTiles.Length; ++i) {
            var tile = grassTiles[i];
            maxX = Mathf.Max(maxX, tile.localPosition.x);
        }
        return maxX + grassWidth - cliffStop;
    }

    public float StopTilingAndGetDistanceToEnd() {
        jumpTiles = false;
        float maxX = 0;

        for (int i = 0; i < grassTiles.Length; ++i) {
            var tile = grassTiles[i];
            maxX = Mathf.Max(maxX, tile.localPosition.x);
        }
        cliff.localPosition = new Vector3(maxX + grassWidth, 0, 28);
        cliffPlaced = true;

        return cliff.localPosition.x - cliffStop;
    }
}
