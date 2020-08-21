using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerSelectController : MonoBehaviour
{
    [SerializeField]
    GameObject flowerPanelPrefab;

    [SerializeField]
    Transform scrollContents;

    // Start is called before the first frame update
    void Start()
    {
        //Load in the seed flower buttons
        foreach(Flower flower in PredefinedFlowers.seedFlowers)
        {
            GameObject panel = Instantiate(flowerPanelPrefab, scrollContents);
            FlowerPanelController controller = panel.GetComponent<FlowerPanelController>();
            controller.flower = flower;
            controller.extraText = "Seed";
        }
    }
}
