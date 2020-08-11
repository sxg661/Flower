using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulationController : MonoBehaviour
{
    public static SimulationController singleton;

    public FlowerController SelectedFlower;

    private void Awake()
    {
        singleton = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        SelectedFlower = null;
    }

    public void ClearSelection()
    {
        SelectedFlower = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SelectedFlower = null;
        }
    }
}
