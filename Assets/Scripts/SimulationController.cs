using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulationController : MonoBehaviour
{
    public static SimulationController singleton;

    public FlowerController selectedFlower;

    public FlowerGhostController currentGhost;

    public bool guiOpen;

    private void Awake()
    {
        singleton = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        selectedFlower = null;
    }

    public void ClearSelection()
    {
        Debug.Log("THIS HAPPENS");
        selectedFlower = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentGhost != null)
        {
            return;
        }


    }
}
