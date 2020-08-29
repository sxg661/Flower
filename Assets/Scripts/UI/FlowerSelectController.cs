using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerSelectController : MonoBehaviour
{
    [SerializeField]
    GameObject flowerPanelPrefab;

    [SerializeField]
    Transform scrollContents;

    int width = 270;

    // Start is called before the first frame update
    void Start()
    {
        //Load in the seed flower buttons
        foreach(Flower flower in PredefinedFlowers.seedFlowers)
        {
            GameObject panel = Instantiate(flowerPanelPrefab, scrollContents);
            RectTransform rectTransform = panel.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(width, rectTransform.rect.height);

            FlowerPanelController controller = panel.GetComponent<FlowerPanelController>();
            controller.flower = flower;
            controller.extraText = "Seed";
        }
    }
}
