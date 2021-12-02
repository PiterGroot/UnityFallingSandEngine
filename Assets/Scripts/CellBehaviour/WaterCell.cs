using UnityEngine;
using UnityEngine.Tilemaps;


public class WaterCell : MonoBehaviour
{
    private bool isLava;
    private int deathCounter;
    private float simulationSpeed;
    private Tilemap Tilemap;
    public bool isDead;
    private GameManager gameManager;
    [SerializeField]private Tile thisTile;
    [SerializeField]private Tile fireTile;
     [SerializeField]private Tile lavaTile;
      [SerializeField]private Tile obsidian;
    [SerializeField] private GameObject smokeCell;
    [SerializeField]private Sprite cellSprite;
    
    private void Awake() {
        gameManager = FindObjectOfType<GameManager>();
        simulationSpeed = gameManager.SIMULATION_SPEED;
        Tilemap = GameObject.FindGameObjectWithTag("Tilemap").GetComponent<Tilemap>();
        InvokeRepeating("UpdateTile", gameManager.SIMULATION_SPEED, gameManager.SIMULATION_SPEED);
        InvokeRepeating("UpdateVaporize", .35f, .35f);
    }
    //update order: down --> down-left --> down-right --> left --> right
    public void UpdateTile(){
        if(!isDead){
            Vector3Int xyPosUp = Tilemap.WorldToCell(new Vector3(transform.position.x, transform.position.y +1, 0));
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
        else if(isDead && !isLava){
            Vector3Int cellPosition = Tilemap.LocalToCell(transform.position);
            Tilemap.SetTile(cellPosition, null);
            //Tilemap.SetTile(Tilemap.WorldToCell(new Vector3(transform.position.x - 1, transform.position.y, 0)), null);
            Instantiate(smokeCell, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        else if(isDead && isLava){
            //contact with lava
            Tile obsidian_ = ScriptableObject.CreateInstance<Tile>();
            obsidian_.color = Color.black;
            obsidian_.sprite = cellSprite;
            Tilemap.SetTile(Tilemap.LocalToCell(transform.position), obsidian_);
            Instantiate(smokeCell, Tilemap.WorldToCell(new Vector3(transform.position.x, transform.position.y +1, 0)), Quaternion.identity);
            Destroy(gameObject);
        }
    }
    private void UpdateVaporize(){
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
            if(Tilemap.GetTile(xyPos) == fireTile){
                isDead = true;
                isLava = false;
            }
            if(Tilemap.GetTile(xyPosLeft) == fireTile){
                isDead = true;
                isLava = false;
            }
            if(Tilemap.GetTile(xyPosRight) == fireTile){
                isDead = true;
                isLava = false;
            }
            if(Tilemap.GetTile(xyPosDown) == fireTile){
                isDead = true;
                isLava = false;
            }
            if(Tilemap.GetTile(xyPosUp) == fireTile){
                isDead = true;
                isLava = false;
            }
            else if(Tilemap.GetTile(xyPosUpRight) == fireTile){
                isDead = true;
                isLava = false;
            }
            else if(Tilemap.GetTile(xyPosUpLeft) == fireTile){
                isDead = true;
                isLava = false;
            }
            else if(Tilemap.GetTile(xyPosDownRight) == fireTile){
                isDead = true;
                isLava = false;
            }
            else if(Tilemap.GetTile(xyPosDownLeft) == fireTile){
                isDead = true;
                isLava = false;
            }
            //lava
            if(Tilemap.GetTile(xyPos) == lavaTile){
                isDead = true;
                isLava = true;
            }
            if(Tilemap.GetTile(xyPosLeft) == lavaTile){
                isDead = true;
                 isLava = true;
            }
            if(Tilemap.GetTile(xyPosRight) == lavaTile){
                isDead = true;
                 isLava = true;
            }
            if(Tilemap.GetTile(xyPosDown) == lavaTile){
                isDead = true;
                isLava = true;
            }
            if(Tilemap.GetTile(xyPosUp) == lavaTile){
                isDead = true;
                isLava = true;
            }
            else if(Tilemap.GetTile(xyPosUpRight) == lavaTile){
                isDead = true;
                 isLava = true;
            }
            else if(Tilemap.GetTile(xyPosUpLeft) == lavaTile){
                isDead = true;
                 isLava = true;
            }
            else if(Tilemap.GetTile(xyPosDownRight) == lavaTile){
                isDead = true;
                 isLava = true;
            }
            else if(Tilemap.GetTile(xyPosDownLeft) == lavaTile){
                isDead = true;
                 isLava = true;
            }
        }
        catch{
                   
        }
    }
}
