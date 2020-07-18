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

    public override string ToString()
    {
        return (Convert.ToInt32(allele1) + Convert.ToInt32(allele2)).ToString();
    }

    public static string getString(Gene[] genes)
    {
        string str = "";
        foreach(Gene gene in genes){
            str = str + gene;
        }
        return str;
    }
}
