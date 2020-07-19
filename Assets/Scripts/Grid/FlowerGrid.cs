using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class FlowerGrid : MonoBehaviour
{
    public static readonly int GRIDWIDTH = 20;
    public static readonly int GRIDHEIGHT = 20;

    Flower[][] grid;

    public static bool InGrid(int x, int y)
    {
        return !(x < 0 || x > GRIDWIDTH || y < 0 || y > GRIDHEIGHT);
    }

    public static (List<int>, List<int>) getAdjacentSquares(int x, int y)
    {
        int[] xadj = new int[] { x - 1, x - 1, x - 1, x, x + 1, x + 1, x + 1, x};
        int[] yadj = new int[] { y - 1, y, y + 1, y + 1, y + 1, y, y - 1, y - 1 };

        List<int> xadjInBounds = new List<int>();
        List<int> yadjInBounds = new List<int>();

        for (int i = 0; i < xadj.Length; i++)
        {
            if(InGrid(xadj[i], yadj[i]))
            {
                xadjInBounds.Add(xadj[i]);
                yadjInBounds.Add(yadj[i]);
            }   
        }

        return (xadjInBounds, xadjInBounds);


    }

    private void Awake()
    {
        InitialiseGrid();
    }

    /// <summary>
    /// Initiliase a grid of empty flowers with dimensions GRIDWIDTH by GRIDHEIGHT.
    /// </summary>
    public void InitialiseGrid()
    {
        grid = new Flower[GRIDHEIGHT][];

        //fill the grid with empty flowers
        grid = grid.Select(row => (new Grid[GRIDWIDTH].Select(col => PredefinedFlowers.noFlower)).ToArray()).ToArray();
    }

    /// <summary>
    /// Add a flower to a point in the grid.
    /// Will fail if point is not in grid bounds, or if the point is already occupied by another flower.
    /// </summary>
    /// <param name="flower">Flower object to add to the grid</param>
    /// <param name="x">Horizontal coordinate</param>
    /// <param name="y">Vertical coordinate</param>
    /// <returns>true if successfull, false if unsuccessful</returns>
    public bool AddFlower(Flower flower, int x, int y)
    {
        if (!InGrid(x, y))
        {
            return false;
        }

        if (grid[y][x].type != FlowerType.NONE)
        {
            return false;
        }

        grid[y][x] = flower;
        return true;
    }

    /// <summary>
    /// Removes a flower from the grid at a certain location.
    /// Will fail if point is not in grid bounds.
    /// </summary>
    /// <param name="flower">Flower object to remove from the grid</param>
    /// <param name="x">Horizontal coordinate</param>
    /// <param name="y">Vertical coordinate</param>
    /// <returns></returns>
    public bool RemoveFlower(Flower flower, int x, int y)
    {
        if (!InGrid(x, y))
        {
            return false;
        }

        grid[y][x] = PredefinedFlowers.noFlower;
        return true;
    }



    public bool AddOffpSpirng(FlowerType type, FlowerColour colour, int x, int y)
    {
        throw new System.NotImplementedException();
    }


    public Dictionary<string, List<Flower>> GetPossibleParents(FlowerType type, FlowerColour colour, int x, int y)
    {
        throw new System.NotImplementedException();

    }


    
        
 }





