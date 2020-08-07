using UnityEngine;
using System.Collections;

public class GhostTileController : MonoBehaviour, IInteractable
{
    Pointer<bool> hover;
    Pointer<bool> click;

    [SerializeField]
    Sprite normal;

    [SerializeField]
    Sprite activated;

    SpriteRenderer myRenderer;
    GridRenderer tileGrid;

    private void Start()
    {
        myRenderer = GetComponent<SpriteRenderer>();
    }
    
    public void Init(GridRenderer grid, Pointer<bool> click, Pointer<bool> hover)
    {
        tileGrid = grid;
        this.click = click;
        this.hover = hover;
    }

    public void Update()
    {

        if (hover.value)
        {
            myRenderer.sprite = activated;
        }
        else
        {
            myRenderer.sprite = normal;
        }
    }

    public void MouseEnter()
    {
        hover.value = true;
    }

    public void MouseExit()
    {
        hover.value = false;
    }


    public void MouseClick()
    {
        click.value = true;
    }


}
