using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is a grid rendering script, which renders the information contained in FlowerGrid.
/// </summary>
public class GridRenderer : MonoBehaviour
{

    public static Vector3 GetWorldPos(int x, int y, float width, float height)
    {
        return new Vector3(x + width/2, y + height/2, 0);
    }

    public static (int, int) GetTileNum(Vector3 worldPos)
    {
        return ((int) Mathf.Floor(worldPos.x), (int) Mathf.Floor(worldPos.y));
    }

    [SerializeField]
    GameObject tilePrefab;

    [SerializeField]
    GameObject ghostTilePrefab;


    //no ghosts for now, might have them in the future though!
    bool showGhosts = false;

    private Pointer<bool> rowClick = new Pointer<bool>(false);
    private Pointer<bool> columnClick = new Pointer<bool>(false);

    List<GameObject> ghostTiles = new List<GameObject>();
    List<GameObject> wallTiles = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        //render the supermarket floor
        for (int x = 0; x < FlowerGrid.flowerGrid.gridWidth; x++)
        {
            for (int y = 0; y < FlowerGrid.flowerGrid.gridHeight; y++)
            {
               Instantiate(tilePrefab, GetWorldPos(x, y, 1, 1), Quaternion.identity);
               
            }
        }
        if (showGhosts)
        {
            AddGhosts();
        }

    }


    void AddGhosts()
    {
        rowClick.value = false;


        Pointer<bool> rowHover = new Pointer<bool>(false);
        for (int x = 0; x < FlowerGrid.flowerGrid.gridWidth; x++)
        {
            GameObject ghost = Instantiate(ghostTilePrefab, GetWorldPos(x, FlowerGrid.flowerGrid.gridHeight, 1, 1), Quaternion.identity);
            ghost.GetComponent<GhostTileController>().Init(this, rowClick, rowHover);
            ghostTiles.Add(ghost);
        }
        

        columnClick.value = false;
        Pointer<bool> columnHover = new Pointer<bool>(false);
        for (int y = 0; y < FlowerGrid.flowerGrid.gridHeight; y++)
        {
            GameObject ghost = Instantiate(ghostTilePrefab, GetWorldPos(FlowerGrid.flowerGrid.gridWidth, y, 1, 1), Quaternion.identity);
            ghost.GetComponent<GhostTileController>().Init(this, columnClick, columnHover);
            ghostTiles.Add(ghost);
        }
        
       

    }

    void DestroyGhosts()
    {
        foreach(GameObject ghost in ghostTiles)
        {
            Destroy(ghost);
        }
        ghostTiles = new List<GameObject>();
    }

    void DestroyWalls()
    {
        foreach(GameObject wall in wallTiles)
        {
            Destroy(wall);
        }
        wallTiles = new List<GameObject>();
    }

    void AddRow()
    {
        for(int x = 0; x < FlowerGrid.flowerGrid.gridWidth; x++)
        {
            Instantiate(tilePrefab, GetWorldPos(x, FlowerGrid.flowerGrid.gridHeight, 1, 1), Quaternion.identity);
        }
        FlowerGrid.flowerGrid.gridHeight += 1;
        //TODO I NEED TO DO MORE HERE TO MAKE THERE BE SPACE FOR FLOWERS IN THESE SQUARES!!!!
    }

    void AddColumn()
    {
        for(int y = 0; y < FlowerGrid.flowerGrid.gridHeight; y++)
        {
            Instantiate(tilePrefab, GetWorldPos(FlowerGrid.flowerGrid.gridWidth, y, 1, 1), Quaternion.identity);
        }
        FlowerGrid.flowerGrid.gridWidth += 1;
        //TODO SOME AS ABOVE FUNCTION
    }

    public void ResetClicks()
    {
        rowClick.value = false;
        columnClick.value = false;
    }


    public void ExpandGrid()
    {
        DestroyGhosts();
        DestroyWalls();
        int cost = 0;
        if(rowClick.value == true)
        {
            AddRow();
        }
        else if(columnClick.value == true)
        {
            AddColumn();
        }
        ResetClicks();
        AddGhosts();
    }

}
