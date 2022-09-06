using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    int width = 9;
    int height = 14;
    Node[,] board;
    [SerializeField] Sprite[] sprites;
    [SerializeField] GameObject tilePrefab;
    [SerializeField] GameObject tileParent;

    // Start is called before the first frame update
    void Start()
    {
        StartGame();
    }

    void StartGame()
    {
        //set board and indexes
        InitBoard();

        //set holes
        SetHoles();

        //randomize sprites
        AssignNodeValues();


        //array of meta data of pieces

        //Verify board

        //instantiate sprites
        InstTiles();
    }

    private void InitBoard()
    {
        board = new Node[width, height];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                board[x, y] = new Node(0, new Point(x, y));
            }
        }
    }

    private void SetHoles()
    {
        for (int x = 3; x < 6; x++)
        {
            for (int y = 5; y < 8; y++)
            {
                board[x, y].value = -1;
            }
        }
    }

    private void AssignNodeValues()
    {
        foreach (Node n in board)
        {
            if (n.value != -1)
            {
                n.value = Random.Range(1, 6);
            }
        }
    }

    private void InstTiles()
    {
        GameObject currentTile;
        foreach (Node n in board)
        {
            currentTile = Instantiate(tilePrefab, tileParent.transform);
            currentTile.GetComponent<RectTransform>().anchoredPosition = new Vector2(n.index.x * 64, n.index.y * -64);
            if (n.value == -1)
            {
                Color tmpC = Camera.main.backgroundColor;
                tmpC.a = 1;
                currentTile.GetComponent<Image>().color = tmpC;
            }
            else
            {
                currentTile.GetComponent<Image>().sprite = sprites[n.value - 1];
            }
        }
    }
}

[System.Serializable]
public class Node
{
    public int value; //0 = blank, 1 = cube, 2 = sphere, 3 = cylinder, 4 = pyramid, 5 - diamond, -1 = hole
    public Point index;

    public Node(int v, Point i)
    {
        value = v;
        index = i;
    }



}

[System.Serializable]
public class Point
{
    public int x;
    public int y;

    public Point(int nx, int ny)
    {
        x = nx;
        y = ny;
    }

    public void mult(int m)
    {
        x *= m;
        y *= m;
    }

    public void add(Point p)
    {
        x += p.x;
        y += p.y;
    }

    public Vector2 ToVector()
    {
        return new Vector2(x, y);
    }

    public bool Equals(Point p)
    {
        return (x == p.x && y == p.y);
    }

    public static Point fromVector(Vector2 v)
    {
        return new Point((int)v.x, (int)v.y);
    }

    public static Point fromVector(Vector3 v)
    {
        return new Point((int)v.x, (int)v.y);
    }

    public static Point mult(Point p, int m)
    {
        return new Point(p.x * m, p.y * m);
    }

    public static Point add(Point p, Point o)
    {
        return new Point(p.x + o.x, p.y + o.y);
    }

    public static Point clone(Point p)
    {
        return new Point(p.x, p.y);
    }

    public static Point zero
    {
        get { return new Point(0, 0); }
    }

    public static Point one
    {
        get { return new Point(1, 1); }
    }

    public static Point up
    {
        get { return new Point(0, 1); }
    }

    public static Point down
    {
        get { return new Point(0, -1); }
    }

    public static Point left
    {
        get { return new Point(-1, 0); }
    }

    public static Point right
    {
        get { return new Point(1, 0); }
    }
}
