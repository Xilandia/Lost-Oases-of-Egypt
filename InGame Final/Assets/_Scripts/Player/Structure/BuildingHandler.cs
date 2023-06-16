using UnityEngine;
using UnityEngine.Tilemaps;
using _Scripts.Player.Management;
using _Scripts.Interaction.Management;
using _Scripts.Utility.Entity;

namespace _Scripts.Player.Structure
{
    public class BuildingHandler : MonoBehaviour
    {
        public static BuildingHandler instance;
        private static Camera cam;

        public GridLayout gridLayout;
        private Grid grid;
        [SerializeField] private Tilemap mainTilemap;
        [SerializeField] private TileBase whiteTile;

        public AudioClip[] buildingSounds;
        public AudioSource soundEffectSource;

        private PlayerBarracks pB;
        private PlayerTower pT;
        private bool placingTower;

        private static RaycastHit hit;

        void Awake()
        {
            instance = this;
            cam = Camera.main;
            grid = gridLayout.gameObject.GetComponent<Grid>();
        }

        public bool TryToPlace()
        {
            if (placingTower)
            {
                if (pT.isPrototype)
                {
                    if (PlayerManager.instance.playerOre >= pT.towerCostOre && PlayerManager.instance.playerWood >= pT.towerCostWood)
                    {
                        if (CanBePlaced(pT.towerPlacable))
                        {
                            pT.towerPlacable.Place();
                            Vector3Int start = gridLayout.WorldToCell(pT.towerPlacable.GetStartPosition());
                            TakeArea(start, pT.towerPlacable.Size);
                            PlayerManager.instance.playerOre -= pT.towerCostOre;
                            PlayerManager.instance.playerWood -= pT.towerCostWood;
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
            else
            {
                if (pB.isPrototype)
                {
                    if (PlayerManager.instance.playerOre >= pB.barracksCostOre && PlayerManager.instance.playerWood >= pB.barracksCostWood)
                    {
                        if (CanBePlaced(pB.barracksPlacable))
                        {
                            pB.barracksPlacable.Place();
                            Vector3Int start = gridLayout.WorldToCell(pB.barracksPlacable.GetStartPosition());
                            TakeArea(start, pB.barracksPlacable.Size);
                            PlayerManager.instance.playerOre -= pB.barracksCostOre;
                            PlayerManager.instance.playerWood -= pB.barracksCostWood;
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
        }

        public static Vector3 GetMouseWorldPosition()
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                return hit.point;
            }

            return Vector3.zero;
        }

        public static Vector3 SnapToGrid(Vector3 position)
        {
            Vector3Int cellPosition = instance.gridLayout.WorldToCell(position);
            return instance.gridLayout.CellToWorld(cellPosition) + new Vector3(0, position.y, 0);
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

        public void InitializeWithObject(GameObject prefab, bool isTower)
        {
            InputHandler.instance.DeselectAll();
            Vector3 position = SnapToGrid(Vector3.zero);
            GameObject newObject = Instantiate(prefab, position, Quaternion.identity);
            placingTower = isTower;
            if (isTower)
            {
                newObject.transform.SetParent(PlayerManager.instance.playerTowers);
                pT = newObject.GetComponent<PlayerTower>();
                EntityHandler.instance.SetPlayerTowerStats(pT, newObject.gameObject.name);
                InputHandler.instance.FirstSelectTower(pT);
                pT.isPrototype = true;
            }
            else
            {
                newObject.transform.SetParent(PlayerManager.instance.playerBarracks);
                pB = newObject.GetComponent<PlayerBarracks>();
                EntityHandler.instance.SetPlayerBarracksStats(pB, newObject.gameObject.name);
                InputHandler.instance.FirstSelectBarracks(pB);
                pB.isPrototype = true;
            }
        }

        private bool CanBePlaced(PlacableObject objectToPlace)
        {
            BoundsInt area = new BoundsInt();
            area.position = gridLayout.WorldToCell(objectToPlace.GetStartPosition());
            area.size = objectToPlace.Size;

            TileBase[] tiles = GetTilesBlock(area, mainTilemap);

            foreach (var tile in tiles)
            {
                if (tile == whiteTile)
                {
                    return false;
                }
            }

            return true;
        }

        private void TakeArea(Vector3Int start, Vector3Int size)
        {
            mainTilemap.BoxFill(start, whiteTile, start.x, start.y, start.x + size.x, start.y + size.y);
        }
    }
}
