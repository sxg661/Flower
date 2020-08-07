using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TestRunner : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        TestAddOffpsringAndUpdatePriorsOfParents();

    }


    string PointsToString(List<int> x, List<int> y)
    {
        string str = "";
        for (int i = 0; i < x.Count; i++)
        {
            str = String.Format("{0}X: {1} Y: {2}    ", str, x[i], y[i]);
        }
        return str;
    }

    void TestAdjSquares()
    {
        int x = 0, y = 0;
        (List<int> adjx, List<int> adjy) = FlowerGrid.getAdjacentSquares(x, y);
        Debug.Log(String.Format("{1} {2} ---- {0}", PointsToString(adjx, adjy), x, y));

        (x,y) = (FlowerGrid.flowerGrid.gridWidth - 1, 7);
        (adjx, adjy) = FlowerGrid.getAdjacentSquares(x, y);
        Debug.Log(String.Format("{1} {2} ---- {0}", PointsToString(adjx, adjy), x, y));

        (x, y) = (4, FlowerGrid.flowerGrid.gridHeight - 1);
        (adjx, adjy) = FlowerGrid.getAdjacentSquares(x, y);
        Debug.Log(String.Format("{1} {2} ---- {0}", PointsToString(adjx, adjy), x, y));

        (x, y) = (2, 9);
        (adjx, adjy) = FlowerGrid.getAdjacentSquares(x, y);
        Debug.Log(String.Format("{1} {2} ---- {0}", PointsToString(adjx, adjy), x, y));

        (x, y) = (0, 9);
        (adjx, adjy) = FlowerGrid.getAdjacentSquares(x, y);
        Debug.Log(String.Format("{1} {2} ---- {0}", PointsToString(adjx, adjy), x, y));

        (x, y) = (0, FlowerGrid.flowerGrid.gridHeight - 1);
        (adjx, adjy) = FlowerGrid.getAdjacentSquares(x, y);
        Debug.Log(String.Format("{1} {2} ---- {0}", PointsToString(adjx, adjy), x, y));

        (x, y) = (FlowerGrid.flowerGrid.gridWidth- 1, FlowerGrid.flowerGrid.gridHeight - 1);
        (adjx, adjy) = FlowerGrid.getAdjacentSquares(x, y);
        Debug.Log(String.Format("{1} {2} ---- {0}", PointsToString(adjx, adjy), x, y));
    }

    void TestCaseEasy()
    {
        Flower flower1 = FlowerColourLookup.lookup.GetFlowerWithColour(FlowerType.MUM, FlowerColour.GREEN);
        Flower flower2 = FlowerColourLookup.lookup.GetFlowerWithColour(FlowerType.MUM, FlowerColour.GREEN);
        Flower childFlower = flower1.BreedWith(flower2);
        Debug.Log(childFlower);
    }

    void TestCaseEasyShowParentGenes()
    {
        Flower flower1 = FlowerColourLookup.lookup.GetFlowerWithColour(FlowerType.MUM, FlowerColour.GREEN);
        Flower flower2 = FlowerColourLookup.lookup.GetFlowerWithColour(FlowerType.MUM, FlowerColour.GREEN);

        Debug.Log("PARENT 1: " + flower1);
        Debug.Log("PARENT 2: " + flower2);

        var childFlower = flower1.BreedAndShowParentGenes(flower2);
        foreach((Gene[] g1, Gene[] g2, Fraction prob, Flower f) in childFlower)
        {
            Debug.Log(String.Format("PARENT 1: {0}, PARENT 2: {1}, PROB: {2}, CHILD: {3}", Gene.getString(g1), prob, Gene.getString(g2), f));
        }
    }

    void TestCaseHard()
    {
        Gene[] genes2210 = new Gene[] { new Gene('2'), new Gene('2'), new Gene('1'),new Gene('0') };
        Gene[] genes0111 = new Gene[] { new Gene('0'), new Gene('1'), new Gene('1'), new Gene('1') };
        Gene[] genes2100 = new Gene[] { new Gene('2'), new Gene('1'), new Gene('0'), new Gene('0') };

        Gene[][] poss1 = new Gene[][] { genes2210, genes0111, genes2100 };
        Fraction[] likeli1 = new Fraction[] { new Fraction(1, 3), new Fraction(1, 6), new Fraction(1, 2) };

        Gene[][] poss2 = new Gene[][] { genes2100, genes2210 };
        Fraction[] likeli2 = new Fraction[] { new Fraction(1, 3), new Fraction(2, 3) };

        Flower flower1 = new Flower(poss1, likeli1, FlowerType.ROSE, FlowerColour.NONE);
        Flower flower2 = new Flower(poss2, likeli2, FlowerType.ROSE, FlowerColour.NONE);

        Flower childFlower1 = flower1.BreedWith(flower2);
        Debug.Log(childFlower1);
        Debug.Log(Fraction.Sum(childFlower1.genesProbs));

        Flower childFlower2 = flower2.BreedWith(flower1);
        Debug.Log(childFlower2);
        Debug.Log(Fraction.Sum(childFlower2.genesProbs));
    }


    void TestParentPairs()
    {
        Flower flower1 = FlowerColourLookup.lookup.GetFlowerWithColour(FlowerType.MUM, FlowerColour.GREEN);
        Flower flower2 = FlowerColourLookup.lookup.GetFlowerWithColour(FlowerType.MUM, FlowerColour.GREEN);
        Flower flower3 = FlowerColourLookup.lookup.GetFlowerWithColour(FlowerType.MUM, FlowerColour.PURPLE);
        Flower flower4 = FlowerColourLookup.lookup.GetFlowerWithColour(FlowerType.MUM, FlowerColour.YELLOW);
        Flower flower5 = FlowerColourLookup.lookup.GetFlowerWithColour(FlowerType.HYACINTH, FlowerColour.BLUE);

        FlowerGrid.flowerGrid.AddFlower(flower1, 4, 4);
        FlowerGrid.flowerGrid.AddFlower(flower2, 3, 4);
        FlowerGrid.flowerGrid.AddFlower(flower3, 4, 3);
        FlowerGrid.flowerGrid.AddFlower(flower4, 4, 6);
        FlowerGrid.flowerGrid.AddFlower(flower5, 3, 5);

        Dictionary<(int, int, int, int), int> gridSqs = FlowerGrid.flowerGrid.GetPossibleParents(FlowerType.MUM, 4, 5);
        foreach((int,int,int,int) gridSq in gridSqs.Keys)
        {
            Debug.Log(gridSq);
            Debug.Log(gridSqs[gridSq]);
        }
    }

    void TestParentPairsLiklihoodsEasy()
    {
        Flower flower1 = new Flower(new String[] { "221" }, new Fraction[] { new Fraction(1, 1) }, FlowerType.MUM, FlowerColour.GREEN);
        Flower flower2 = new Flower(new String[] { "221" }, new Fraction[] { new Fraction(1, 1) }, FlowerType.MUM, FlowerColour.GREEN);
        Flower flower3 = new Flower(new String[] { "210" }, new Fraction[] { new Fraction(1, 1) }, FlowerType.MUM, FlowerColour.PURPLE);
        Flower flower4 = new Flower(new String[] { "220", "221" }, new Fraction[] { new Fraction(1, 2), new Fraction(1, 2) }, FlowerType.MUM, FlowerColour.GREEN);

        FlowerGrid.flowerGrid.AddFlower(flower1, 1, 1);
        FlowerGrid.flowerGrid.AddFlower(flower2, 1, 0);
        FlowerGrid.flowerGrid.AddFlower(flower3, 2, 2);
        FlowerGrid.flowerGrid.AddFlower(flower4, 0, 3);


        Dictionary<(int, int, int, int), int> gridSqs = FlowerGrid.flowerGrid.GetPossibleParents(FlowerType.MUM, 1, 2);
        Dictionary<(int, int, int, int), Fraction> gridSqsLikli = FlowerGrid.flowerGrid.GetLiklihoodsOfParents(gridSqs, FlowerColour.GREEN);
        foreach((int, int, int, int) gridSq in gridSqsLikli.Keys)
        {
            Debug.Log(gridSq);
            Debug.Log(gridSqsLikli[gridSq]);
        }

        Dictionary<(int,int),List<(int,int,Fraction)>> flowerCombos = FlowerGrid.flowerGrid.GetCombosForEachFlower(gridSqsLikli);
        foreach((int x, int y) in flowerCombos.Keys)
        {
            Debug.Log(String.Format("FLOWER AT {0},{1}", x, y));
            foreach((int x2, int y2, Fraction liklihood) in flowerCombos[(x, y)])
            {
                Debug.Log(String.Format("{0}, {1}, liklihood {2}", x2, y2, liklihood));
            }
        }
    }


    void TestAddOffspring()
    {
        Flower flower1 = new Flower(new String[] { "221" }, new Fraction[] { new Fraction(1, 1) }, FlowerType.MUM, FlowerColour.GREEN);
        Flower flower2 = new Flower(new String[] { "221" }, new Fraction[] { new Fraction(1, 1) }, FlowerType.MUM, FlowerColour.GREEN);
        Flower flower3 = new Flower(new String[] { "210" }, new Fraction[] { new Fraction(1, 1) }, FlowerType.MUM, FlowerColour.PURPLE);
        Flower flower4 = new Flower(new String[] { "220", "221" }, new Fraction[] { new Fraction(1, 2), new Fraction(1, 2) }, FlowerType.MUM, FlowerColour.GREEN);

        FlowerGrid.flowerGrid.AddFlower(flower1, 1, 1);
        FlowerGrid.flowerGrid.AddFlower(flower2, 1, 0);
        FlowerGrid.flowerGrid.AddFlower(flower3, 2, 2);
        FlowerGrid.flowerGrid.AddFlower(flower4, 0, 3);


        FlowerGrid.flowerGrid.AddOffspring(FlowerType.MUM, FlowerColour.GREEN, 1, 2);

        Debug.Log(FlowerGrid.flowerGrid.GetFlower(1,2));

    }

    void TestAddOffpsringAndUpdatePriorsOfParents()
    {
        Flower flower1 = new Flower(new String[] { "221", "220"}, new Fraction[] { new Fraction(1, 3), new Fraction(2,3) }, FlowerType.MUM, FlowerColour.GREEN);
        Flower flower2 = new Flower(new String[] { "220" }, new Fraction[] { new Fraction(1, 1) }, FlowerType.MUM, FlowerColour.GREEN);
        Flower flower3 = new Flower(new String[] { "210" }, new Fraction[] { new Fraction(1, 1) }, FlowerType.MUM, FlowerColour.PURPLE);
        Flower flower4 = new Flower(new String[] { "210", "211" }, new Fraction[] { new Fraction(1, 2), new Fraction(1, 2) }, FlowerType.MUM, FlowerColour.PURPLE);

        FlowerGrid.flowerGrid.AddFlower(flower1, 1, 1);
        FlowerGrid.flowerGrid.AddFlower(flower2, 2, 2);
        FlowerGrid.flowerGrid.AddFlower(flower3, 1, 0);
        FlowerGrid.flowerGrid.AddFlower(flower4, 0, 3);

        FlowerGrid.flowerGrid.AddOffspring(FlowerType.MUM, FlowerColour.GREEN, 1, 2);

    }

    // Update is called once per frame
    void Update()
    {
        
        
    }
}
