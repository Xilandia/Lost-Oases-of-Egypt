using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildingHandler : MonoBehaviour
{
    public static BuildingHandler instance;

    public GridLayout gridLayout;
    private Grid grid;
    [SerializeField] private Tilemap MainTilemap;
    [SerializeField] private TileBase whiteTile;
    
    public AudioClip[] buildingSounds;
    public AudioSource soundEffectSource;
    
    public GameObject[] buildingPrefabs;

    private PlacableObject objectToPlace;
    private PlayerTrainer pT; // combine as part of refactor

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
            if (PlayerManager.instance.playerOre >= pT.trainerCost)
            {
                if (CanBePlaced(objectToPlace))
                {
                    objectToPlace.Place();
                    Vector3Int start = gridLayout.WorldToCell(objectToPlace.GetStartPosition());
                    TakeArea(start, objectToPlace.Size);
                    PlayerManager.instance.playerOre -= pT.trainerCost;
                    soundEffectSource.PlayOneShot(buildingSounds[2]);
                    
                    return true;
                }
                
                soundEffectSource.PlayOneShot(buildingSounds[1]);
                Debug.Log("Can't place structure here");
            }
            else
            {
                soundEffectSource.PlayOneShot(buildingSounds[0]);
                Debug.Log("Can't afford structure");
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
        return instance.gridLayout.CellToWorld(cellPosition) + new Vector3(0,100,0);
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
        newObject.transform.SetParent(PlayerManager.instance.playerTrainers);
        objectToPlace = newObject.GetComponent<PlacableObject>();
        pT = newObject.GetComponent<PlayerTrainer>();
        EntityHandler.instance.SetPlayerTrainerStats(pT, newObject.gameObject.name);
        InputHandler.instance.FirstSelectStructure(newObject.transform);
        pT.isPrototype = true;
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
