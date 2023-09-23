using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveHandler
{
    private static int unitSpeed = 6;

    public IEnumerator MoveAlongPath(Entity entity, List<OverlayTile> path)
    {
        OverlayTile finalTile = new OverlayTile();
        while (path.Count != 0)
        {
            var step = unitSpeed * Time.deltaTime;
            var zIndex = path[0].transform.position.z;
            entity.transform.position = Vector2.MoveTowards(entity.transform.position, path[0].transform.position, step);
            entity.transform.position = new Vector3(entity.transform.position.x, entity.transform.position.y, zIndex);

            if (Vector2.Distance(entity.transform.position, path[0].transform.position) < 0.0001f)
            {
                CombatEvents.current.TilePositionEntity(entity, path[0]);
                /*
                if (entity.subTileSpaces.Count > 0)
                {
                    foreach (EntitySubTile subTile in entity.subTileSpaces)
                    {
                        subTile.assignSubTile();
                    }
                }
                */
                path.RemoveAt(0);
            }

            yield return new WaitForSeconds(Time.deltaTime);
        }

        if (path.Count == 0)
        {
            CombatEvents.current.ActionComplete();
        }
    }
}
