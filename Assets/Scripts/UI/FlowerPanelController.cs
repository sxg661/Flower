using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlowerPanelController : MonoBehaviour
{
    [SerializeField]
    Image flowerImage;

    [SerializeField]
    Text flowerNameText;

    [SerializeField]
    GameObject flowerGhostPrefab;

    public Flower flower;

    //e.g. Seed or Island
    public string extraText;
    
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
        string flowerName = flower.GetName();
        if(extraText != "" && extraText != null)
        {
            flowerName = string.Format("{0} {1}", extraText, flowerName);
        }
        flowerNameText.text = flowerName;
    }

    public void CreateGhost()
    {
        GameObject obj = Instantiate(flowerGhostPrefab, Vector3.zero, Quaternion.identity);
        obj.GetComponent<FlowerGhostController>().flower = flower;
    }

}
