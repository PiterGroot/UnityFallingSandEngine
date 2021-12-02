using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{   
    [HideInInspector]public int startBrushSize;
    private Vector3 mousePos;
    private Tile CurrentTile, waterTile;
    private GameObject CellPrefab, waterCell;
   
    [SerializeField]private int moveSpeed;
    [SerializeField]private Tilemap CellGrid;
    public bool UsingFire;
    public int brushSize;
    public float SIMULATION_SPEED = .05f;
    public float killBorder, killBorderUp;

    private void Awake() {
        startBrushSize = brushSize;
    }
    private void Update() {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;
        killBorder = (80 + Camera.main.transform.position.y) - 150;
        killBorderUp = (80 + Camera.main.transform.position.y);
        if(Input.GetKeyDown(KeyCode.Tab)){
            SceneManager.LoadScene("Simulation");
        }
        if(Input.GetKey(KeyCode.Mouse0)){
            if(Input.GetKey(KeyCode.LeftShift)){
                CheckGridWater(mousePos, brushSize);
            }
            else{
                CheckGrid(mousePos, brushSize);
                if(CurrentTile.name == "FireTile" || CurrentTile.name == "FireCell"){
                    UsingFire = true;
                }
                else{
                    UsingFire = false;
                }
                
            }
        }
        
        if(Input.GetKey(KeyCode.Mouse1)){
            for (int x = -(int)brushSize; x < brushSize; x++){
                for (int y = -(int)brushSize; y < brushSize; y++){
                    Vector3Int gridPos = CellGrid.WorldToCell(mousePos + new Vector3(x, y, 0));
                    CellGrid.SetTile(gridPos, null);
                    CellGrid.RefreshTile(gridPos);
                }
            }
        }
    }
    private void CheckGrid(Vector3 Position, int brushSize){
        for (int x = -(int)brushSize; x < brushSize; x++){
            for (int y = -(int)brushSize; y < brushSize; y++){
                Vector3Int gridPos = CellGrid.WorldToCell(Position + new Vector3(x, y, 0));
                if(!CellGrid.HasTile(gridPos)){
                    //Vector3Int cellPosition = CellGrid.LocalToCell(mousePos);
                    GameObject newCell = Instantiate(CellPrefab, gridPos, transform.rotation);
                    Vector3Int snappedCellPosition = CellGrid.LocalToCell(newCell.transform.position);
                    newCell.transform.localPosition = CellGrid.GetCellCenterLocal(gridPos);
                    CellGrid.SetTile(snappedCellPosition, CurrentTile);
                    //CellGrid.RefreshTile(snappedCellPosition);
                    newCell.transform.parent = gameObject.transform;  
                }
            }
        }
    }
    private void CheckGridWater(Vector3 Position, int brushSize){
        for (int x = -(int)brushSize; x < brushSize; x++){
            for (int y = -(int)brushSize; y < brushSize; y++){
                Vector3Int gridPos = CellGrid.WorldToCell(Position + new Vector3(x, y, 0));
                if(!CellGrid.HasTile(gridPos)){
                    //Vector3Int cellPosition = CellGrid.LocalToCell(mousePos);
                    GameObject newCell = Instantiate(waterCell, gridPos, transform.rotation);
                    Vector3Int snappedCellPosition = CellGrid.LocalToCell(newCell.transform.position);
                    newCell.transform.localPosition = CellGrid.GetCellCenterLocal(gridPos);
                    CellGrid.SetTile(snappedCellPosition, waterTile);
                    //CellGrid.RefreshTile(snappedCellPosition);
                    newCell.transform.parent = gameObject.transform;  
                }
            }
        }
    }
    public void UpdateCurrentSettings(Tile currentTile, GameObject cellPrefab){
        CurrentTile = currentTile;
        CellPrefab = cellPrefab;
    }
}
