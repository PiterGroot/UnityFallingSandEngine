using UnityEngine;
using UnityEngine.Tilemaps;


public class WaterCell : MonoBehaviour
{
    private bool isLava;

    private Tilemap _tilemap;
    public bool isDead;
    private GameManager _gameManager;
    [SerializeField] private Tile currentTile;
    [SerializeField] private Tile fireTile;
    [SerializeField] private Tile lavaTile;
    [SerializeField] private Sprite cellSprite;
    [SerializeField] private GameObject smokeCell;

    private void Awake()
    {
        _gameManager = FindObjectOfType<GameManager>();

        _tilemap = GameObject.FindGameObjectWithTag("Tilemap").GetComponent<Tilemap>();
        InvokeRepeating("UpdateTile", GameManager.SIMULATION_SPEED, GameManager.SIMULATION_SPEED);
        InvokeRepeating("UpdateVaporize", .35f, .35f);
    }
    //update order: down --> down-left --> down-right --> left --> right
    public void UpdateTile()
    {
        if (!isDead)
        {
            Vector3Int xyPosUp = _tilemap.WorldToCell(new Vector3(transform.position.x, transform.position.y + 1, 0));
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
        else if (isDead && !isLava)
        {
            Vector3Int cellPosition = _tilemap.LocalToCell(transform.position);
            _tilemap.SetTile(cellPosition, null);
            //Tilemap.SetTile(Tilemap.WorldToCell(new Vector3(transform.position.x - 1, transform.position.y, 0)), null);
            Instantiate(smokeCell, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        else if (isDead && isLava)
        {
            //contact with lava
            Tile obsidian_ = ScriptableObject.CreateInstance<Tile>();
            obsidian_.color = Color.black;
            obsidian_.sprite = cellSprite;
            _tilemap.SetTile(_tilemap.LocalToCell(transform.position), obsidian_);
            Instantiate(smokeCell, _tilemap.WorldToCell(new Vector3(transform.position.x, transform.position.y + 1, 0)), Quaternion.identity);
            _gameManager.simulatedCells++;
            Destroy(gameObject);
        }
    }
    private void UpdateVaporize()
    {
        Vector3Int xyPos = _tilemap.LocalToCell(transform.position);
        Vector3Int xyPosDown = _tilemap.WorldToCell(new Vector3(transform.position.x, transform.position.y - 1, 0));
        Vector3Int xyPosUp = _tilemap.WorldToCell(new Vector3(transform.position.x, transform.position.y + 1, 0));
        Vector3Int xyPosDownLeft = _tilemap.WorldToCell(new Vector3(transform.position.x - 1, transform.position.y - 1, 0));
        Vector3Int xyPosDownRight = _tilemap.WorldToCell(new Vector3(transform.position.x + 1, transform.position.y - 1, 0));
        Vector3Int xyPosLeft = _tilemap.WorldToCell(new Vector3(transform.position.x - 1, transform.position.y, 0));
        Vector3Int xyPosRight = _tilemap.WorldToCell(new Vector3(transform.position.x + 1, transform.position.y, 0));
        Vector3Int xyPosUpLeft = _tilemap.WorldToCell(new Vector3(transform.position.x - 1, transform.position.y + 1, 0));
        Vector3Int xyPosUpRight = _tilemap.WorldToCell(new Vector3(transform.position.x + 1, transform.position.y + 1, 0));
        try
        {
            //fire
            if (_tilemap.GetTile(xyPos) == fireTile)
            {
                isDead = true;
                isLava = false;
            }
            if (_tilemap.GetTile(xyPosLeft) == fireTile)
            {
                isDead = true;
                isLava = false;
            }
            if (_tilemap.GetTile(xyPosRight) == fireTile)
            {
                isDead = true;
                isLava = false;
            }
            if (_tilemap.GetTile(xyPosDown) == fireTile)
            {
                isDead = true;
                isLava = false;
            }
            if (_tilemap.GetTile(xyPosUp) == fireTile)
            {
                isDead = true;
                isLava = false;
            }
            else if (_tilemap.GetTile(xyPosUpRight) == fireTile)
            {
                isDead = true;
                isLava = false;
            }
            else if (_tilemap.GetTile(xyPosUpLeft) == fireTile)
            {
                isDead = true;
                isLava = false;
            }
            else if (_tilemap.GetTile(xyPosDownRight) == fireTile)
            {
                isDead = true;
                isLava = false;
            }
            else if (_tilemap.GetTile(xyPosDownLeft) == fireTile)
            {
                isDead = true;
                isLava = false;
            }
            //lava
            if (_tilemap.GetTile(xyPos) == lavaTile)
            {
                isDead = true;
                isLava = true;
            }
            if (_tilemap.GetTile(xyPosLeft) == lavaTile)
            {
                isDead = true;
                isLava = true;
            }
            if (_tilemap.GetTile(xyPosRight) == lavaTile)
            {
                isDead = true;
                isLava = true;
            }
            if (_tilemap.GetTile(xyPosDown) == lavaTile)
            {
                isDead = true;
                isLava = true;
            }
            if (_tilemap.GetTile(xyPosUp) == lavaTile)
            {
                isDead = true;
                isLava = true;
            }
            else if (_tilemap.GetTile(xyPosUpRight) == lavaTile)
            {
                isDead = true;
                isLava = true;
            }
            else if (_tilemap.GetTile(xyPosUpLeft) == lavaTile)
            {
                isDead = true;
                isLava = true;
            }
            else if (_tilemap.GetTile(xyPosDownRight) == lavaTile)
            {
                isDead = true;
                isLava = true;
            }
            else if (_tilemap.GetTile(xyPosDownLeft) == lavaTile)
            {
                isDead = true;
                isLava = true;
            }
        }
        catch
        {

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
