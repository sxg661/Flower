using UnityEngine;
using System.Collections;
using System;
using System.Linq;

public struct Gene
{
    public bool allele1;

    public bool allele2;

    public Gene(bool dom1, bool dom2)
    {

        // if one gene is true (dominant) and the other is false (recessive), the dominant gene
        // should be at the front
        if(dom1 == false && dom2 == true)
        {
            dom1 = true;
            dom2 = false;
        }

        allele1 = dom1;
        allele2 = dom2;
    }

    public Gene(char ternary)
    {
        switch (ternary)
        {
            case '0':
                allele1 = false;
                allele2 = false;
                break;
            case '1':
                allele1 = true;
                allele2 = false;
                break;
            case '2':
                allele1 = true;
                allele2 = true;
                break;
            default:
                throw new Exception("Error : Invalid flower ternary gene code " + ternary + ", must be 0,1 or 2");
        }
    }

    public static Gene[] GetGeneArray(String geneCode)
    {
        Gene[] genes = new Gene[geneCode.Length];
        for(int i = 0; i < geneCode.Length; i++)
        {
            char geneChar = geneCode[i];
            genes[i] = new Gene(geneChar);
        }
        return genes;
    }

    public (Gene[], Fraction[]) BreedWith(Gene other)
    {
        if (allele1 == allele2 && other.allele1 == other.allele2)
        {
            return (new Gene[] { new Gene(allele1, other.allele1) }, 
              new Fraction[] { new Fraction(1,1) });
        }
        else if (allele1 == allele2)
        {
            return (new Gene[] { new Gene(allele1, true), new Gene(allele2, false) }, 
               new Fraction[] { new Fraction(1,2), new Fraction(1,2) });
        }
        else if (other.allele1 == other.allele2)
        {
            return (new Gene[] { new Gene(other.allele1, true), new Gene(other.allele1, false) }, 
                new Fraction[] {new Fraction(1, 2), new Fraction(1, 2) });
        }
        else
        {
            return (new Gene[] { new Gene(true, false), new Gene(true, true), new Gene(false, false) }, 
                new Fraction[] {new Fraction(1, 2), new Fraction(1, 4), new Fraction(1, 4) });
        }
    }

    public override bool Equals(System.Object other)
    {
        Gene otherGene = (Gene) other;
        return otherGene.allele1 == allele1 && otherGene.allele2 == allele2;
    }

    public override int GetHashCode()
    {
        return ToString().GetHashCode();
    }

    public override string ToString()
    {
        return (Convert.ToInt32(allele1) + Convert.ToInt32(allele2)).ToString();
    }

    public static string GetString(Gene[] genes)
    {
        string str = "";
        foreach(Gene gene in genes){
            str = str + gene;
        }
        return str;
    }

    public static string FormatForGUI(Gene[] genes, FlowerType type)
    {
        char[] dominantGeneCodes;
        char[] recessiveGeneCodes;

        if (type == FlowerType.COSMOS || type == FlowerType.LILY || type == FlowerType.TULIP)
        {
            dominantGeneCodes = new char[] { 'R', 'Y', 'S' };
            recessiveGeneCodes = new char[] { 'r', 'y', 's' };
        }
        else if (type == FlowerType.WINDFLOWER)
        {
            dominantGeneCodes = new char[] { 'R', 'O', 'w' };
            recessiveGeneCodes = new char[] { 'r', 'o', 'W' };
        }
        else
        {
            //IMPORTANT
            //For some reason in the flower CSVs the W gene is encoded in reverse with 0 meaning two dominant!
            //Switching them in the arrays is not strictly necessary, as the special case in considered the switch statement below, 
            // However, they are still flipped here for readability purposes.
            dominantGeneCodes = new char[] { 'R', 'Y', 'w', 'S' };
            recessiveGeneCodes = new char[] { 'r', 'y', 'W', 's' };
        }


        string guiStr = "";
        for (int i = 0; i < genes.Length; i++)
        {
            Gene gene = genes[i];
            if (i > dominantGeneCodes.Length)
            {
                continue;
            }

            char dominantCode = dominantGeneCodes[i];
            char recessiveCode = recessiveGeneCodes[i];

            switch (gene.ToString())
            {
                case "0":
                    guiStr += " " + recessiveCode + recessiveCode;
                    break;
                case "1":
                    //Captial Letter goes first, W gene is funny, see note labled IMPORTANT above.
                    if(dominantCode == 'w')
                    {
                        guiStr += " " + recessiveCode + dominantCode;
                    }
                    else
                    {
                        guiStr += " " + dominantCode + recessiveCode;
                    }
                    break;
                case "2":
                    guiStr += " " + dominantCode + dominantCode;
                    break;
            }
        }

        if (guiStr.Length < 1)
        {
            return "";
        }

        //substring to remove space at start
        return guiStr.Substring(1, guiStr.Length - 1);
    }

}
