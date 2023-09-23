using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileHandler
{
    public List<OverlayTile> AllTiles = new List<OverlayTile>();
    public List<OverlayTile> ActiveTiles = new List<OverlayTile>();

    public void ColorTiles(Entity entity, Color color, List<OverlayTile> tiles, bool hideActiveTile)
    {
        foreach (var item in tiles)
        {
            item.gridSprite.color = color;
            if (ActiveTiles.Contains(item) != true) { ActiveTiles.Add(item); }
        }
        if (hideActiveTile)
        {
            entity.activeTile.HideTile();
            ActiveTiles.Remove(entity.activeTile);
        }
    }

    public void ClearTiles()
    {
        foreach (OverlayTile tile in ActiveTiles)
        {
            tile.HideTile();
        }

        ActiveTiles.Clear();
    }

    public void ShowTiles(List<OverlayTile> tiles)
    {
        foreach (OverlayTile tile in tiles)
        {
            tile.ShowTile();
            ActiveTiles.Add(tile);
        }
    }

    public void ClearSpecificTiles(List<OverlayTile> tiles)
    {
        foreach (OverlayTile tile in tiles)
        {
            tile.HideTile();
            ActiveTiles.Remove(tile);
        }
    }

    public void PositionCharacterOnTile(Entity entity, OverlayTile tile)
    {
        if (entity.activeTile != null && entity.activeTile != tile)
        {
            entity.activeTile.isBlocked = false;
            entity.activeTile.activeCharacter = null;
        }
        /*
        if (entity.subTileSpaces != null)
        {
            foreach (EntitySubTile SubTile in entity.subTileSpaces)
            {
                if (SubTile.subTile != null)
                {
                    SubTile.subTile.isBlocked = false;
                    SubTile.subTile.blockingChar = null;
                    SubTile.assignSubTile();
                }
            }
        }*/

        entity.transform.position = new Vector3(tile.gameObject.transform.position.x, tile.gameObject.transform.position.y - 0.0001f, tile.gameObject.transform.position.z);
        entity.gameObject.GetComponentInChildren<SpriteRenderer>().sortingOrder = tile.GetComponentInChildren<Canvas>().sortingOrder;
        entity.activeTile = tile;
        entity.activeTile.isBlocked = true;
        entity.activeTile.activeCharacter = entity;
    }
}
