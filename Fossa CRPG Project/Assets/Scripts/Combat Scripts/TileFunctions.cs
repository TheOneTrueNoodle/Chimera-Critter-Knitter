using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Combat
{
    public class TileFunctions
    {
        public OverlayTile GetSingleFocusedOnTile(Vector3 pos3d)
        {
            int layer_mask = LayerMask.GetMask("OverlayTiles");
            Ray ray = new Ray(pos3d, Vector3.down);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layer_mask))
            {
                Debug.DrawRay(pos3d, Vector3.down * 10, Color.yellow);
                OverlayTile hitTile = hit.collider.gameObject.GetComponent<OverlayTile>();

                if (hitTile.isBlocked != true)
                {
                    return hitTile;
                }
                else
                {
                    OverlayTile nextFreeTile = null;
                    for (int i = 1; nextFreeTile == null; i++)
                    {
                        nextFreeTile = CheckNextFreeTile(new Vector2Int(hitTile.gridLocation.x + i, hitTile.gridLocation.y));
                        if (nextFreeTile != null) { break; }
                        nextFreeTile = CheckNextFreeTile(new Vector2Int(hitTile.gridLocation.x - i, hitTile.gridLocation.y));
                        if (nextFreeTile != null) { break; }
                        nextFreeTile = CheckNextFreeTile(new Vector2Int(hitTile.gridLocation.x, hitTile.gridLocation.y + i));
                        if (nextFreeTile != null) { break; }
                        nextFreeTile = CheckNextFreeTile(new Vector2Int(hitTile.gridLocation.x, hitTile.gridLocation.y - i));
                        if (nextFreeTile != null) { break; }
                        nextFreeTile = CheckNextFreeTile(new Vector2Int(hitTile.gridLocation.x + i, hitTile.gridLocation.y + i));
                        if (nextFreeTile != null) { break; }
                        nextFreeTile = CheckNextFreeTile(new Vector2Int(hitTile.gridLocation.x - i, hitTile.gridLocation.y + i));
                        if (nextFreeTile != null) { break; }
                        nextFreeTile = CheckNextFreeTile(new Vector2Int(hitTile.gridLocation.x + i, hitTile.gridLocation.y + i));
                        if (nextFreeTile != null) { break; }
                        nextFreeTile = CheckNextFreeTile(new Vector2Int(hitTile.gridLocation.x - i, hitTile.gridLocation.y - i));
                        if (nextFreeTile != null) { break; }
                    }
                    Debug.Log(nextFreeTile);
                    Vector2 freeTilePos2d = nextFreeTile.transform.position;
                    RaycastHit2D[] newHits = Physics2D.RaycastAll(freeTilePos2d, Vector2.zero);
                    if (newHits.Length > 0)
                    {
                        return nextFreeTile;
                    }
                }
            }

            Debug.DrawRay(pos3d, Vector3.down * 10, Color.yellow);
            Debug.Log("They arent finding the tiles anymore...");
            return null;
        }

        /*
        public RaycastHit2D? GetMultipleFocusedOnTile(Vector2 pos2d, List<Vector2> subTileLocalPos2d)
        {
            RaycastHit2D[] hits = Physics2D.RaycastAll(pos2d, Vector2.zero);

            if (hits.Length > 0)
            {
                OverlayTile hitTile = hits.OrderByDescending(i => i.collider.transform.position.z).First().collider.gameObject.GetComponent<OverlayTile>();


                if (hitTile.isBlocked != true)
                {
                    bool allSubTilesFree = true;
                    foreach (Vector2 subPos in subTileLocalPos2d)
                    {
                        RaycastHit2D[] subHits = Physics2D.RaycastAll(hitTile.grid2DLocation + subPos, Vector2.zero);
                        if (subHits.Length > 0)
                        {
                            OverlayTile subTile = subHits.OrderByDescending(i => i.collider.transform.position.z).First().collider.gameObject.GetComponent<OverlayTile>();
                            if (subTile.isBlocked == true || subTile.gameObject.transform.position.z != hitTile.gameObject.transform.position.z) { allSubTilesFree = false; }
                        }
                    }
                    if (allSubTilesFree)
                    {
                        return hits.OrderByDescending(i => i.collider.transform.position.z).First();
                    }
                }

                OverlayTile nextFreeTile = null;
                for (int i = 1; nextFreeTile == null; i++)
                {
                    nextFreeTile = CheckNextFreeTileMultiple(new Vector2Int(hitTile.gridLocation.x + i, hitTile.gridLocation.y), subTileLocalPos2d);
                    if (nextFreeTile != null) { break; }
                    nextFreeTile = CheckNextFreeTileMultiple(new Vector2Int(hitTile.gridLocation.x - i, hitTile.gridLocation.y), subTileLocalPos2d);
                    if (nextFreeTile != null) { break; }
                    nextFreeTile = CheckNextFreeTileMultiple(new Vector2Int(hitTile.gridLocation.x, hitTile.gridLocation.y + i), subTileLocalPos2d);
                    if (nextFreeTile != null) { break; }
                    nextFreeTile = CheckNextFreeTileMultiple(new Vector2Int(hitTile.gridLocation.x, hitTile.gridLocation.y - i), subTileLocalPos2d);
                    if (nextFreeTile != null) { break; }
                    nextFreeTile = CheckNextFreeTileMultiple(new Vector2Int(hitTile.gridLocation.x + i, hitTile.gridLocation.y + i), subTileLocalPos2d);
                    if (nextFreeTile != null) { break; }
                    nextFreeTile = CheckNextFreeTileMultiple(new Vector2Int(hitTile.gridLocation.x - i, hitTile.gridLocation.y + i), subTileLocalPos2d);
                    if (nextFreeTile != null) { break; }
                    nextFreeTile = CheckNextFreeTileMultiple(new Vector2Int(hitTile.gridLocation.x + i, hitTile.gridLocation.y + i), subTileLocalPos2d);
                    if (nextFreeTile != null) { break; }
                    nextFreeTile = CheckNextFreeTileMultiple(new Vector2Int(hitTile.gridLocation.x - i, hitTile.gridLocation.y - i), subTileLocalPos2d);
                    if (nextFreeTile != null) { break; }
                }
                Debug.Log(nextFreeTile);
                Vector2 freeTilePos2d = nextFreeTile.transform.position;
                RaycastHit2D[] newHits = Physics2D.RaycastAll(freeTilePos2d, Vector2.zero);
                if (newHits.Length > 0)
                {
                    return newHits.OrderByDescending(i => i.collider.transform.position.z).First();
                }
            }

            Debug.Log("They arent finding the tiles anymore...");
            return null;
        }
        */

        private OverlayTile CheckNextFreeTile(Vector2Int freeTileCheck)
        {
            if (MapManager.Instance.map[freeTileCheck] != null && MapManager.Instance.map[freeTileCheck].isBlocked != true)
            {
                return MapManager.Instance.map[freeTileCheck];
            }
            return null;
        }
        private OverlayTile CheckNextFreeTileMultiple(Vector2Int freeTileCheck, List<Vector2> subTileLocalPos2d)
        {
            if (MapManager.Instance.map[freeTileCheck] != null && MapManager.Instance.map[freeTileCheck].isBlocked != true)
            {
                bool allSubTilesFree = true;
                foreach (Vector2 subPos in subTileLocalPos2d)
                {
                    RaycastHit2D[] subHits = Physics2D.RaycastAll(freeTileCheck + subPos, Vector2.zero);
                    if (subHits.Length > 0)
                    {
                        OverlayTile subTile = subHits.OrderByDescending(i => i.collider.transform.position.z).First().collider.gameObject.GetComponent<OverlayTile>();
                        if (subTile.isBlocked == true || subTile.gameObject.transform.position.z != MapManager.Instance.map[freeTileCheck].gameObject.transform.position.z) { allSubTilesFree = false; }
                    }
                }
                if (allSubTilesFree)
                {
                    return MapManager.Instance.map[freeTileCheck];
                }
            }
            return null;
        }
    }
}
