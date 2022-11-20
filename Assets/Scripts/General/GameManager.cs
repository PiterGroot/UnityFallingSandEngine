using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{   
    private bool _showInventory = false;
    private Tile _currentTile;
    private GameObject _cellPrefab;

    [HideInInspector]public bool usingFire;
    [HideInInspector]public bool canClick;
    [HideInInspector]public int startBrushSize;

    [SerializeField]private Tilemap CellGrid;
    [SerializeField]private Animator inventoryAnim;

    public int brushSize;
    public float killBorder, killBorderUp;
    public int simulatedCells;

    public static float SIMULATION_SPEED = .02f;
    
    private void Awake() {
        usingFire = true;
        startBrushSize = brushSize;
    }

    private void Update() {
        UpdateCameraBorder();
        HandleInput();
    }

    private void PlaceCells(Vector3 Position, int brushSize){
        for (int x = -(int)brushSize; x < brushSize; x++){
            for (int y = -(int)brushSize; y < brushSize; y++){
                Vector3Int gridPos = CellGrid.WorldToCell(Position + new Vector3(x, y, 0));
                if(!CellGrid.HasTile(gridPos)){
                    simulatedCells++;

                    GameObject newCell = Instantiate(_cellPrefab, gridPos, transform.rotation);
                    Vector3Int snappedCellPosition = CellGrid.LocalToCell(newCell.transform.position);

                    newCell.transform.localPosition = CellGrid.GetCellCenterLocal(gridPos);
                    CellGrid.SetTile(snappedCellPosition, _currentTile);
                    newCell.transform.parent = gameObject.transform;  
                }
            }
        }
    }
    private void UpdateCameraBorder()
    {
        killBorder = (80 + Camera.main.transform.position.y) - 150;
        killBorderUp = (80 + Camera.main.transform.position.y);
    }

    public void UpdateCurrentSettings(Tile currentTile, GameObject cellPrefab){
        _currentTile = currentTile;
        _cellPrefab = cellPrefab;
    }

    private void HandleInput(){

        //show elements UI
        if (Input.GetKeyDown(KeyCode.E)){
            _showInventory = !_showInventory;

            if (_showInventory) inventoryAnim.SetTrigger("Open");
            else inventoryAnim.SetTrigger("Close");
        }

        //reset scene
        if(Input.GetKeyDown(KeyCode.Tab)){
            SceneManager.LoadScene("Simulation");
        }

        //place cells
        if(Input.GetKey(KeyCode.Mouse0) && canClick){
            PlaceCells(GetMousePos(), brushSize);
        }
        
        //destroy cells
        if(Input.GetKey(KeyCode.Mouse1) && canClick){
            for (int x = -(int)brushSize; x < brushSize; x++){
                for (int y = -(int)brushSize; y < brushSize; y++){
                    Vector3Int gridPos = CellGrid.WorldToCell(GetMousePos() + new Vector3(x, y, 0));
                    CellGrid.SetTile(gridPos, null);
                    CellGrid.RefreshTile(gridPos);
                }
            }
        }
    }
    //get current mouse position 
    private Vector3 GetMousePos()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;
        return mousePos;
    }
}
