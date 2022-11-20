using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GassCell : MonoBehaviour
{
    private Tilemap _tilemap;

    private GameManager _gameManager;
    [SerializeField] private Tile currentTile;
    [SerializeField] private Tile waterTile;
    [SerializeField] private Tile lavaTile;

    private void Awake()
    {
        _gameManager = FindObjectOfType<GameManager>();
        _tilemap = GameObject.FindGameObjectWithTag("Tilemap").GetComponent<Tilemap>();

        InvokeRepeating("UpdateTile", GameManager.SIMULATION_SPEED, GameManager.SIMULATION_SPEED);
    }
    //update order: down --> down-left --> down-right --> left --> right
    public void UpdateTile()
    {
        Vector3Int xyPosUp = _tilemap.WorldToCell(new Vector3(transform.position.x, transform.position.y + 1, 0));
        Vector3Int xyPosUpLeft = _tilemap.WorldToCell(new Vector3(transform.position.x - 1, transform.position.y + 1, 0));
        Vector3Int xyPosUpRight = _tilemap.WorldToCell(new Vector3(transform.position.x + 1, transform.position.y + 1, 0));
        Vector3Int xyPosLeft = _tilemap.WorldToCell(new Vector3(transform.position.x - 1, transform.position.y, 0));
        Vector3Int xyPosRight = _tilemap.WorldToCell(new Vector3(transform.position.x + 1, transform.position.y, 0));

        if (_tilemap.GetTile(xyPosUp) == waterTile)
        {
            Vector3Int cellPosition = _tilemap.LocalToCell(transform.position);
            _tilemap.SetTile(cellPosition, null);
            transform.position = new Vector3(transform.position.x, transform.position.y + 1, 0);
            Vector3Int _cellPosition = _tilemap.LocalToCell(transform.position);
            _tilemap.SetTile(_cellPosition, currentTile);
        }
        else if (_tilemap.GetTile(xyPosUp) == lavaTile)
        {
            Vector3Int cellPosition = _tilemap.LocalToCell(transform.position);
            _tilemap.SetTile(cellPosition, null);
            transform.position = new Vector3(transform.position.x, transform.position.y + 1, 0);
            Vector3Int _cellPosition = _tilemap.LocalToCell(transform.position);
            _tilemap.SetTile(_cellPosition, currentTile);
        }
        else if (!_tilemap.HasTile(xyPosUp))
        {
            //movedown
            Vector3Int cellPosition = _tilemap.LocalToCell(transform.position);
            _tilemap.SetTile(cellPosition, null);
            _tilemap.SetTile(xyPosUp, currentTile);
            transform.position = new Vector3(transform.position.x, transform.position.y + 1, 0);
            if (transform.position.y > _gameManager.killBorderUp)
            {
                _tilemap.SetTile(xyPosUp, null);
                _gameManager.simulatedCells--;
                Destroy(gameObject);
            }
        }
        else if (!_tilemap.HasTile(xyPosUpLeft))
        {
            //down-left
            Vector3Int cellPosition = _tilemap.LocalToCell(transform.position);
            _tilemap.SetTile(cellPosition, null);
            _tilemap.SetTile(xyPosUpLeft, currentTile);
            transform.position = new Vector3(transform.position.x - 1, transform.position.y + 1, 0);
        }
        else if (!_tilemap.HasTile(xyPosUpRight))
        {
            //down-right
            Vector3Int cellPosition = _tilemap.LocalToCell(transform.position);
            _tilemap.SetTile(cellPosition, null);
            _tilemap.SetTile(xyPosUpRight, currentTile);
            transform.position = new Vector3(transform.position.x + 1, transform.position.y + 1, 0);
        }
        else if (!_tilemap.HasTile(xyPosLeft))
        {
            //left
            Vector3Int cellPosition = _tilemap.LocalToCell(transform.position);
            _tilemap.SetTile(cellPosition, null);
            _tilemap.SetTile(xyPosLeft, currentTile);
            transform.position = _tilemap.WorldToCell(new Vector3(transform.position.x - 1, transform.position.y, 0));
        }
        else if (!_tilemap.HasTile(xyPosRight))
        {
            //right
            Vector3Int cellPosition = _tilemap.LocalToCell(transform.position);
            _tilemap.SetTile(cellPosition, null);
            _tilemap.SetTile(xyPosRight, currentTile);
            transform.position = new Vector3(transform.position.x + 1, transform.position.y, 0);
        }
        //TODO another left right hastile check, but now with a check if under 3 cells (downleft, downright and down) are occupied
        else
        {
            //isDead = true;
            return;
        }
    }
    private void OnMouseOver()
    {
        if (Input.GetKey(KeyCode.Mouse1))
        {
            _gameManager.simulatedCells--;
            Vector3Int cellPosition = _tilemap.LocalToCell(transform.position);
            _tilemap.SetTile(cellPosition, null);
            Destroy(gameObject);
        }
    }
}
