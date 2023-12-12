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
        if (entity.activeTile != null)
        {
            entity.activeTile.isBlocked = false;
            entity.activeTile.activeCharacter = null;
        }
        if (tile == null) { Debug.LogError("Tile is null for " + entity.CharacterData.Name); }

        entity.transform.position = new Vector3(tile.gameObject.transform.position.x, tile.gameObject.transform.position.y - 0.0001f, tile.gameObject.transform.position.z);
        entity.activeTile = tile;
        entity.activeTile.isBlocked = true;
        entity.activeTile.activeCharacter = entity;
    }

    public void ClearUnitTiles(List<Entity> playerEntities, List<CombatAIController> enemyEntities, List<CombatAIController> otherEntities, List<CombatObstacle> obstacles)
    {
        foreach (Entity player in playerEntities)
        {
            if (player.activeTile != null)
            {
                player.activeTile.isBlocked = false;
                player.activeTile.activeCharacter = null;
                player.activeTile = null;
            }
        }
        foreach (Entity enemy in enemyEntities)
        {
            if (enemy.activeTile != null)
            {
                enemy.activeTile.isBlocked = false;
                enemy.activeTile.activeCharacter = null;
                enemy.activeTile = null;
            }
        }
        foreach (Entity other in otherEntities)
        {
            if (other.activeTile != null)
            {
                other.activeTile.isBlocked = false;
                other.activeTile.activeCharacter = null;
                other.activeTile = null;
            }
        }
        foreach (CombatObstacle obstacle in obstacles)
        {
            if (obstacle.activeTile != null)
            {
                obstacle.activeTile.isBlocked = false;
                obstacle.activeTile.activeCharacter = null;
                obstacle.activeTile = null;
            }
        }
    }
}
