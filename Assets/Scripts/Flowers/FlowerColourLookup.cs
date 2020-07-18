using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Linq;

public class FlowerColourLookup : MonoBehaviour
{

    public static readonly string FLOWER_FOLDER = "FlowerData";
    public static FlowerColourLookup lookup;


    public Dictionary<FlowerType, Dictionary<string,FlowerColour>> colourLookup = new Dictionary<FlowerType, Dictionary<string, FlowerColour>>();
    public Dictionary<FlowerType, Dictionary<FlowerColour, List<String>>> geneLookup = new Dictionary<FlowerType, Dictionary<FlowerColour, List<String>>>();

    private void Awake()
    {
        InitialiseLookups();
    }

    /// <summary>
    /// Gets a generuc flower of a given type and colour.
    /// All possible genetics codes have equal probablity to start with. 
    /// </summary>
    /// <param name="type"></param>
    /// <param name="colour"></param>
    /// <returns>Flower object of colour and type</returns>
    public Flower GetFlowerWithColour(FlowerType type, FlowerColour colour)
    {
        if(type is FlowerType.NONE || colour is FlowerColour.NONE)
        {
            return PredefinedFlowers.noFlower;
        }

        string[] possibleGenes = geneLookup[type][colour].ToArray();

        Fraction[] probabilities = new Fraction[possibleGenes.Length];
        probabilities = probabilities.Select(x => new Fraction(1,possibleGenes.Length)).ToArray();

        return new Flower(possibleGenes, probabilities, type,colour);
    }


    /// <summary>
    /// Reads the CSV files found in the FlowerData folder within the base lookup folder. 
    /// Loads them into dictionaries to allow searches:
    ///     - 1: for possible genes based on colours and flower type
    ///     - 2: for the expressed colour based on genes and flower type
    /// </summary>
    private void InitialiseLookups()
    {
        lookup = this;

        foreach(FlowerType flowerType in Enum.GetValues(typeof(FlowerType)))
        {
            if(flowerType.ToString() == "NONE")
            {
                continue;
            }

            string csvName = Path.Combine(FLOWER_FOLDER, flowerType.ToString() + ".csv");

            var flowerColLookup = new Dictionary<string, FlowerColour>();
            var flowerGeneLookup = new Dictionary<FlowerColour, List<string>>();

            try
            {
                using (StreamReader reader = new StreamReader(File.OpenRead(csvName)))
                {
                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        string[] values = line.Split(',');

                        string[] genes = values.Take(values.Length - 1).ToArray();
                        string geneID = String.Join("", genes);

                        FlowerColour colour = (FlowerColour) Enum.Parse(typeof(FlowerColour), values[values.Length - 1]);

                        flowerColLookup[geneID] = colour;
          
                        if (!flowerGeneLookup.ContainsKey(colour))
                        {
                            flowerGeneLookup[colour] = new List<string>();
                        }
                        flowerGeneLookup[colour].Add(geneID);
                    }
                }
                colourLookup[flowerType] = flowerColLookup;
                geneLookup[flowerType] = flowerGeneLookup;
            }
            catch(Exception e)
            {
                Debug.Log("ERROR " + csvName + " " + e.ToString());
            }
            
         
        }
    }
}
