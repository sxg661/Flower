using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System;

public class FlowerGrid : MonoBehaviour
{

    public static FlowerGrid flowerGrid;

    public int gridWidth = 20;
    public int gridHeight = 20;

    Flower[][] grid;

    public static bool InGrid(int x, int y)
    {
        return x >= 0 && x < flowerGrid.gridWidth && y >= 0 && y < flowerGrid.gridHeight;
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
        grid = new Flower[gridHeight][];

        //fill the grid with empty flowers
        grid = grid.Select(row => (new Grid[gridWidth].Select(col => PredefinedFlowers.noFlower)).ToArray()).ToArray();
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


    /// <summary>
    /// Add flower to grid as offpsring, implying that the surrouding flowers are parents.
    /// Will work out the possible genes of the offpsring and update possible genes of possible parent flowers.
    /// </summary>
    /// <param name="type"></param>
    /// <param name="colour"></param>
    /// <param name="x">x position in grid</param>
    /// <param name="y">y position in grid</param>
    /// <returns>true if parents found, false if not valid parents found</returns>
    public bool AddOffspring(FlowerType type, FlowerColour colour, int x, int y)
    {
        if(GetFlower(x,y).type != FlowerType.NONE)
        {
            return false;
        }

        Dictionary<(int, int, int, int), int> possibleParents = GetPossibleParents(type, x, y);
        if(possibleParents.Count == 0)
        {
            return false;
        }

        Dictionary<(int, int, int, int), Fraction> parentLiklihoods = GetLiklihoodsOfParents(possibleParents, colour);
        if(parentLiklihoods.Count == 0)
        {
            return false;
        }

        Dictionary<(int, int), List<(int, int, Fraction)>> combosPerFlower = GetCombosForEachFlower(parentLiklihoods);

        // get the possible genes of this offspring and add it to the grid
        var genesOffspring = new Dictionary<String, Fraction>();
        foreach((int x1, int y1, int x2, int y2) in parentLiklihoods.Keys)
        {
            Flower offspring = GetFlower(x1, y1);
            if(x1 != x2 || y1 != y2)
            {
                offspring = GetFlower(x1, y1).BreedWith(GetFlower(x2, y2));
            }

            for(int i = 0; i < offspring.genesPoss.Count(); i++)
            {
                string offspringGeneCode = Gene.getString(offspring.genesPoss[i]);
                if(FlowerColourLookup.lookup.colourLookup[type][offspringGeneCode] == colour)
                {
                    if (!genesOffspring.ContainsKey(offspringGeneCode))
                    {
                        genesOffspring[offspringGeneCode] = new Fraction(0, 0);
                    }
                    genesOffspring[offspringGeneCode] += offspring.genesProbs[i] * parentLiklihoods[(x1,y1,x2,y2)];
                }
                
            }
        }
        AddFlower(new Flower(genesOffspring.Keys.ToArray(), Fraction.Normalise(genesOffspring.Values.ToArray()), type, colour), x, y);

        UpdatePriorBeliefs(combosPerFlower, colour);


        return true;

    }

    /// <summary>
    /// Update the beliefs for any parents that could be the parent of a certain flower.
    /// </summary>
    /// <param name="combosPerFlower">For each flower, its possible mates and the likelyhood of the parent combination</param>
    /// <param name="offpsringColour">The colour of the offpsring produced</param>
    public void UpdatePriorBeliefs(Dictionary<(int, int), List<(int, int, Fraction)>> combosPerFlower, FlowerColour offpsringColour)
    {
        //iterate through the flowers
        foreach((int x, int y) in combosPerFlower.Keys)
        {
            Flower flower = GetFlower(x, y);

            if(flower.genesPoss.Length == 1)
            {
                continue;
            }

            Fraction liklihoodParent = new Fraction(0, 0);
            Fraction[] geneLiklils = new Fraction[flower.genesPoss.Length];

            //iterate through each potential mate for this flower
            foreach ((int xOther, int yOther, Fraction liklihood) in combosPerFlower[(x, y)])
            {
                liklihoodParent += liklihood;

                Fraction[] geneLiklilsGivenPair = new Fraction[flower.genesPoss.Length];
                String[] geneIDs = flower.genesPoss.Select(g => Gene.getString(g)).ToArray();

                Flower flowerOther = GetFlower(xOther, yOther);


                //based on this pair, work out the liklihoods of genes for this flower given the offpsring

                //first work out the probablity of the offspring given the genes
                Fraction[] offspringProbsGivenGenes = new Fraction[geneLiklils.Length];

                var offpsringWithParentGenes = flower.BreedAndShowParentGenes(flowerOther);
                foreach ((Gene[] genes, Gene[] genesOther, Fraction probGenes, Flower offpsring) in offpsringWithParentGenes)
                {

                    string gID = Gene.getString(genes);
                    int index = Array.IndexOf(geneIDs, gID);

                    for(int i = 0; i < offpsring.genesPoss.Length; i++)
                    {
                        String offpsringGeneID = Gene.getString(offpsring.genesPoss[i]);
                        if(FlowerColourLookup.lookup.colourLookup[flower.type][offpsringGeneID] == offpsringColour)
                        {
                            offspringProbsGivenGenes[index] += offpsring.genesProbs[i] * probGenes;
                        }
                    }
                }

                //now use Bayes to get the probability of the genes given the offspring and add it to our so far discovered gene liklihoods
                Fraction[] geneProbsGivenOffspring = Fraction.Normalise(offspringProbsGivenGenes);
                geneLiklils = geneLiklils.Zip(geneProbsGivenOffspring.Select(l => l * liklihood), (f1, f2) => f1 + f2).ToArray();
            }

            if (liklihoodParent.numerator == 0)
            {
                continue;
            }

            //if there is a chance that the flower is not the parent, we need to account for this too!
            if(liklihoodParent.numerator != liklihoodParent.denominator)
            {
                Fraction likliNotParent =  new Fraction(1, 1) - liklihoodParent;
                geneLiklils = geneLiklils.Zip(flower.genesProbs.Select(p => p * likliNotParent), (f1, f2) => f1 + f2).ToArray();
            }

            Flower newFlower = new Flower(flower.genesPoss, geneLiklils, flower.type, flower.colour);

            

        }
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
    /// Gets a unique hash value of an index in the grid.
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
    /// Gets the liklihood that each pair of possible parents in the pair that created the offpsring of a certain colour.
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
            // (cloning does not work like breeding as far as I know, as the flower just copies itself rather than breeding
            // with itself)
            var offspring = parent1;
            if (x1 != x2 || y1 != y2)
            {
                offspring = parent1.BreedWith(parent2);
            }

            //look at the potential offspring of these parents and see which match the colour we are looking for
            // from this get a probabity of these parents producing the offspring i.e. p(offspring | parents)
            Fraction probOfColourGivenParents = new Fraction(0, 0);
            for(int j = 0; j < offspring.genesPoss.Length; j++)
            {
                string geneID = Gene.getString(offspring.genesPoss[j]);
                FlowerColour colour = FlowerColourLookup.lookup.colourLookup[offspring.type][geneID];
                if(colour == offspringColour)
                {
                    probOfColourGivenParents += offspring.genesProbs[j];
                }
            }
            offspringProbsGivenParents[i] = probOfColourGivenParents;

            //now get the probablity of these parents the parents given any offspring, i.e. p(parents)
            parentProbs[i] = new Fraction(parentPairsOccurances[(x1, y1, x2, y2)], numCombos);
        }

        //now calculate the liklihood of the parents given the offspring i.e. p(parents | offspring)
        // does this using bayes: p(a | b) = ( p(b | a) * p(a) )/ p(b)
        //  (this is done in one line below by getting all the p(a | b) * p(a) values in a list and normalising it (making add up to 1)
        //  the normalisation is essentially the the equivolent of doing p(b), as p(b) = sum( p(b | a) * p(a) ).
        Fraction[] probsbOffpring = Fraction.Normalise(offspringProbsGivenParents.Zip(parentProbs, (prob1, prob2) => prob1 * prob2).ToArray());
        var liklihoodParentsGivenOffspring = new Dictionary<(int, int, int, int), Fraction>();
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

    /// <summary>
    /// Reorganise a dictionary of (combinations, liklihood) to a dictionary of (flower, possible mates and liklihood of combo)
    /// This will allow the prior beliefs of each flower to be easily updated down the line.
    /// </summary>
    /// <param name="comboLiklis"></param>
    /// <returns>Dictionary : (x1, y1) of flower -> List: (x2,y2) of mate, liklihood of parent pair</returns>
    public Dictionary<(int, int), List<(int, int, Fraction)>> GetCombosForEachFlower(Dictionary<(int,int,int,int), Fraction> comboLiklihoods)
    {
        var combosForFlowers = new Dictionary<(int, int), List<(int, int, Fraction)>>();

        foreach((int x1, int y1, int x2, int y2) in comboLiklihoods.Keys)
        {

            Fraction liklihood = comboLiklihoods[(x1,y1,x2,y2)];

            if (!combosForFlowers.ContainsKey((x1, y1)))
            {
                combosForFlowers[(x1, y1)] = new List<(int, int, Fraction)>();
            }
            combosForFlowers[(x1, y1)].Add((x2, y2, liklihood));

            //if this is a cloning flower we are done now, so move on to the 
            if(x1 == x2  && y1 == y2)
            {
                continue;
            }

            if (!combosForFlowers.ContainsKey((x2, y2)))
            {
                combosForFlowers[(x2, y2)] = new List<(int, int, Fraction)>();
            }
            combosForFlowers[(x2, y2)].Add((x1, y1, liklihood));

        }

        return combosForFlowers;
    } 




}





