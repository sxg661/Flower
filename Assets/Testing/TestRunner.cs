using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TestRunner : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

        TestCaseHard();


       
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

    // Update is called once per frame
    void Update()
    {
        
        
    }
}
