using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class StaticCell : MonoBehaviour
{
    private GameManager gameManager;
    private Tilemap Tilemap;

    private void Awake() {
        gameManager = FindObjectOfType<GameManager>();
        Tilemap = GameObject.FindGameObjectWithTag("Tilemap").GetComponent<Tilemap>();
    }  
    private void OnMouseOver() {
        if(Input.GetKey(KeyCode.Mouse1)){
            gameManager.simulatedCells--;
            Vector3Int cellPosition = Tilemap.LocalToCell(transform.position);
            Tilemap.SetTile(cellPosition, null);
            Destroy(gameObject);
        }
    }
}
