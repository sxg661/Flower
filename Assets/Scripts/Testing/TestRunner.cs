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
        TestAdjSquares();
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

        (x,y) = (FlowerGrid.GRIDWIDTH - 1, 7);
        (adjx, adjy) = FlowerGrid.getAdjacentSquares(x, y);
        Debug.Log(String.Format("{1} {2} ---- {0}", PointsToString(adjx, adjy), x, y));

        (x, y) = (4, FlowerGrid.GRIDHEIGHT - 1);
        (adjx, adjy) = FlowerGrid.getAdjacentSquares(x, y);
        Debug.Log(String.Format("{1} {2} ---- {0}", PointsToString(adjx, adjy), x, y));

        (x, y) = (2, 9);
        (adjx, adjy) = FlowerGrid.getAdjacentSquares(x, y);
        Debug.Log(String.Format("{1} {2} ---- {0}", PointsToString(adjx, adjy), x, y));

        (x, y) = (0, 9);
        (adjx, adjy) = FlowerGrid.getAdjacentSquares(x, y);
        Debug.Log(String.Format("{1} {2} ---- {0}", PointsToString(adjx, adjy), x, y));

        (x, y) = (0, FlowerGrid.GRIDHEIGHT - 1);
        (adjx, adjy) = FlowerGrid.getAdjacentSquares(x, y);
        Debug.Log(String.Format("{1} {2} ---- {0}", PointsToString(adjx, adjy), x, y));

        (x, y) = (FlowerGrid.GRIDWIDTH - 1, FlowerGrid.GRIDHEIGHT - 1);
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

    void TestCaseColour()
    {
        Flower flower1 = FlowerColourLookup.lookup.GetFlowerWithColour(FlowerType.MUM, FlowerColour.GREEN);
        Flower flower2 = FlowerColourLookup.lookup.GetFlowerWithColour(FlowerType.MUM, FlowerColour.GREEN);
        Flower childFlower = flower1.GetOffspringWithColour(flower2, FlowerColour.GREEN);
        Debug.Log(childFlower);
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }
}
