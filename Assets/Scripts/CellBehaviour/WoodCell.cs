using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class WoodCell : MonoBehaviour
{
    private Tilemap _tilemap;
    private GameManager _gameManager;
    public bool isDead;
    public bool isOnfire;
    [SerializeField]private Vector2 DieTimer;
    [SerializeField]private Tile currentTile;
    [SerializeField]private Tile fireTile;
    [SerializeField]private Tile lavaTile;
    [SerializeField]private GameObject smokeTile;
    
    private void Awake() {
        _gameManager = FindObjectOfType<GameManager>();
        _tilemap = GameObject.FindGameObjectWithTag("Tilemap").GetComponent<Tilemap>();

        InvokeRepeating("UpdateTile", GameManager.SIMULATION_SPEED, GameManager.SIMULATION_SPEED);
        InvokeRepeating("CheckForFire", 0, .2f);
    }
    //update order: down --> down-left --> down-right --> left --> right
    public void UpdateTile(){
        bool UpdateWood = (Random.value > .95);
        if(!isDead && isOnfire && UpdateWood){
            isDead = true;
            Invoke("SelfDestruct", Random.Range(DieTimer.x, DieTimer.y));
            Vector3Int cellPosition = _tilemap.LocalToCell(transform.position);
            _tilemap.SetTile(cellPosition, fireTile);
            _tilemap.SetTile(_tilemap.WorldToCell(new Vector3(transform.position.x, transform.position.y, 0)), fireTile);
        }
    }
    private void CheckForFire(){
        if(!isOnfire){
            bool Dissipate = true;
            if(Dissipate && _gameManager.usingFire){
                Vector3Int xyPos = _tilemap.LocalToCell(transform.position);
                Vector3Int xyPosDown = _tilemap.WorldToCell(new Vector3(transform.position.x, transform.position.y -1, 0));
                Vector3Int xyPosUp = _tilemap.WorldToCell(new Vector3(transform.position.x, transform.position.y +1, 0));
                Vector3Int xyPosDownLeft = _tilemap.WorldToCell(new Vector3(transform.position.x - 1, transform.position.y -1, 0));
                Vector3Int xyPosDownRight = _tilemap.WorldToCell(new Vector3(transform.position.x + 1, transform.position.y -1, 0));
                Vector3Int xyPosLeft = _tilemap.WorldToCell(new Vector3(transform.position.x - 1, transform.position.y, 0));
                Vector3Int xyPosRight = _tilemap.WorldToCell(new Vector3(transform.position.x + 1, transform.position.y, 0));
                Vector3Int xyPosUpLeft = _tilemap.WorldToCell(new Vector3(transform.position.x - 1, transform.position.y +1, 0));
                Vector3Int xyPosUpRight = _tilemap.WorldToCell(new Vector3(transform.position.x + 1, transform.position.y +1, 0));
                try{
                    //fire
                    if(_tilemap.GetTile(xyPos) == fireTile || _tilemap.GetTile(xyPos) == lavaTile){
                        isOnfire = true;
                    }
                    if(_tilemap.GetTile(xyPosLeft) == fireTile || _tilemap.GetTile(xyPosLeft) == lavaTile){
                        isOnfire = true;
                    }
                    if(_tilemap.GetTile(xyPosRight) == fireTile || _tilemap.GetTile(xyPosRight) == lavaTile){
                        isOnfire = true;
                    }
                    if(_tilemap.GetTile(xyPosDown) == fireTile || _tilemap.GetTile(xyPosDown) == lavaTile){
                        isOnfire = true;
                    }
                    if(_tilemap.GetTile(xyPosUp) == fireTile || _tilemap.GetTile(xyPosUp) == lavaTile){
                        isOnfire = true;
                    }
                    else if(_tilemap.GetTile(xyPosUpRight) == fireTile || _tilemap.GetTile(xyPosUpRight) == lavaTile){
                        isOnfire = true;
                    }
                    else if(_tilemap.GetTile(xyPosUpLeft) == fireTile || _tilemap.GetTile(xyPosUpLeft) == lavaTile){
                        isOnfire = true;
                    }
                    else if(_tilemap.GetTile(xyPosDownRight) == fireTile || _tilemap.GetTile(xyPosDownRight) == lavaTile){
                        isOnfire = true;
                    }
                    else if(_tilemap.GetTile(xyPosDownLeft) == fireTile || _tilemap.GetTile(xyPosDownLeft) == lavaTile){
                        isOnfire = true;
                    }
                }
                catch{
                   
                }
            }
        }
    }
    private void SelfDestruct(){
        Vector3Int cellPosition = _tilemap.LocalToCell(transform.position);
        _tilemap.SetTile(cellPosition, null);
        _tilemap.SetTile(_tilemap.WorldToCell(new Vector3(transform.position.x - 1, transform.position.y, 0)), null);
        bool smoke = (Random.value > .8);
        _gameManager.simulatedCells--;
        if(smoke){
            _gameManager.simulatedCells++;
            Instantiate(smokeTile, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }
    private void OnMouseOver() {
        if(Input.GetKey(KeyCode.Mouse1)){
             _gameManager.simulatedCells--;
            Vector3Int cellPosition = _tilemap.LocalToCell(transform.position);
            _tilemap.SetTile(cellPosition, null);
            Destroy(gameObject);
        }
    }
}
