using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SandCell : MonoBehaviour
{
    private bool Activate;
    private Tilemap Tilemap;
    private float simulationSpeed;
    public bool isDead;
    private GameManager gameManager;
    [SerializeField]private Tile thisTile;
    
    private void Awake() {
        gameManager = FindObjectOfType<GameManager>();
        Tilemap = GameObject.FindGameObjectWithTag("Tilemap").GetComponent<Tilemap>();
        simulationSpeed = gameManager.SIMULATION_SPEED;
        InvokeRepeating("UpdateTile", simulationSpeed, simulationSpeed);
    }
    //update order: down --> down-left --> down-right --> left --> right
    public void UpdateTile(){
        if(!isDead){
            Vector3Int xyPosDown = Tilemap.WorldToCell(new Vector3(transform.position.x, transform.position.y -1, 0));
            Vector3Int xyPosDownLeft = Tilemap.WorldToCell(new Vector3(transform.position.x - 1, transform.position.y -1, 0));
            Vector3Int xyPosDownRight = Tilemap.WorldToCell(new Vector3(transform.position.x + 1, transform.position.y -1, 0));
            Vector3Int xyPosLeft = Tilemap.WorldToCell(new Vector3(transform.position.x - 1, transform.position.y, 0));
            Vector3Int xyPosRight = Tilemap.WorldToCell(new Vector3(transform.position.x + 1, transform.position.y, 0));
            if(!Tilemap.HasTile(xyPosDown)){
                //movedown
                Vector3Int cellPosition = Tilemap.LocalToCell(transform.position);
                Tilemap.SetTile(cellPosition, null);
                Tilemap.SetTile(xyPosDown, thisTile);
                transform.position = new Vector3(transform.position.x, transform.position.y -1,0);
                if(transform.position.y < gameManager.killBorder){
                    Tilemap.SetTile(xyPosDown, null);
                    gameManager.SimulatedCells--;
                    Destroy(gameObject);
                }
            }
            else if(!Tilemap.HasTile(xyPosDownLeft)){
                //down-left
                Vector3Int cellPosition = Tilemap.LocalToCell(transform.position);
                Tilemap.SetTile(cellPosition, null);
                Tilemap.SetTile(xyPosDownLeft, thisTile);
                transform.position = new Vector3(transform.position.x - 1, transform.position.y -1, 0);
            }
            else if(!Tilemap.HasTile(xyPosDownRight)){
                //down-right
                Vector3Int cellPosition = Tilemap.LocalToCell(transform.position);
                Tilemap.SetTile(cellPosition, null);
                Tilemap.SetTile(xyPosDownRight, thisTile);
                transform.position = new Vector3(transform.position.x + 1, transform.position.y -1, 0);
            }
            //TODO another left right hastile check, but now with a check if under 3 cells (downleft, downright and down) are occupied
            else{
                //
                return;
            }
        }
    }
    private void OnMouseOver() {
        if(Input.GetKey(KeyCode.Mouse1)){
             gameManager.SimulatedCells--;
             Vector3Int cellPosition = Tilemap.LocalToCell(transform.position);
            Tilemap.SetTile(cellPosition, null);
            Destroy(gameObject);
        }
    }
}
