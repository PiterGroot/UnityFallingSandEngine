using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class WoodCell : MonoBehaviour
{
    private Tile downTile;
    private Tilemap Tilemap;
    private float simulationSpeed;
    private GameManager gameManager;
    public bool isDead;
    public bool isOnfire;
    [SerializeField]private Vector2 DieTimer;
    [SerializeField]private Tile thisTile;
    [SerializeField]private Tile fireTile;
    [SerializeField]private Tile lavaTile;
    [SerializeField]private GameObject fireCell;
    [SerializeField]private GameObject smokeTile;
    
    private void Awake() {
        gameManager = FindObjectOfType<GameManager>();
        Tilemap = GameObject.FindGameObjectWithTag("Tilemap").GetComponent<Tilemap>();
        simulationSpeed = gameManager.SIMULATION_SPEED;
        InvokeRepeating("UpdateTile", simulationSpeed, simulationSpeed);
        InvokeRepeating("CheckForFire", 0, .2f);
    }
    //update order: down --> down-left --> down-right --> left --> right
    public void UpdateTile(){
        bool UpdateWood = (Random.value > .95);
        if(!isDead && isOnfire && UpdateWood){
            isDead = true;
            Invoke("SelfDestruct", Random.Range(DieTimer.x, DieTimer.y));
            Vector3Int cellPosition = Tilemap.LocalToCell(transform.position);
            Tilemap.SetTile(cellPosition, fireTile);
            Tilemap.SetTile(Tilemap.WorldToCell(new Vector3(transform.position.x, transform.position.y, 0)), fireTile);
        }
    }
    private void CheckForFire(){
        if(!isOnfire){
            bool Dissipate = true;
            if(Dissipate && gameManager.UsingFire){
                Vector3Int xyPos = Tilemap.LocalToCell(transform.position);
                Vector3Int xyPosDown = Tilemap.WorldToCell(new Vector3(transform.position.x, transform.position.y -1, 0));
                Vector3Int xyPosUp = Tilemap.WorldToCell(new Vector3(transform.position.x, transform.position.y +1, 0));
                Vector3Int xyPosDownLeft = Tilemap.WorldToCell(new Vector3(transform.position.x - 1, transform.position.y -1, 0));
                Vector3Int xyPosDownRight = Tilemap.WorldToCell(new Vector3(transform.position.x + 1, transform.position.y -1, 0));
                Vector3Int xyPosLeft = Tilemap.WorldToCell(new Vector3(transform.position.x - 1, transform.position.y, 0));
                Vector3Int xyPosRight = Tilemap.WorldToCell(new Vector3(transform.position.x + 1, transform.position.y, 0));
                Vector3Int xyPosUpLeft = Tilemap.WorldToCell(new Vector3(transform.position.x - 1, transform.position.y +1, 0));
                Vector3Int xyPosUpRight = Tilemap.WorldToCell(new Vector3(transform.position.x + 1, transform.position.y +1, 0));
                try{
                    //fire
                    if(Tilemap.GetTile(xyPos) == fireTile || Tilemap.GetTile(xyPos) == lavaTile){
                        isOnfire = true;
                    }
                    if(Tilemap.GetTile(xyPosLeft) == fireTile || Tilemap.GetTile(xyPosLeft) == lavaTile){
                        isOnfire = true;
                    }
                    if(Tilemap.GetTile(xyPosRight) == fireTile || Tilemap.GetTile(xyPosRight) == lavaTile){
                        isOnfire = true;
                    }
                    if(Tilemap.GetTile(xyPosDown) == fireTile || Tilemap.GetTile(xyPosDown) == lavaTile){
                        isOnfire = true;
                    }
                    if(Tilemap.GetTile(xyPosUp) == fireTile || Tilemap.GetTile(xyPosUp) == lavaTile){
                        isOnfire = true;
                    }
                    else if(Tilemap.GetTile(xyPosUpRight) == fireTile || Tilemap.GetTile(xyPosUpRight) == lavaTile){
                        isOnfire = true;
                    }
                    else if(Tilemap.GetTile(xyPosUpLeft) == fireTile || Tilemap.GetTile(xyPosUpLeft) == lavaTile){
                        isOnfire = true;
                    }
                    else if(Tilemap.GetTile(xyPosDownRight) == fireTile || Tilemap.GetTile(xyPosDownRight) == lavaTile){
                        isOnfire = true;
                    }
                    else if(Tilemap.GetTile(xyPosDownLeft) == fireTile || Tilemap.GetTile(xyPosDownLeft) == lavaTile){
                        isOnfire = true;
                    }
                }
                catch{
                   
                }
            }
        }
    }
    private void SelfDestruct(){
        Vector3Int cellPosition = Tilemap.LocalToCell(transform.position);
        Tilemap.SetTile(cellPosition, null);
        Tilemap.SetTile(Tilemap.WorldToCell(new Vector3(transform.position.x - 1, transform.position.y, 0)), null);
        bool smoke = (Random.value > .8);
        if(smoke){
            Instantiate(smokeTile, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }
}
