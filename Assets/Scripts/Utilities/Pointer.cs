using UnityEngine;
using System.Collections;

public class Pointer<T>
{
    public T value;

    public Pointer(T val)
    {
        value = val;
    }
}
