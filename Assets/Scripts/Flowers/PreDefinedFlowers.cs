using UnityEngine;
using System.Collections;
using System.Linq;

public class PredefinedFlowers
{
    public static Flower noFlower = new Flower(new Gene[0][], new Fraction[0], FlowerType.NONE, FlowerColour.NONE);
}

