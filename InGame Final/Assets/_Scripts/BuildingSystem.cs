using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildingSystem : MonoBehaviour
{
    public static BuildingSystem instance;

    public GridLayout gridLayout;
    private Grid grid;
    [SerializeField] private Tilemap MainTilemap;
    [SerializeField] private TileBase whiteTile;
    
    public GameObject[] buildingPrefabs;

    private PlacableObject objectToPlace;

    private static RaycastHit hit;
    
    void Awake()
    {
        instance = this;
        grid = gridLayout.gameObject.GetComponent<Grid>();
    }
    
    public bool TryToPlace()
    {
        if (objectToPlace != null)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (CanBePlaced(objectToPlace))
                {
                    objectToPlace.Place();
                    Vector3Int start = gridLayout.WorldToCell(objectToPlace.GetStartPosition());
                    TakeArea(start, objectToPlace.Size);

                    return true;
                }
                else
                {
                    // make jingle to indicate that object cannot be placed
                }
            }
        }

        return false;
    }
    
    public static Vector3 GetMouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        if (Physics.Raycast(ray, out hit))
        {
            return hit.point;
        }
        
        return Vector3.zero;
    }
    
    public static Vector3 SnapToGrid(Vector3 position)
    {
        Vector3Int cellPosition = instance.gridLayout.WorldToCell(position);
        return instance.gridLayout.CellToWorld(cellPosition);
    }

    private static TileBase[] GetTilesBlock(BoundsInt area, Tilemap tilemap)
    {
        TileBase[] tilebases = new TileBase[area.size.x * area.size.y * area.size.z];
        int counter = 0;

        foreach (var v in area.allPositionsWithin)
        {
            Vector3Int pos = new Vector3Int(v.x, v.y, 0);
            tilebases[counter] = tilemap.GetTile(pos);
            counter++;
        }
        
        return tilebases;
    }
    
    public void InitializeWithObject(int prefabIndex)
    {
        Vector3 position = SnapToGrid(Vector3.zero);
        GameObject newObject = Instantiate(buildingPrefabs[prefabIndex], position, Quaternion.identity);
        //newObject.transform.SetParent(grid.transform);
        objectToPlace = newObject.GetComponent<PlacableObject>();
        newObject.GetComponent<PlayerTrainer>().isPrototype = true;
        //newObject.AddComponent<ObjectDrag>();
        InputHandler.instance.FirstSelectStructure(newObject.transform);
    }
    
    private bool CanBePlaced(PlacableObject objectToPlace)
    {
        BoundsInt area = new BoundsInt();
        area.position = gridLayout.WorldToCell(objectToPlace.GetStartPosition());
        area.size = objectToPlace.Size;
        
        TileBase[] tiles = GetTilesBlock(area, MainTilemap);
        
        foreach (var tile in tiles)
        {
            if (tile == whiteTile)
            {
                return false;
            }
        }
        
        return true;
    }

    public void TakeArea(Vector3Int start, Vector3Int size)
    {
        MainTilemap.BoxFill(start, whiteTile, start.x, start.y, start.x + size.x, start.y + size.y);
    }
    
}
