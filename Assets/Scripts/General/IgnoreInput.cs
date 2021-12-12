using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreInput : MonoBehaviour
{
    private GameManager gameManager;

    private void Awake() {
        gameManager = FindObjectOfType<GameManager>();
        gameManager.canClick = true;
    }

    private void OnMouseDown() {
        gameManager.canClick = false;
    }
    private void OnMouseUp() {
         gameManager.canClick = true;
    }
}
