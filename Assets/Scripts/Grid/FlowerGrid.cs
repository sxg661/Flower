using UnityEngine;
using System.Collections;
using System.Linq;

public class FlowerGrid : MonoBehaviour
{
    public static readonly int GRIDWIDTH = 20;
    public static readonly int GRIDHEIGHT = 20;

    Flower[][] grid;

    private void Awake()
    {
        grid = new Flower[GRIDHEIGHT][];

        //initialise the grid full of empty flowers
        grid = grid.Select(row => (new Grid[GRIDWIDTH].Select(col => PredefinedFlowers.noFlower)).ToArray()).ToArray();
    }

    /// <summary>
    /// Add a flower to the grid at a certain point in the grid.
    /// Will fail if point is not in grid, or if the point is already occupied by another flower.
    /// </summary>
    /// <param name="flower"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns>true if successfull, false if unsuccessful</returns>
    public bool AddFlower(Flower flower, int x, int y)
    {
        if (x < 0 || x > GRIDWIDTH || y < 0 || y > GRIDHEIGHT)
        {
            return false;
        }
        
        if(grid[y][x].type != FlowerType.NONE)
        {
            return false;
        }

        grid[y][x] = flower;
        return true;
    }




}
