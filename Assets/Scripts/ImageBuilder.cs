using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ImageBuilder : MonoBehaviour
{
    private GameManager gameManager;
    [SerializeField]private Tilemap Tilemap;
    public Texture2D map;
    public ColorToTile[] colorMappings;
    // Update is called once per frame
    private void Awake() {
        gameManager = gameObject.GetComponent<GameManager>();
         GenerateLevel();
    }
    private void GenerateLevel(){
        for (int x = 0; x < map.width; x++)
        {
            for (int y = 0; y < map.height; y++)
            {
                PlaceTile(x, y);
            }
        }
    }

    private void PlaceTile(int x, int y){
        Color pixelColor = map.GetPixel(x, y);
        if(pixelColor.a ==0){
            return;
        }
        foreach(ColorToTile colorToTile in colorMappings){
            if(colorToTile.Color.Equals(pixelColor)){
                gameManager.simulatedCells++;
                Instantiate(colorToTile.Cell, new Vector3(x, y, 0), Quaternion.identity, transform);
                Vector3Int snappedCellPosition = Tilemap.LocalToCell(new Vector3(x, y, 0));
                Tilemap.SetTile(snappedCellPosition, colorToTile.Tile);
            }
        }
    }
}
