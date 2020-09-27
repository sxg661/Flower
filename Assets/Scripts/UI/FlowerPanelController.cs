using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class FlowerPanelController : MonoBehaviour
{
    [SerializeField]
    Image flowerImage;

    [SerializeField]
    Text flowerNameText;

    [SerializeField]
    GameObject flowerGhostPrefab;

    Action onClickAction = null;

    public Flower flower;

    //e.g. Seed or Island
    public string text;

    /// <summary>
    /// Give the flower panel controller all the information it needs to render 
    /// the flower panel correctly.
    /// </summary>
    /// <param name="width">The width of the panel in pixels.</param>
    /// <param name="flower">The flower that the panel reperesents.</param>
    /// <param name="text">An text to put on the panel.</param>
    /// <param name="action">The action peformed when the panel is clicked.</param>
    /// </summary>
    public void GiveInfo(
        int width, 
        Flower flower, 
        string text,
        Action clickAction
        )
    { 
        SetWidth(width);
        this.flower = flower;
        if (clickAction != null)
        {
            AssignClickAction(clickAction);
        }
        this.text = text;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        if(flower == null)
        {
            return;
        }

        //get the flower image from resources
        string spriteFilePath = flower.GetFilePath();
        Sprite flowerSprite = Resources.Load<Sprite>(spriteFilePath);
        flowerImage.sprite = flowerSprite;

        //format the text
        flowerNameText.text = text;
    }

    public void CreateGhost()
    {
        GameObject obj = Instantiate(flowerGhostPrefab, Vector3.zero, Quaternion.identity);
        obj.GetComponent<FlowerGhostController>().flower = flower;
    }

    public void SetWidth(int width)
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(width, rectTransform.rect.height);
    }

    public void AssignClickAction(Action action)
    {
        onClickAction = action;
    }

    public void Click()
    {
        onClickAction?.Invoke();
    }
}
