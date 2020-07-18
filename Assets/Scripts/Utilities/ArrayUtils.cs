using UnityEngine;
using System.Collections;
using System.Linq;
using System;

/// <summary>
/// Utility functions for dealing with arrays of any contents type.
/// </summary>
public class ArrayUtils 
{

    /// <summary>
    /// Prepends an item to an array. 
    /// Does not alter the original array.
    /// </summary>
    /// <typeparam name="T">Contents type</typeparam>
    /// <param name="elem">Element to add</param>
    /// <param name="array">Array to add to</param>
    /// <returns>Array with prepended element</returns>
    public static T[] Prepend<T>(T elem, T[] array)
    {
        T[] newArray = new T[array.Length + 1];
        Array.Copy(array, 0, newArray, 1, array.Length);
        newArray[0] = elem;
        return newArray;
    }

    /// <summary>
    /// Concatenates two arrays.
    /// Does not alter the original arrays.
    /// </summary>
    /// <typeparam name="T">Contents type</typeparam>
    /// <param name="array1">The array to come first in the new concatenated array.</param>
    /// <param name="array2">The array to come last in the new concatenated array.</param>
    /// <returns></returns>
    public static T[] Concatenate<T>(T[] array1, T[] array2)
    {
        T[] newArray = new T[array1.Length + array2.Length];
        Array.Copy(array1, 0, newArray, 0, array1.Length);
        Array.Copy(array2, 0, newArray, array1.Length, array2.Length);
        return newArray;
    }

    /// <summary>
    /// Checks that the contents of the array is equal.
    /// Uses the equals operator of the cotents type.
    /// </summary>
    /// <typeparam name="T">Contents type: must be class that inherits from System.Object</typeparam>
    /// <param name="array1"></param>
    /// <param name="array2"></param>
    /// <returns></returns>
    public static bool ContentsEqual<T> (T[] array1, T[] array2) where T : class
    {
        if (array1.Length != array2.Length)
        {
            return false;
        }

        for(int i = 0; i < array1.Length; i++)
        {
            if (array1[i] != array2[i])
            {
                return false;
            }
        }

        return true;
    }
}
