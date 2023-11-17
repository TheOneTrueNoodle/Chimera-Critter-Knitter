using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
    private static MapManager _instance;
    public static MapManager Instance { get { return _instance; } }

    public OverlayTile overlayTilePrefab;
    public GameObject overlayContainer;

    public Dictionary<Vector2Int, OverlayTile> map;
    [HideInInspector] public MapBounds mapBounds;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
        SetMap();
    }

    public void SetMap()
    {
        Tilemap[] childTilemaps = gameObject.GetComponentsInChildren<Tilemap>();
        map = new Dictionary<Vector2Int, OverlayTile>();

        mapBounds = new MapBounds();
        foreach (var tilemap in childTilemaps)
        {
            foreach (Transform child in tilemap.transform)
            {
                var gridLocation = tilemap.WorldToCell(child.position);
                var meshBounds = child.gameObject.GetComponent<MeshRenderer>().bounds;
                var overlayTile = Instantiate(overlayTilePrefab, overlayContainer.transform);

                /*
                foreach (var tileData in tileTypeList.items)
                {
                    foreach (var material in tileData.Tiles3D)
                    {
                        if (material == child.GetComponent<MeshRenderer>().sharedMaterial)
                        {
                            overlayTile.tileData = tileData;
                        }
                    }
                }
                */

                overlayTile.transform.position = new Vector3(child.position.x, child.position.y + meshBounds.extents.y + 0.0001f, child.position.z);
                overlayTile.gridLocation = gridLocation;
                var tileKey = new Vector2Int(gridLocation.x, gridLocation.y);
                SetMapBounds(gridLocation);

                if (!map.ContainsKey(tileKey))
                {
                    map.Add(tileKey, overlayTile);
                }
                else
                {
                    var old = map[tileKey];
                    map.Remove(tileKey);
                    map.Add(tileKey, overlayTile);
                    Destroy(old.gameObject);
                    Destroy(child.gameObject);
                }
            }
        }
    }

    private void SetMapBounds(Vector3Int gridLocation)
    {
        if (mapBounds.yMin > gridLocation.y)
            mapBounds.yMin = gridLocation.y;

        if (mapBounds.yMax < gridLocation.y)
            mapBounds.yMax = gridLocation.y;

        if (mapBounds.xMin > gridLocation.x)
            mapBounds.xMin = gridLocation.x;

        if (mapBounds.xMax > gridLocation.x)
            mapBounds.xMax = gridLocation.x;
    }

    public List<OverlayTile> GetNeighbourTiles(OverlayTile currentOverlayTile, List<OverlayTile> searchableTiles, bool hideOccupiedTiles)
    {
        Dictionary<Vector2Int, OverlayTile> tilesToSearch = new Dictionary<Vector2Int, OverlayTile>();

        if (searchableTiles.Count > 0)
        {
            foreach (var item in searchableTiles)
            {
                tilesToSearch.Add(item.grid2DLocation, item);
            }
        }
        else
        {
            tilesToSearch = map;
        }

        List<OverlayTile> neighbours = new List<OverlayTile>();

        //Check Top Tile
        Vector2Int LocationToCheck = new Vector2Int(currentOverlayTile.gridLocation.x, currentOverlayTile.gridLocation.y + 1);
        if (tilesToSearch.ContainsKey(LocationToCheck))
        {
            if (Mathf.Abs(currentOverlayTile.gridLocation.z - tilesToSearch[LocationToCheck].gridLocation.z) <= 1)
            {
                if (tilesToSearch[LocationToCheck].isBlocked && !hideOccupiedTiles) { neighbours.Add(tilesToSearch[LocationToCheck]); }
                else if (!tilesToSearch[LocationToCheck].isBlocked) { neighbours.Add(tilesToSearch[LocationToCheck]); }
            }
        }

        //Check Bottom Tile
        LocationToCheck = new Vector2Int(currentOverlayTile.gridLocation.x, currentOverlayTile.gridLocation.y - 1);
        if (tilesToSearch.ContainsKey(LocationToCheck))
        {
            if (Mathf.Abs(currentOverlayTile.gridLocation.z - tilesToSearch[LocationToCheck].gridLocation.z) <= 1)
            {
                if (tilesToSearch[LocationToCheck].isBlocked && !hideOccupiedTiles) { neighbours.Add(tilesToSearch[LocationToCheck]); }
                else if (!tilesToSearch[LocationToCheck].isBlocked) { neighbours.Add(tilesToSearch[LocationToCheck]); }
            }
        }

        //Check Left Tile
        LocationToCheck = new Vector2Int(currentOverlayTile.gridLocation.x - 1, currentOverlayTile.gridLocation.y);
        if (tilesToSearch.ContainsKey(LocationToCheck))
        {
            if (Mathf.Abs(currentOverlayTile.gridLocation.z - tilesToSearch[LocationToCheck].gridLocation.z) <= 1)
            {
                if (tilesToSearch[LocationToCheck].isBlocked && !hideOccupiedTiles) { neighbours.Add(tilesToSearch[LocationToCheck]); }
                else if (!tilesToSearch[LocationToCheck].isBlocked) { neighbours.Add(tilesToSearch[LocationToCheck]); }
            }
        }

        //Check Right Tile
        LocationToCheck = new Vector2Int(currentOverlayTile.gridLocation.x + 1, currentOverlayTile.gridLocation.y);
        if (tilesToSearch.ContainsKey(LocationToCheck))
        {
            if (Mathf.Abs(currentOverlayTile.gridLocation.z - tilesToSearch[LocationToCheck].gridLocation.z) <= 1)
            {
                if (tilesToSearch[LocationToCheck].isBlocked && !hideOccupiedTiles) { neighbours.Add(tilesToSearch[LocationToCheck]); }
                else if (!tilesToSearch[LocationToCheck].isBlocked) { neighbours.Add(tilesToSearch[LocationToCheck]); }
            }
        }

        return neighbours;
    }
}
public class MapBounds
{
    public int xMax = 0;
    public int yMax = 0;
    public int xMin = 0;
    public int yMin = 0;

    public MapBounds(int xMax, int yMax, int xMin, int yMin)
    {
        this.xMax = xMax;
        this.yMax = yMax;
        this.xMin = xMin;
        this.yMin = yMin;
    }

    public MapBounds()
    { 
    }
}
