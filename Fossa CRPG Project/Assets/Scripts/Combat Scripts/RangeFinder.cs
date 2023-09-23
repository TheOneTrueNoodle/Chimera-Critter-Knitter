using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RangeFinder
{
    public List<OverlayTile> GetTilesInRange(List<OverlayTile> startTiles, int range, bool hideOccupiedTiles, bool includeOrigin)
    {
        var inRangeTiles = new List<OverlayTile>();
        int stepCount = 0;

        inRangeTiles.AddRange(startTiles);

        var tileForPreviousStep = new List<OverlayTile>();
        tileForPreviousStep.AddRange(startTiles);

        while (stepCount < range)
        {
            var surroundingTiles = new List<OverlayTile>();

            foreach (var item in tileForPreviousStep)
            {
                var tiles = MapManager.Instance.GetNeighbourTiles(item, new List<OverlayTile>(), hideOccupiedTiles);
                foreach (OverlayTile tile in tiles)
                {
                    if (!surroundingTiles.Contains(tile))
                    { surroundingTiles.Add(tile); }
                }
            }

            inRangeTiles.AddRange(surroundingTiles);
            tileForPreviousStep = surroundingTiles.Distinct().ToList();
            stepCount++;
        }

        if (!includeOrigin)
        {
            foreach (var tile in startTiles)
            {
                inRangeTiles.Remove(tile);
            }
        }
        return inRangeTiles.Distinct().ToList();
    }
    public List<OverlayTile> GetMovementTilesInRange(Entity entity, int range, bool hideOccupiedTiles, bool includeOrigin)
    {
        OverlayTile startTile = entity.activeTile;
        var inRangeTiles = new List<OverlayTile>();
        int stepCount = 0;

        inRangeTiles.Add(startTile);

        var tileForPreviousStep = new List<OverlayTile>();
        tileForPreviousStep.Add(startTile);

        while (stepCount < range)
        {
            var surroundingTiles = new List<OverlayTile>();

            foreach (var item in tileForPreviousStep)
            {
                var tiles = MapManager.Instance.GetNeighbourTiles(item, new List<OverlayTile>(), false);
                foreach (var tile in tiles)
                {
                    if (hideOccupiedTiles && tile.isBlocked && tile.activeCharacter != entity)
                    {
                        //Dont add
                    }
                    else
                    {
                        //add
                        surroundingTiles.Add(tile);
                    }
                }
            }

            /*
            if (entity.subTileSpaces.Count > 0)
            {
                foreach (var tile in surroundingTiles)
                {
                    bool subTilesFree = true;
                    foreach (var subTileCheck in entity.subTileSpaces)
                    {
                        var tileDifference = startTile.grid2DLocation - subTileCheck.subTile.grid2DLocation;
                        if (MapManager.Instance.map.ContainsKey(tile.grid2DLocation + tileDifference))
                        {
                            OverlayTile subTile = MapManager.Instance.map[tile.grid2DLocation + tileDifference];
                            if (subTile.isBlocked)
                            {
                                subTilesFree = false;
                            }
                        }
                    }
                    if (subTilesFree)
                    {
                        inRangeTiles.Add(tile);
                        tileForPreviousStep = surroundingTiles.Distinct().ToList();
                        stepCount++;
                    }
                }
            }
            else
            {*/
                inRangeTiles.AddRange(surroundingTiles);
                tileForPreviousStep = surroundingTiles.Distinct().ToList();
                stepCount++;
            //}
        }

        if (!includeOrigin)
            inRangeTiles.Remove(startTile);
        return inRangeTiles.Distinct().ToList();
    }
}
