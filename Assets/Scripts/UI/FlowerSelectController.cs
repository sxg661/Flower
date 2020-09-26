using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
            FlowerPanelController controller = panel.GetComponent<FlowerPanelController>();
            controller.GiveInfo(width, flower, string.Format("{0} {1}", "Seed", flower.GetName()), controller.CreateGhost);
        }
    }
}
