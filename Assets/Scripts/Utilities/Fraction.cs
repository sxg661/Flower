using UnityEngine;
using System.Collections;
using System;

public struct Fraction
{

    public int numerator;
    public int denominator;
    
    public Fraction(int numer, int denom)
    {
        numerator = numer;
        denominator = denom;
        Simplify();
    }

    public Fraction(float dec)
    {
        float remain = dec % 1;

        int power10 = 0;
        if (remain != 0)
        {
            power10 = dec.ToString().Substring(2).Length;
        }



        float den = Mathf.Pow(10f, power10);
        float num = (dec * den);

        denominator = (int)den;
        numerator = (int)num;

        Simplify();

    }

    public bool IsSame(Fraction other)
    {
        Fraction difference = this - other;
        return (difference.numerator == 0);
    }

    public float GetDecimal()
    {
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
        int hcf = HCF(numerator, denominator);
        numerator = numerator / hcf;
        denominator = denominator / hcf;
    }

    private static int HCF(int a, int b)
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
        int num = f.numerator;
        int den = f.denominator;
        int hcf = HCF(num, den);

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

        int num = (a.numerator * b.denominator) + (b.numerator * a.denominator);
        int den = (b.denominator * a.denominator);
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

        int num = (a.numerator * b.denominator) - (b.numerator * a.denominator);
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
            newArr[i] = Simplify(arr[i] * Reciprocal(sum));
        }
        return newArr;
    }
}
