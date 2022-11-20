using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FireCell : MonoBehaviour
{
    public bool isDead;
    private Tilemap _tilemap;

    private GameManager gameManager;
    [SerializeField] private Vector2 DieTimer;
    [SerializeField] private Tile thisTile;
    [SerializeField] private GameObject smokeCell;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        _tilemap = GameObject.FindGameObjectWithTag("Tilemap").GetComponent<Tilemap>();
        InvokeRepeating("UpdateTile", GameManager.SIMULATION_SPEED, GameManager.SIMULATION_SPEED);
        Invoke("SelfDestruct", Random.Range(DieTimer.x, DieTimer.y));
    }
    //update order: down --> down-left --> down-right --> left --> right
    public void UpdateTile()
    {
        bool update = (Random.value < .7);
        if (!isDead && update)
        {
            Vector3Int xyPosUp = _tilemap.WorldToCell(new Vector3(transform.position.x, transform.position.y + 1, 0));
            Vector3Int xyPosUpLeft = _tilemap.WorldToCell(new Vector3(transform.position.x - 1, transform.position.y + 1, 0));
            Vector3Int xyPosUpRight = _tilemap.WorldToCell(new Vector3(transform.position.x + 1, transform.position.y + 1, 0));
            Vector3Int xyPosLeft = _tilemap.WorldToCell(new Vector3(transform.position.x - 1, transform.position.y, 0));
            Vector3Int xyPosRight = _tilemap.WorldToCell(new Vector3(transform.position.x + 1, transform.position.y, 0));
            if (!_tilemap.HasTile(xyPosUp))
            {
                //movedown
                Vector3Int cellPosition = _tilemap.LocalToCell(transform.position);
                _tilemap.SetTile(cellPosition, null);
                _tilemap.SetTile(xyPosUp, thisTile);
                transform.position = new Vector3(transform.position.x, transform.position.y + 1, 0);
                if (transform.position.y > gameManager.killBorderUp)
                {
                    _tilemap.SetTile(xyPosUp, null);
                    gameManager.simulatedCells--;
                    Destroy(gameObject);
                }
            }
            else if (!_tilemap.HasTile(xyPosUpLeft))
            {
                //down-left
                Vector3Int cellPosition = _tilemap.LocalToCell(transform.position);
                _tilemap.SetTile(cellPosition, null);
                _tilemap.SetTile(xyPosUpLeft, thisTile);
                transform.position = new Vector3(transform.position.x - 1, transform.position.y + 1, 0);
            }
            else if (!_tilemap.HasTile(xyPosUpRight))
            {
                //down-right
                Vector3Int cellPosition = _tilemap.LocalToCell(transform.position);
                _tilemap.SetTile(cellPosition, null);
                _tilemap.SetTile(xyPosUpRight, thisTile);
                transform.position = new Vector3(transform.position.x + 1, transform.position.y + 1, 0);
            }
            else if (!_tilemap.HasTile(xyPosLeft))
            {
                //left
                Vector3Int cellPosition = _tilemap.LocalToCell(transform.position);
                _tilemap.SetTile(cellPosition, null);
                _tilemap.SetTile(xyPosLeft, thisTile);
                transform.position = _tilemap.WorldToCell(new Vector3(transform.position.x - 1, transform.position.y, 0));
            }
            else if (!_tilemap.HasTile(xyPosRight))
            {
                //right
                Vector3Int cellPosition = _tilemap.LocalToCell(transform.position);
                _tilemap.SetTile(cellPosition, null);
                _tilemap.SetTile(xyPosRight, thisTile);
                transform.position = new Vector3(transform.position.x + 1, transform.position.y, 0);
            }
            //TODO another left right hastile check, but now with a check if under 3 cells (downleft, downright and down) are occupied
            else
            {
                //isDead = true;
                return;
            }
        }
    }
    private void SelfDestruct()
    {
        isDead = true;
        Vector3Int cellPosition = _tilemap.LocalToCell(transform.position);
        _tilemap.SetTile(cellPosition, null);
        bool Smoke = (Random.value > .5);
        if (Smoke)
        {
            gameManager.simulatedCells++;
            Instantiate(smokeCell, transform.position, Quaternion.identity);
        }
        gameManager.simulatedCells--;
        Destroy(gameObject);
    }
    private void OnMouseOver()
    {
        if (Input.GetKey(KeyCode.Mouse1))
        {
            gameManager.simulatedCells--;
            Vector3Int cellPosition = _tilemap.LocalToCell(transform.position);
            _tilemap.SetTile(cellPosition, null);
            Destroy(gameObject);
        }
    }
}
