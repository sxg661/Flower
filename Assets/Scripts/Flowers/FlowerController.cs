using UnityEngine;
using System.Collections;

public class FlowerController : MonoBehaviour
{
    Flower flower;

    SpriteRenderer renderer;

    public void Awake()
    {
    }

    public void Start()
    {
        Flower flower = FlowerColourLookup.lookup.GetFlowerWithColour(FlowerType.COSMOS, FlowerColour.YELLOW);
        renderer = GetComponent<SpriteRenderer>();

        string imageName = string.Format("{0}/{1}", flower.type, flower.colour);
        Sprite flowerSprite = Resources.Load<Sprite>(imageName);

        if(flowerSprite == null)
        {
            flowerSprite = Resources.Load<Sprite>("OOPS");
        }

        renderer.sprite = flowerSprite;
    }


}
