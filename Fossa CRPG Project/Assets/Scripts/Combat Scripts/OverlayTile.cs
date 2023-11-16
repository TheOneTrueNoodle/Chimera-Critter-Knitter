using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OverlayTile : MonoBehaviour
{
    public int G;
    public int H;

    public int F { get { return G + H; } }

    public bool isBlocked;
    public Entity activeCharacter;

    public OverlayTile previous;

    public Vector3Int gridLocation;
    public Vector2Int grid2DLocation { get { return new Vector2Int(gridLocation.x, gridLocation.y); } }

    public Image gridSprite;
    public List<Sprite> arrows;

    public void ShowTile()
    {
        gridSprite.color = new Color(1, 1, 1, 1);
    }

    public void HideTile()
    {
        gridSprite.color = new Color(1, 1, 1, 0);
        SetArrowSprite(ArrowTranslator.ArrowDirection.None);
    }

    public void SetArrowSprite(ArrowTranslator.ArrowDirection d)
    {
        var arrow = GetComponentsInChildren<SpriteRenderer>()[0];

        if (d == ArrowTranslator.ArrowDirection.None)
        {
            arrow.color = new Color(1, 1, 1, 0);
        }
        else
        {
            arrow.color = new Color(1, 1, 1, 1);
            arrow.sprite = arrows[(int)d];
            arrow.sortingOrder = gameObject.GetComponentInChildren<Canvas>().sortingOrder;
        }
    }

    public void Clicked()
    {
        Debug.Log("Tile Clicked");
        CombatEvents.current.TileClicked(this);
    }
}
