using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePieces : MonoBehaviour
{
    MovePieces instance;
    Tile moving;
    Point newIndex;
    Vector2 mouseStart;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (moving == null)
            return;

        Vector2 dir = (Vector2)Input.mousePosition - mouseStart;
        Vector2 nDir = dir.normalized;
        Vector2 aDir = new Vector2(Mathf.Abs(dir.x), Mathf.Abs(dir.y));

        newIndex = Point.clone(moving.node.index);
        
    }

    public void MovePiece(Tile tile)
    {
        if (moving != null)
            return;
        moving = tile;
    }
}
