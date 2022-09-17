using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tile : MonoBehaviour
{
    public Node node;

    GameManager tInstance;
    Canvas canvas;

    private void Awake()
    {
        tInstance = GameManager.instance;
    }

    public void Clicked(BaseEventData data)
    {
        tInstance.ChooseTile(gameObject);
    }

}
