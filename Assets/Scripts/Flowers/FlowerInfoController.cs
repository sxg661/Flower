using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlowerInfoController : MonoBehaviour
{

    [SerializeField]
    Text titleText;

    [SerializeField]
    Image flowerImage;

    FlowerController currentController;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    string FormatCases(string str)
    {
        string strFirst = str.Substring(0, 1).ToUpper();
        string strSecond = str.Substring(1, str.Length - 1).ToLower();
        return strFirst + strSecond;
    }

    // Update is called once per frame
    void Update()
    {
        FlowerController controller = SimulationController.singleton.SelectedFlower;

        if (controller == currentController)
        {
            return;
        }

        if (controller == null)
        {
            titleText.text = "No Flower Selected";
            flowerImage.enabled = false;
            currentController = null;
            return;
        }

        flowerImage.enabled = true;

        currentController = controller;

        string title = string.Format("{0} {1}", FormatCases(controller.flower.colour.ToString()), FormatCases(controller.flower.type.ToString()));
        titleText.text = title;

        Sprite imageSprite = controller.spriteRenderer.sprite;
        flowerImage.sprite = imageSprite;


    }
}
