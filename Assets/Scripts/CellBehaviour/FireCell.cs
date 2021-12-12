using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FireCell : MonoBehaviour
{
    private Tilemap Tilemap;
    private float simulationSpeed;
    public bool isDead;
    private GameManager gameManager;
    [SerializeField]private Vector2 DieTimer;
    [SerializeField]private Tile thisTile;
    [SerializeField]private GameObject smokeCell;
    
    private void Awake() {
        gameManager = FindObjectOfType<GameManager>();
        Tilemap = GameObject.FindGameObjectWithTag("Tilemap").GetComponent<Tilemap>();
        simulationSpeed = gameManager.SIMULATION_SPEED;
        InvokeRepeating("UpdateTile", simulationSpeed, simulationSpeed);
        Invoke("SelfDestruct", Random.Range(DieTimer.x, DieTimer.y));
    }
    //update order: down --> down-left --> down-right --> left --> right
    public void UpdateTile(){
        bool Update = (Random.value < .7);
        if(!isDead && Update){
            Vector3Int xyPosUp = Tilemap.WorldToCell(new Vector3(transform.position.x, transform.position.y +1, 0));
            Vector3Int xyPosUpLeft = Tilemap.WorldToCell(new Vector3(transform.position.x - 1, transform.position.y +1, 0));
            Vector3Int xyPosUpRight = Tilemap.WorldToCell(new Vector3(transform.position.x + 1, transform.position.y +1, 0));
            Vector3Int xyPosLeft = Tilemap.WorldToCell(new Vector3(transform.position.x - 1, transform.position.y, 0));
            Vector3Int xyPosRight = Tilemap.WorldToCell(new Vector3(transform.position.x + 1, transform.position.y, 0));
            if(!Tilemap.HasTile(xyPosUp)){
                //movedown
                Vector3Int cellPosition = Tilemap.LocalToCell(transform.position);
                Tilemap.SetTile(cellPosition, null);
                Tilemap.SetTile(xyPosUp, thisTile);
                transform.position = new Vector3(transform.position.x, transform.position.y +1,0);
                if(transform.position.y > gameManager.killBorderUp){
                    Tilemap.SetTile(xyPosUp, null);
                    gameManager.SimulatedCells--;
                    Destroy(gameObject);
                }
            }
            else if(!Tilemap.HasTile(xyPosUpLeft)){
                //down-left
                Vector3Int cellPosition = Tilemap.LocalToCell(transform.position);
                Tilemap.SetTile(cellPosition, null);
                Tilemap.SetTile(xyPosUpLeft, thisTile);
                transform.position = new Vector3(transform.position.x - 1, transform.position.y +1, 0);
            }
            else if(!Tilemap.HasTile(xyPosUpRight)){
                //down-right
                Vector3Int cellPosition = Tilemap.LocalToCell(transform.position);
                Tilemap.SetTile(cellPosition, null);
                Tilemap.SetTile(xyPosUpRight, thisTile);
                transform.position = new Vector3(transform.position.x + 1, transform.position.y +1, 0);
            }
            else if(!Tilemap.HasTile(xyPosLeft)){
                //left
                Vector3Int cellPosition = Tilemap.LocalToCell(transform.position);
                Tilemap.SetTile(cellPosition, null);
                Tilemap.SetTile(xyPosLeft, thisTile);
                transform.position = Tilemap.WorldToCell(new Vector3(transform.position.x - 1, transform.position.y, 0));
            }
            else if(!Tilemap.HasTile(xyPosRight)){
                //right
                Vector3Int cellPosition = Tilemap.LocalToCell(transform.position);
                    Tilemap.SetTile(cellPosition, null);
                    Tilemap.SetTile(xyPosRight, thisTile);
                    transform.position = new Vector3(transform.position.x + 1, transform.position.y, 0);
            }
            //TODO another left right hastile check, but now with a check if under 3 cells (downleft, downright and down) are occupied
            else{
                //isDead = true;
                return;
            }
        }
    }
    private void SelfDestruct(){
        isDead = true;
        Vector3Int cellPosition = Tilemap.LocalToCell(transform.position);
        Tilemap.SetTile(cellPosition, null);
        bool Smoke = (Random.value > .5);
        if(Smoke){
            Instantiate(smokeCell, transform.position, Quaternion.identity);
        }
        gameManager.SimulatedCells--;
        Destroy(gameObject);
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
