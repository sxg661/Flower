using UnityEngine;
using System.Collections;

public struct Fraction
{

    int numerator;
    int denominator;
    
    public Fraction(int numer, int denom)
    {
        numerator = numer;
        denominator = denom;
    }

    public override string ToString()
    {
        return string.Format("{0}/{1}", numerator, denominator);
    }

    public static Fraction Simplify(Fraction f)
    {
        //use Euclidean algorithm to get highest common factor
        int num = f.numerator;
        int den = f.denominator;
        while (num > 0 && den > 0)
        {
            if (num > den)
                num %= den;
            else
                den %= num;
        }

        int hcf = num == 0 ? den : num;

        return new Fraction(f.numerator / hcf, f.denominator / hcf);
    }

    public static Fraction Recipocal(Fraction f)
    {
        return new Fraction(f.denominator, f.numerator);
    }

    public static Fraction operator *(Fraction a, Fraction b)
    {
        return Simplify(new Fraction(a.numerator * b.numerator, a.denominator * b.denominator));
    }

    public static Fraction operator +(Fraction a, Fraction b)
    {
        if(a.numerator == 0)
        {
            return Simplify(b);
        }

        if (b.numerator == 0)
        {
            return Simplify(a);
        }

        int num = (a.numerator * b.denominator) + (b.numerator * a.denominator);
        int den = (b.denominator * a.denominator);
        return Simplify(new Fraction(num, den));
    }

    public static Fraction Sum(Fraction[] arr)
    {
        Fraction total = new Fraction(0,0);
        foreach(Fraction fraction in arr)
        {
            total += fraction;
        }
        return total;
    }

    public static Fraction[] Normalise(Fraction[] arr)
    {
        Fraction sum = Sum(arr);
        Fraction[] newArr = new Fraction[arr.Length];
        for(int i = 0; i < arr.Length; i++)
        {
            newArr[i] = Simplify(arr[i] * Recipocal(sum));
        }
        return newArr;
    }
}
