using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class Flower
{
    public FlowerType type;
    public FlowerColour colour;

    public Gene[][] genesPoss;
    public Fraction[] genesProbs;

    public Flower(Gene[][] genesPoss, Fraction[] genesProbs, FlowerType type, FlowerColour colour)
    {
        this.genesPoss = genesPoss;
        this.genesProbs = genesProbs;
        this.type = type;
        this.colour = colour;
    }

    public Flower(string[] geneCodes, Fraction[] genesProbs, FlowerType type, FlowerColour colour)
    {
        //convert the string gene codes into arrays of Gene objects e.g. 210 to [Gene(TT), Gene(TF) , Gene(FF)].
        genesPoss = geneCodes.Select(x => x.Select(g => new Gene(g)).ToArray()).ToArray();

        this.genesProbs = genesProbs;
        this.type = type;
        this.colour = colour;
    }


    public void Validate()
    {
        if(genesPoss.Length != genesProbs.Length)
        {
            type = FlowerType.NONE;
        }

        var newGenesPoss = new List<Fraction>();
        var newGenesProbs = new List<Fraction>();
        for (int i = 0; i < genesPoss.Length; i++)
        {
            
        }
    }

    public override string ToString()
    {
        string str = String.Format("Type: {0},  Colour: {1}  ", type, colour);
        for (int i = 0; i < genesPoss.Length; i++)
        {
            str = String.Format("{0}; {1}). {2} with {3} liklihood  ", str, i, Gene.getString(genesPoss[i]), genesProbs[i]);
        }
        return str;
    }

    /// <summary>d.
    /// List all the genes the parents would have to have to create a possible offspring.
    /// </summary>
    /// <param name="other"></param>
    /// <param name="colour"></param>
    /// <returns>Parent Genes 1, Parent Genes 2, Probablity of Combination, Offpsring Flower</returns>
    public List<(Gene[], Gene[], Fraction, Flower)> BreedAndShowParentGenes(Flower other)
    {
        if (type != other.type)
        {
            return new List<(Gene[], Gene[], Fraction, Flower)>();
        }

        var offpsring = new List<(Gene[], Gene[], Fraction, Flower)>();
        for(int i = 0; i < genesPoss.Count(); i++)
        {
            for (int j = 0; j < other.genesPoss.Count(); j++)
            {

                (Gene[][] genes, Fraction[] liklihoods) = BreedGeneLists(genesPoss[i], other.genesPoss[j]);
                Flower child = new Flower(genes, liklihoods, type, FlowerColour.NONE);
                offpsring.Add((genesPoss[i], other.genesPoss[j], genesProbs[i] * other.genesProbs[j], child));

            }
        }

        return offpsring;
    }


        /// <summary>
        /// Get the offspring of breeding this flower with another flower.
        /// Offspring will have all possible genetic codes with the probabilities of getting them.
        /// </summary>
        /// <param name="other">The flower to breed with</param>
        /// <returns></returns>
        public Flower BreedWith(Flower other)
        {
            if (type != other.type)
            {
                return PredefinedFlowers.noFlower;
            }

            Dictionary<string, (Gene[], Fraction)> allPoss = new Dictionary<string, (Gene[], Fraction)>();

            for (int i = 0; i < genesPoss.Length; i++)
            {
                Gene[] genelist1 = genesPoss[i];
                Fraction prob1 = genesProbs[i];

                for (int j = 0; j < other.genesPoss.Length; j++)
                {
                    Gene[] genelist2 = other.genesPoss[j];
                    Fraction prob2 = other.genesProbs[j];

                    (Gene[][] possChildren, Fraction[] probsChildren) = BreedGeneLists(genelist1, genelist2);

                    for(int k = 0; k < possChildren.Length; k++)
                    {
                        string key = Gene.getString(possChildren[k]);
                        if (!allPoss.ContainsKey(key))
                        {
                            allPoss[key] = (possChildren[k], new Fraction(0,0));
                        }
                        allPoss[key] = (allPoss[key].Item1, allPoss[key].Item2 + (probsChildren[k] * prob1 * prob2));
                    }
                }

            }

            Gene[][] flowerGenePoss = allPoss.Values.Select(i => i.Item1).ToArray();
            Fraction[] probs = allPoss.Values.Select(i => i.Item2).ToArray();

            return new Flower(flowerGenePoss, probs, type, FlowerColour.NONE);

        }

    /// <summary>
    /// Breed two lists of genes together to get all the possible gene lists of their offspring.
    /// </summary>
    /// <param name="genelist1"></param>
    /// <param name="genelist2"></param>
    /// <returns></returns>
    private static (Gene[][], Fraction[]) BreedGeneLists(Gene[] genelist1, Gene[] genelist2)
    {

        (Gene[] firstGenePoss, Fraction[] firstGeneProbs) = genelist1[0].BreedWith(genelist2[0]);

        if (genelist1.Length == 1)
        {
            return (firstGenePoss.Select(gene => new Gene[] { gene }).ToArray(), firstGeneProbs);
        }

        (Gene[][] tailGenePoss, Fraction[] tailGeneProbs) = BreedGeneLists(genelist1.Skip(1).ToArray(), genelist2.Skip(1).ToArray());

        Gene[][] childPoss = new Gene[tailGenePoss.Length * firstGenePoss.Length][];
        Fraction[] childProb = new Fraction[tailGenePoss.Length * firstGenePoss.Length];
        int arrayInd = 0;

        for (int i = 0; i < firstGenePoss.Length; i++)
        {
            for (int j = 0; j < tailGenePoss.Length; j++)
            {
                Gene[] genePoss = ArrayUtils.Prepend(firstGenePoss[i], tailGenePoss[j]);

                childPoss[arrayInd] = genePoss;
                childProb[arrayInd++] = firstGeneProbs[i] * tailGeneProbs[j];

            }
        }
        return (childPoss, childProb);
    
    }




}
