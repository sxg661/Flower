using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System;

public class FlowerGrid : MonoBehaviour
{
    public static readonly int GRIDWIDTH = 20;
    public static readonly int GRIDHEIGHT = 20;

    public static FlowerGrid flowerGrid;

    Flower[][] grid;

    public static bool InGrid(int x, int y)
    {
        return x >= 0 && x < GRIDWIDTH && y >= 0 && y < GRIDHEIGHT;
    }


    /// <summary>
    /// Gets all the adjacent squares of a tile that are within grid bounds.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public static (List<int>, List<int>) getAdjacentSquares(int x, int y)
    {
        int[] xadj = new int[] { x - 1, x - 1, x - 1, x, x + 1, x + 1, x + 1, x };
        int[] yadj = new int[] { y - 1, y, y + 1, y + 1, y + 1, y, y - 1, y - 1 };

        List<int> xadjInBounds = new List<int>();
        List<int> yadjInBounds = new List<int>();

        for (int i = 0; i < xadj.Length; i++)
        {
            int xval = xadj[i];
            int yval = yadj[i];

            if (InGrid(xval, yval))
            {
                xadjInBounds.Add(xval);
                yadjInBounds.Add(yval);
            }
        }

        return (xadjInBounds, yadjInBounds);


    }

    private void Awake()
    {
        flowerGrid = this;
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

    public Flower GetFlower(int x, int y)
    {
        if (!InGrid(x, y))
        {
            return PredefinedFlowers.noFlower;
        }

        return grid[y][x];
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



    /// <summary>
    /// Gets all the possible flower
    /// </summary>
    /// <param name="type"></param>
    /// <param name="colour"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns> Dictionary: key - (parent1x, p""1y, p""2x,  p""2y), value - Occurances</returns>
    public Dictionary<(int, int, int, int), int> GetPossibleParents(FlowerType type, int x, int y)
    {
        (List<int> primaryParentXs, List<int> primaryParentYs) = getAdjacentSquares(x, y);
        int total = 0;

        var parentPairs = new Dictionary<(int, int, int, int), int>();

        for (int i = 0; i < primaryParentXs.Count; i++)
        {
            int parent1x = primaryParentXs[i];
            int parent1y = primaryParentYs[i];

            if (GetFlower(parent1x, parent1y).type != type)
            {
                continue;
            }


            (List<int> secondaryParentsXs, List<int> secondaryParentsYs) = getAdjacentSquares(parent1x, parent1y);

            int numPartnersFound = 0;

            for (int j = 0; j < secondaryParentsXs.Count; j++)
            {
                int parent2x = secondaryParentsXs[j];
                int parent2y = secondaryParentsYs[j];

                if (parent2x == x && parent2y == y)
                {
                    continue;
                }

                if (GetFlower(parent2x, parent2y).type != type)
                {
                    continue;
                }

                (int, int, int, int) point;

                //use unique hashes of coordinates to make sures pairs are always in the same order
                if (Hash(parent1x, parent1y) < Hash(parent2x, parent2y))
                {
                    point = (parent1x, parent1y, parent2x, parent2y);
                }
                else
                {
                    point = (parent2x, parent2y, parent1x, parent1y);
                }

                if (!parentPairs.ContainsKey(point))
                {
                    parentPairs[point] = 0;
                }
                parentPairs[point] += 1;

                numPartnersFound++;


            }

            if (numPartnersFound == 0)
            {
                (int, int, int, int) point = (parent1x, parent1y, parent1x, parent1y);
                parentPairs[point] = 1;
            }
        }
        return parentPairs;
    }
        
    /// <summary>
    /// 
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    private int Hash(int x, int y)
    {
        String str = String.Format("{0} {1}", x, y);
        return str.GetHashCode();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="parentPairs"></param>
    /// <param name="offspringColour"></param>
    /// <returns></returns>
    public Dictionary<(int,int,int,int), Fraction> GetLiklihoodsOfParents(Dictionary<(int, int, int, int), int> parentPairsOccurances, FlowerColour offspringColour)
    {
        (int, int, int, int)[] parentPairs = parentPairsOccurances.Keys.ToArray();

        var offspringProbsGivenParents = new Fraction[parentPairs.Length];
        var parentProbs = new Fraction[parentPairs.Length];

        int numCombos = parentPairsOccurances.Values.ToArray().Sum();

        // first get the probablity of the offspring given the parents.
        for( int i = 0; i < parentPairs.Length; i++)
        {
            (int x1, int y1, int x2,int y2) = parentPairs[i];
            Flower parent1 = GetFlower(x1, y1);
            Flower parent2 = GetFlower(x2, y2);

            //if it is the same flower with itself it is cloning, so just use the parent flower itself as the offspring
            // (cloning does not work just breeding as far as I know, as the flower just copies itself rather than breeding
            // with itself.
            var offspring = parent1;
            if (x1 != x2 || y1 != y2)
            {
                offspring = parent1.BreedWith(parent2);
            }

            //look at the potential offspring of these parents and see which match the colour we are looking for
            // if so, add the probablitiy of the offspring to the overall probablitility of the offpsring of this colour occuring
            // given this pare
            Fraction probOfColourGivenParents = new Fraction(0, 0);
            for(int j = 0; j < offspring.genesPoss.Length; j++)
            {
                Gene[] genes = offspring.genesPoss[j];
                FlowerColour colour = FlowerColourLookup.lookup.colourLookup[offspring.type][Gene.getString(genes)];
                if(colour == offspringColour)
                {
                    probOfColourGivenParents += offspring.genesProbs[j];
                }
            }

            offspringProbsGivenParents[i] = probOfColourGivenParents;
            parentProbs[i] = new Fraction(parentPairsOccurances[(x1, y1, x2, y2)], numCombos);
        }

        //now calculate the liklihood of the parents given the offspring usig bayes
        var liklihoodParentsGivenOffspring = new Dictionary<(int, int, int, int), Fraction>();
        Fraction[] probsbOffpring = Fraction.Normalise(offspringProbsGivenParents.Zip(parentProbs, (prob1, prob2) => prob1 * prob2).ToArray());

        for (int i = 0; i < parentPairs.Length; i++)
        {
            (int x1, int y1, int x2, int y2) = parentPairs[i];
            if(probsbOffpring[i].numerator != 0)
            {
                liklihoodParentsGivenOffspring[(x1, y1, x2, y2)] = probsbOffpring[i];
            }
        }

        return liklihoodParentsGivenOffspring;
    }



}





