using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerGhostController : MonoBehaviour, IInteractable
{
    int x;

    int y;

    public Flower flower;

    bool place = false;

    SpriteRenderer renderer;

    private void Start()
    {
        if(SimulationController.singleton.currentGhost != null)
        {
            Destroy(SimulationController.singleton.currentGhost.gameObject);
        }
        SimulationController.singleton.currentGhost = this;
        SimulationController.singleton.selectedFlower = null;

        string spriteFilePath = flower.GetFilePath();
        Sprite flowerSprite = Resources.Load<Sprite>(spriteFilePath);
        renderer = GetComponent<SpriteRenderer>();
        renderer.sprite = flowerSprite;
    }

    private void OnDestroy()
    {
        if (SimulationController.singleton.currentGhost == this)
        {
            SimulationController.singleton.currentGhost = null;
        }
    }

    private void Update()
    {
        Vector3 positioin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        (int newX, int newY) = GridRenderer.GetTileNum(positioin);

        transform.position = GridRenderer.GetWorldPos(newX, newY, 1, 1);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Destroy(gameObject);
        }

        if(place && !ClickDetector.clickDetector.overGUI)
        {
            FlowerGrid.flowerGrid.AddAndRenderFlower(flower, newX, newY);
            place = false;
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
        place = true;
    }
}
