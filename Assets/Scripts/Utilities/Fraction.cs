using UnityEngine;
using System.Collections;
using System;

public struct Fraction
{

    public long numerator;
    public long denominator;
    
    public Fraction(long numer, long denom)
    {
        numerator = numer;
        denominator = denom;
        Simplify();
    }

    public Fraction(double dec)
    {
        double remain = dec % 1;

        long power10 = 0;
        if (remain != 0)
        {
            power10 = dec.ToString().Substring(2).Length;
        }



        double den = Mathf.Pow(10f, power10);
        double num = (dec * den);

        denominator = (long)den;
        numerator = (long)num;

        Simplify();

    }

    public bool IsSame(Fraction other)
    {
        Fraction difference = this - other;
        return (difference.numerator == 0);
    }

    public float GetDecimal()
    {
        if (numerator == 0)
        {
            return 0;
        }
        double num = numerator;
        double denom = denominator;
        return (float) (num / denom);
    }


    public override string ToString()
    {
        return string.Format("{0}/{1}", numerator, denominator);
    }

    public void Simplify()
    {
        if(numerator == 0)
        {
            denominator = 0;
            return;
        }

        long hcf = HCF(numerator, denominator);
        numerator = numerator / hcf;
        denominator = denominator / hcf;
    }

    private static long HCF(long a, long b)
    {

        while (a > 0 && b > 0)
        {
            if (a > b)
                a %= b;
            else
                b %= a;
        }

        return a == 0 ? b : a;
    }


    public static Fraction Simplify(Fraction f)
    {
        if(f.numerator == 0)
        {
            return new Fraction(0, 0);
        }

        //use Euclidean algorithm to get highest common factor
        long num = f.numerator;
        long den = f.denominator;
        long hcf = HCF(num, den);

        return new Fraction(f.numerator / hcf, f.denominator / hcf);
    }

    public static Fraction Reciprocal(Fraction f)
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

        long num = (a.numerator * b.denominator) + (b.numerator * a.denominator);
        long den = (b.denominator * a.denominator);
        return Simplify(new Fraction(num, den));
    }

    public static Fraction operator -(Fraction a, Fraction b)
    {
        if (a.numerator == 0)
        {
            return Simplify(new Fraction(-b.numerator, b.denominator));
        }

        if (b.numerator == 0)
        {
            return Simplify(a);
        }

        long num = (a.numerator * b.denominator) - (b.numerator * a.denominator);
        long den = (b.denominator * a.denominator);
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
        if(arr.Length == 0)
        {
            return new Fraction[] {};
        }
        Fraction sum = Sum(arr);
        Fraction[] newArr = new Fraction[arr.Length];
        for(int i = 0; i < arr.Length; i++)
        {
            newArr[i] = Simplify(arr[i] * Reciprocal(sum));
        }
        return newArr;
    }

    public void RoundToDenom(long newDenom)
    {
        double num = numerator;
        double denom = denominator;

        double mult = denom / newDenom;
        num = num/mult;
        numerator = (long) Math.Round(num);
        denominator = newDenom;

        Simplify();
    }

    private static long GetLargestDenom(Fraction[] arr)
    {
        long largest = 0;
        foreach(Fraction frac in arr)
        {
            largest = Math.Max(largest, frac.denominator);
        }
        return largest;
    }

    public static Fraction[] Round(Fraction[] arr)
    {
        Fraction[] newArr = new Fraction[arr.Length];
        Array.Copy(arr, newArr, arr.Length);

        int roundTo = 100;

        if(GetLargestDenom(arr) < roundTo)
        {
            return newArr;
        }

        foreach(Fraction frac in newArr)
        {
            frac.RoundToDenom(roundTo);
        }

        Fraction sum = Sum(newArr);

        return newArr;
    }
}
