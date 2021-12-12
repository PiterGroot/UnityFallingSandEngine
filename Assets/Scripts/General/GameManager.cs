using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{   
    [HideInInspector]public bool canClick;
    [HideInInspector]public bool UsingFire;
    [HideInInspector]public int startBrushSize;
    private Vector3 mousePos;
    private Tile CurrentTile, waterTile;
    private GameObject CellPrefab, waterCell;
    [SerializeField]int inventoryCount;
   
    [SerializeField]private int moveSpeed;
    [SerializeField]private Tilemap CellGrid;
    [SerializeField]private Animator inventoryAnim;

    public int brushSize;
    public float SIMULATION_SPEED = .05f;
    public float killBorder, killBorderUp;
    public int SimulatedCells;

    private void Awake() {
        startBrushSize = brushSize;
    }
    private void Update() {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;
        killBorder = (80 + Camera.main.transform.position.y) - 150;
        killBorderUp = (80 + Camera.main.transform.position.y);

        HandleInput();
    }
    private void CheckGrid(Vector3 Position, int brushSize){
        for (int x = -(int)brushSize; x < brushSize; x++){
            for (int y = -(int)brushSize; y < brushSize; y++){
                Vector3Int gridPos = CellGrid.WorldToCell(Position + new Vector3(x, y, 0));
                if(!CellGrid.HasTile(gridPos)){
                    SimulatedCells++;
                    GameObject newCell = Instantiate(CellPrefab, gridPos, transform.rotation);
                    Vector3Int snappedCellPosition = CellGrid.LocalToCell(newCell.transform.position);
                    newCell.transform.localPosition = CellGrid.GetCellCenterLocal(gridPos);
                    CellGrid.SetTile(snappedCellPosition, CurrentTile);
                    newCell.transform.parent = gameObject.transform;  
                }
            }
        }
    }
    public void UpdateCurrentSettings(Tile currentTile, GameObject cellPrefab){
        CurrentTile = currentTile;
        CellPrefab = cellPrefab;
    }
    private void HandleInput(){
        if(Input.GetKeyDown(KeyCode.E)){
            inventoryCount++;
            switch(inventoryCount){
                case 0:
                break;
                case 1:
                inventoryAnim.SetTrigger("Open");
                break;
                case 2:
                inventoryCount = 0;
                inventoryAnim.SetTrigger("Close");
                break;
            }
        }
        if(Input.GetKeyDown(KeyCode.Tab)){
            SceneManager.LoadScene("Simulation");
        }
        if(Input.GetKey(KeyCode.Mouse0) && canClick){
            CheckGrid(mousePos, brushSize);
            if(CurrentTile.name == "FireTile" || CurrentTile.name == "FireCell"){
                UsingFire = true;
            }
        }
        
        if(Input.GetKey(KeyCode.Mouse1) && canClick){
            for (int x = -(int)brushSize; x < brushSize; x++){
                for (int y = -(int)brushSize; y < brushSize; y++){
                    Vector3Int gridPos = CellGrid.WorldToCell(mousePos + new Vector3(x, y, 0));
                    CellGrid.SetTile(gridPos, null);
                    CellGrid.RefreshTile(gridPos);
                }
            }
        }
    }
}
