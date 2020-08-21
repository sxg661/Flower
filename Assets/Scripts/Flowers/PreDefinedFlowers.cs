using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class PredefinedFlowers
{
    public static Flower noFlower = new Flower(new Gene[0][], new Fraction[0], FlowerType.NONE, FlowerColour.NONE);

    public static List<Flower> seedFlowers = new List<Flower>()
    {
        new Flower("2001", FlowerType.ROSE), //seed Red
        new Flower("0200", FlowerType.ROSE), //seed Yellow
        new Flower("0010", FlowerType.ROSE), //seed White
        new Flower("200", FlowerType.COSMOS), //seed Red
        new Flower("021", FlowerType.COSMOS), //seed Yellow
        new Flower("001", FlowerType.COSMOS), //seed White
        new Flower("201", FlowerType.LILY), //seed Red
        new Flower("020", FlowerType.LILY), //seed Yellow
        new Flower("002", FlowerType.LILY), //seed White
        new Flower("200", FlowerType.PANSY), //seed Red
        new Flower("020", FlowerType.PANSY), //seed Rellow
        new Flower("001", FlowerType.PANSY), //seed White
        new Flower("201", FlowerType.TULIP), //seed Red
        new Flower("020", FlowerType.TULIP), //seed Yellow
        new Flower("001", FlowerType.TULIP), //seed White
        new Flower("201", FlowerType.HYACINTH), //seed Red
        new Flower("020", FlowerType.HYACINTH), //seed Yellow
        new Flower("001", FlowerType.HYACINTH), //seed White
        new Flower("200", FlowerType.MUM), //seed Red
        new Flower("020", FlowerType.MUM), //seed Yellow
        new Flower("001", FlowerType.MUM), //seed White
        new Flower("200", FlowerType.WINDFLOWER), //seed Red
        new Flower("020", FlowerType.WINDFLOWER), //seed Orange
        new Flower("001", FlowerType.WINDFLOWER), //seed white
    };
}

