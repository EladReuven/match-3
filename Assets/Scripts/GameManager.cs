using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    int width = 9;
    int height = 14;
    Node[,] board;
    [SerializeField] Sprite[] sprites;
    //[SerializeField] Sprite hole;
    [SerializeField] GameObject tilePrefab;
    [SerializeField] GameObject tileParent;

    List<GameObject> tilesToSwap = new List<GameObject>();
    List<Node> NodesToDelete = new List<Node>();

    #region UnityFunction
    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        StartGame();
    }

    #endregion

    #region CustomMethods
    void StartGame()
    {
        //set board and indexes
        InitBoard();

        //set holes
        SetHolesLVL1();

        //randomize sprites
        AssignNodeValues();

        //array of meta data of pieces

        //Verify board
        do
        {
            foreach (Node node in NodesToDelete)
            {
                node.value = Random.Range(0, sprites.Length);
            }
            VerifyBoard();
        }
        while (NodesToDelete.Count > 0);
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

    private void SetHolesLVL1()
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
                n.value = Random.Range(1, sprites.Length);
            }
        }
    }

    private void VerifyBoard()
    {
        NodesToDelete.Clear();
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Node node = board[x, y];

                if (NodesToDelete.Contains(node))
                    continue;

                if (node.value == -1)
                    continue;

                VerifyX(y, x, node);
                VerifyY(y, x, node);
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
                //currentTile.GetComponent<Image>().sprite = hole;

            }
            else
            {
                currentTile.GetComponent<Image>().sprite = sprites[n.value];
            }
            currentTile.GetComponent<Tile>().node = n;
        }
    }

    private void VerifyY(int y, int x, Node node)
    {
        if (y + 2 < height && node.value == board[x, y + 1].value && node.value == board[x, y + 2].value)
        {
            NodesToDelete.Add(node);
            NodesToDelete.Add(board[x, y + 1]);
            NodesToDelete.Add(board[x, y + 2]);
            //if it's a column of 4
            if (y + 3 < height && node.value == board[x, y + 3].value)
            {
                NodesToDelete.Add(board[x, y + 3]);
                //if it's a row of 5
                if (y + 4 < height && node.value == board[x, y + 4].value)
                {
                    NodesToDelete.Add(board[x, y + 4]);
                }
            }
        }
    }

    private void VerifyX(int y, int x, Node node)
    {
        if (x + 2 < width && node.value == board[x + 1, y].value && node.value == board[x + 2, y].value)
        {
            NodesToDelete.Add(node);
            NodesToDelete.Add(board[x + 1, y]);
            NodesToDelete.Add(board[x + 2, y]);
            //if it's a row of 4
            if (x + 3 < width && node.value == board[x + 3, y].value)
            {
                NodesToDelete.Add(board[x + 3, y]);
                //if it's a row of 5
                if (x + 4 < width && node.value == board[x + 4, y].value)
                {
                    NodesToDelete.Add(board[x + 4, y]);
                }
            }
        }
    }

    private bool VerifyChoice(GameObject tile)
    {

        //current node is next to previous node
        //current node == previous node + up or right or down or left
        Node currentNode = tile.GetComponent<Tile>().node;

        return isNearbyTile(currentNode);
    }

    private bool isNearbyTile(Node currentNode)
    {
        Node prevNode = tilesToSwap[0].GetComponent<Tile>().node;
        return (currentNode.index.x == Point.add(prevNode.index, Point.up).x && currentNode.index.y == Point.add(prevNode.index, Point.up).y) ||
            (currentNode.index.x == Point.add(prevNode.index, Point.right).x && currentNode.index.y == Point.add(prevNode.index, Point.right).y) ||
            (currentNode.index.x == Point.add(prevNode.index, Point.down).x && currentNode.index.y == Point.add(prevNode.index, Point.down).y) ||
            (currentNode.index.x == Point.add(prevNode.index, Point.left).x && currentNode.index.y == Point.add(prevNode.index, Point.left).y);
    }

    public void ChooseTile(GameObject tile)
    {
        Tile tile1 = tile.GetComponent<Tile>();
        Node currentNode = tile1.node;
        if (currentNode.value == -1)
            return;

        Debug.Log("Count before: " + tilesToSwap.Count);
        if (tilesToSwap.Contains(tile))
        {
            tilesToSwap.Remove(tile);
            Debug.Log("Count after remove: " + tilesToSwap.Count);
        }
        else if (tilesToSwap.Count > 0)
        {
            if(VerifyChoice(tile))
            {
                tilesToSwap.Add(tile);
                SwapTiles();
                return;
            }
            else
            {
                Debug.Log("Tile Not next to previous choice");
                return;
            }
        }
        else
        {
            Debug.Log($"added tile: x{tile.GetComponent<Tile>().node.index.x} y{tile.GetComponent<Tile>().node.index.y}");
            tilesToSwap.Add(tile);
        }
        tile1.ShowIsPressed();
    }

    public void SwapTiles()
    {
        Tile tile1 = tilesToSwap[0].GetComponent<Tile>();
        Tile tile2 = tilesToSwap[1].GetComponent<Tile>();
        Debug.Log("Swapping Tiles");
        Node node1 = tile1.node;
        Node node2 = tile2.node;

        Point tmpPoint;
        tmpPoint = node1.index;
        node1.index = node2.index;
        node2.index = tmpPoint;
        tilesToSwap[0].GetComponent<RectTransform>().anchoredPosition = new Vector2(node1.index.x * 64, node1.index.y * -64);
        tilesToSwap[1].GetComponent<RectTransform>().anchoredPosition = new Vector2(node2.index.x * 64, node2.index.y * -64);
        tile1.BackToNotBlack();
        tile2.BackToNotBlack();
        tilesToSwap.Clear();
    }

    [ContextMenu("Print TilesToSwap")]
    public void PrintTilesToSwap()
    {
        Debug.Log($"tile 0: x{tilesToSwap[0].GetComponent<Tile>().node.index.x} y{tilesToSwap[0].GetComponent<Tile>().node.index.y}");
        Debug.Log($"tile 1: x{tilesToSwap[1].GetComponent<Tile>().node.index.x} y{tilesToSwap[1].GetComponent<Tile>().node.index.y}");
    }

    #endregion

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

    public static Point subtract(Point p, Point o)
    {
        return new Point(p.x - o.x, p.y - o.y);
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
