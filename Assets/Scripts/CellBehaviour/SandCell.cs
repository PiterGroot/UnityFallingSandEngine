using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SandCell : MonoBehaviour
{
    private Tilemap _tilemap;

    private GameManager _gameManager;
    [SerializeField] private Tile currentTile;

    private void Awake()
    {
        _gameManager = FindObjectOfType<GameManager>();
        _tilemap = GameObject.FindGameObjectWithTag("Tilemap").GetComponent<Tilemap>();
        InvokeRepeating("UpdateTile", GameManager.SIMULATION_SPEED, GameManager.SIMULATION_SPEED);
    }
    
    //update order: down --> down-left --> down-right 
    public void UpdateTile()
    {
        Vector3Int xyPosDown = _tilemap.WorldToCell(new Vector3(transform.position.x, transform.position.y - 1, 0));
        Vector3Int xyPosDownLeft = _tilemap.WorldToCell(new Vector3(transform.position.x - 1, transform.position.y - 1, 0));
        Vector3Int xyPosDownRight = _tilemap.WorldToCell(new Vector3(transform.position.x + 1, transform.position.y - 1, 0));
        Vector3Int xyPosLeft = _tilemap.WorldToCell(new Vector3(transform.position.x - 1, transform.position.y, 0));
        Vector3Int xyPosRight = _tilemap.WorldToCell(new Vector3(transform.position.x + 1, transform.position.y, 0));
        if (!_tilemap.HasTile(xyPosDown))
        {
            //movedown
            Vector3Int cellPosition = _tilemap.LocalToCell(transform.position);
            _tilemap.SetTile(cellPosition, null);
            _tilemap.SetTile(xyPosDown, currentTile);
            transform.position = new Vector3(transform.position.x, transform.position.y - 1, 0);
            if (transform.position.y < _gameManager.killBorder)
            {
                _tilemap.SetTile(xyPosDown, null);
                _gameManager.simulatedCells--;
                Destroy(gameObject);
            }
        }
        else if (!_tilemap.HasTile(xyPosDownLeft))
        {
            //down-left
            Vector3Int cellPosition = _tilemap.LocalToCell(transform.position);
            _tilemap.SetTile(cellPosition, null);
            _tilemap.SetTile(xyPosDownLeft, currentTile);
            transform.position = new Vector3(transform.position.x - 1, transform.position.y - 1, 0);
        }
        else if (!_tilemap.HasTile(xyPosDownRight))
        {
            //down-right
            Vector3Int cellPosition = _tilemap.LocalToCell(transform.position);
            _tilemap.SetTile(cellPosition, null);
            _tilemap.SetTile(xyPosDownRight, currentTile);
            transform.position = new Vector3(transform.position.x + 1, transform.position.y - 1, 0);
        }
        //TODO another left right hastile check, but now with a check if under 3 cells (downleft, downright and down) are occupied
        else
        {
            //
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
