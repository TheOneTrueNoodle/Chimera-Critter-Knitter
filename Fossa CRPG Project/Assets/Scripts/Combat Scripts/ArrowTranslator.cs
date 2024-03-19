using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowTranslator
{
    public enum ArrowDirection
    {
        None = 0,
        Up = 1,
        Down = 2,
        Left = 3,
        Right = 4,
        TopRight = 5,
        BottomRight = 6,
        TopLeft = 7,
        BottomLeft = 8,
        UpFinished = 9,
        DownFinished = 10,
        LeftFinished = 11,
        RightFinished = 12
    }

    public ArrowDirection TranslateDirection(OverlayTile previousTile, OverlayTile currentTile, OverlayTile futureTile)
    {
        bool isFinal = futureTile == null;

        Vector2Int pastDirection = previousTile != null ? currentTile.grid2DLocation - previousTile.grid2DLocation : new Vector2Int(0, 0);
        Vector2Int futureDirection = futureTile != null ? futureTile.grid2DLocation - currentTile.grid2DLocation : new Vector2Int(0, 0);
        Vector2Int direction = pastDirection != futureDirection ? pastDirection + futureDirection : futureDirection;

        //Straight Lines
        if (direction == new Vector2Int(0, 1) && !isFinal)
        {
            return ArrowDirection.Up;
        }
        if (direction == new Vector2Int(0, -1) && !isFinal)
        {
            return ArrowDirection.Down;
        }
        if (direction == new Vector2Int(1, 0) && !isFinal)
        {
            return ArrowDirection.Right;
        }
        if (direction == new Vector2Int(-1, 0) && !isFinal)
        {
            return ArrowDirection.Left;
        }

        //Corners
        if (direction == new Vector2Int(1, 1))
        {
            //Up and then right
            return ArrowDirection.TopRight;
            /*
            if (pastDirection.y < futureDirection.y)
            {
                return ArrowDirection.TopRight;
            }
            else
            {
                return ArrowDirection.BottomLeft;
            }
            */
        }
        if (direction == new Vector2Int(-1, 1))
        {
            //Up and the left
            return ArrowDirection.TopLeft;
            /*
            if (pastDirection.y < futureDirection.y)
            {
                return ArrowDirection.TopLeft;
            }
            else
            {
                return ArrowDirection.BottomRight;
            }
            */
        }
        if (direction == new Vector2Int(1, -1))
        {
            //Down and then right
            return ArrowDirection.BottomRight;
            /*
            if (pastDirection.y < futureDirection.y)
            {
                return ArrowDirection.TopLeft;
            }
            else
            {
                return ArrowDirection.BottomRight;
            }
            */
        }
        if (direction == new Vector2Int(-1, -1))
        {
            //Down and the left
            return ArrowDirection.BottomLeft;
            /*
            if (pastDirection.y < futureDirection.y)
            {
                return ArrowDirection.TopRight;
            }
            else
            {
                return ArrowDirection.BottomLeft;
            }
            */
        }

        //Final Arrows
        if (direction == new Vector2Int(0, 1) && isFinal)
        {
            return ArrowDirection.UpFinished;
        }
        if (direction == new Vector2Int(0, -1) && isFinal)
        {
            return ArrowDirection.DownFinished;
        }
        if (direction == new Vector2Int(1, 0) && isFinal)
        {
            return ArrowDirection.RightFinished;
        }
        if (direction == new Vector2Int(-1, 0) && isFinal)
        {
            return ArrowDirection.LeftFinished;
        }

        return ArrowDirection.None;
    }
}
