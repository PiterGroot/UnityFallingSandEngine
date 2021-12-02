using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CellSwitcher : MonoBehaviour
{
    private GameManager gameManager;
    [HideInInspector]public Tile CurrentTile, SecundaryTile;
    [HideInInspector]public GameObject CurrentCellPrefab, SecundaryCellPrefab;
    [SerializeField]private Tile[] Tiles;
    [SerializeField]private GameObject[] CellPrefabs;
    private void Awake() {
        gameManager = gameObject.GetComponent<GameManager>();
        gameManager.UpdateCurrentSettings(CurrentTile, CurrentCellPrefab);
    }
    // Update is called once per frame
    void Update()
    {
        
        if(Input.GetKeyDown(KeyCode.Alpha1)){
            CurrentTile = Tiles[0];
            CurrentCellPrefab = CellPrefabs[0];
            gameManager.UpdateCurrentSettings(CurrentTile, CurrentCellPrefab);
            gameManager.brushSize = gameManager.startBrushSize;
        }
        if(Input.GetKeyDown(KeyCode.Alpha2)){
            CurrentTile = Tiles[1];
            CurrentCellPrefab = CellPrefabs[1];
            gameManager.UpdateCurrentSettings(CurrentTile, CurrentCellPrefab);
            gameManager.brushSize = gameManager.startBrushSize;
        }
        if(Input.GetKeyDown(KeyCode.Alpha3)){
            CurrentTile = Tiles[2];
            CurrentCellPrefab = CellPrefabs[2];
            gameManager.UpdateCurrentSettings(CurrentTile, CurrentCellPrefab);
            gameManager.brushSize = gameManager.startBrushSize;
        }
        if(Input.GetKeyDown(KeyCode.Alpha4)){
            CurrentTile = Tiles[3];
            CurrentCellPrefab = CellPrefabs[3];
            gameManager.UpdateCurrentSettings(CurrentTile, CurrentCellPrefab);
            gameManager.brushSize = gameManager.startBrushSize;
        }
        if(Input.GetKeyDown(KeyCode.Alpha5)){
            CurrentTile = Tiles[4];
            CurrentCellPrefab = CellPrefabs[4];
            gameManager.UpdateCurrentSettings(CurrentTile, CurrentCellPrefab);
        }
        if(Input.GetKeyDown(KeyCode.Alpha6)){
            CurrentTile = Tiles[5];
            CurrentCellPrefab = CellPrefabs[5];
            gameManager.UpdateCurrentSettings(CurrentTile, CurrentCellPrefab);
            gameManager.brushSize = gameManager.startBrushSize + 1;
        }
        if(Input.GetKeyDown(KeyCode.Alpha7)){
            CurrentTile = Tiles[6];
            CurrentCellPrefab = CellPrefabs[6];
            gameManager.UpdateCurrentSettings(CurrentTile, CurrentCellPrefab);
            gameManager.brushSize = gameManager.startBrushSize + 1;
        }
    }
}
