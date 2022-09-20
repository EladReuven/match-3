using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Tile : MonoBehaviour , IPointerDownHandler, IPointerUpHandler
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

    //first touch point of finger
    //every frame check position of finger drag
    //candy can only move on x and y axis while the other is 0
    //if 1/4 distance to position passed then swap candies

    public void OnPointerDown(PointerEventData pointerEventData)
    {
        if(tInstance.updating)
            return;

    }

    public void OnPointerUp(PointerEventData pointerEventData)
    {

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
