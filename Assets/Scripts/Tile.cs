using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    public Node node;
    bool pressed = false;
    GameManager tInstance;
    Canvas canvas;
    Image image;

    private void Awake()
    {
        tInstance = GameManager.instance;
        image = GetComponent<Image>();
    }

    //used as an event
    public void Clicked(BaseEventData data)
    {
        tInstance.ChooseTile(gameObject);
    }

    public void ShowIsPressed()
    {
        Debug.Log("color");
        pressed = !pressed;
        if(pressed)
        {
            Debug.Log("true color");
            image.color = new Color(106f/255f, 106f / 255f, 106f / 255f, 1);
        }
        else
        {
            Debug.Log("false color");
            image.color = Color.white;
        }
    }

    public void BackToNotBlack()
    {
        image.color = Color.white;
        pressed = false;
    }
}
