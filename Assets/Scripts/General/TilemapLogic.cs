using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapLogic : MonoBehaviour
{
    [ContextMenu("ClearTileMap")]
    public void ClearTilemap(){
        GetComponent<Tilemap>().ClearAllTiles();
    }

}
