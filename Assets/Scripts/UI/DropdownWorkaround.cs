﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropdownWorkaround : MonoBehaviour
{
    public string _SortingLayerName = "Default";

    void Awake()
    {
        Canvas canvas = GetComponent<Canvas>();
        if (canvas != null)
            canvas.sortingLayerName = _SortingLayerName;
    }
}
