using UnityEngine;
using System.Collections;
using System;

public class FlowerController : MonoBehaviour, IInteractable
{
    public Flower flower;

    public SpriteRenderer spriteRenderer;

    [SerializeField]
    SpriteRenderer selectedImage;

    [SerializeField]
    Color GhostColor;

    [SerializeField]
    Color DefaultColor;

    [SerializeField]
    int x;

    [SerializeField]
    int y;

    public bool beingDragged;

    public void Awake()
    {
    }

    public void GiveDetails(Flower flower, int x, int y)
    {
        this.flower = flower;
        this.x = x;
        this.y = y;
    }


    public void Start()
    {
        transform.position = GridRenderer.GetWorldPos(x, y, 1, 1);

        //Adds itself to the grid
        FlowerGrid.flowerGrid.AddFlower(flower, x, y);

        //Get the correct image
        string imageName = string.Format("{0}/{1}", flower.type, flower.colour);
        Sprite flowerSprite = Resources.Load<Sprite>(imageName);
        if (flowerSprite == null)
        {
            flowerSprite = Resources.Load<Sprite>("OOPS");
        }

        //Change the sprite
        spriteRenderer.sprite = flowerSprite;
    }

    public void Update()
    {
        UpdatePosition();
        UpdateAppearance();
    }

    private void UpdateAppearance()
    {
        selectedImage.enabled = SimulationController.singleton.SelectedFlower == this;

        if (beingDragged)
        {
            spriteRenderer.color = GhostColor;
            spriteRenderer.sortingOrder = 1;
        }
        else
        {
            spriteRenderer.color = DefaultColor;
            spriteRenderer.sortingOrder = 0;
        }
    }

    public void UpdatePosition()
    {
        if (beingDragged)
        {
            Vector3 positioin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            (int newX, int newY) = GridRenderer.GetTileNum(positioin);

            if (Input.GetMouseButtonUp(0) || Input.GetKeyDown(KeyCode.Escape))
            {
                beingDragged = false;
                if (!FlowerGrid.IsValidTilePlacement(newX, newY) || Input.GetKeyDown(KeyCode.Escape))
                {
                    transform.position = GridRenderer.GetWorldPos(x, y, 1, 1);
                    return;
                }

                transform.position = GridRenderer.GetWorldPos(newX, newY, 1, 1);

                FlowerGrid.flowerGrid.RemoveFlower(x, y);
                x = newX;
                y = newY;
                FlowerGrid.flowerGrid.AddFlower(flower, x, y);


            }
            else
            {
                transform.position = GridRenderer.GetWorldPos(newX, newY, 1, 1);
            }

        }
    }


    public void MouseEnter()
    {
        return;
    }

    public void MouseExit()
    {
        return;
    }

    public void MouseButtonDown()
    {
        beingDragged = true;

        SimulationController.singleton.SelectedFlower = this;
        
    }
}
