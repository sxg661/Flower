using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanScript : MonoBehaviour
{
    float DRAGSPEED = 0.02f;
    float ARROWSPEED = 0.6f;

    Vector3 lastMousePos;

    bool mouseDown = false;

    (float, float) xRange;
    (float, float) yRange;

    private void Start()
    {
        xRange = (0, FlowerGrid.flowerGrid.gridWidth);
        yRange = (0, FlowerGrid.flowerGrid.gridHeight);
        Vector3 gridCentre = GridRenderer.GetWorldPos(0, 0, FlowerGrid.flowerGrid.gridWidth, FlowerGrid.flowerGrid.gridHeight);
        transform.position = new Vector3(gridCentre.x, gridCentre.y, transform.position.z);
    }


    // Update is called once per frame
    void Update()
    {

        Vector3 movement = new Vector3(UnityEngine.Input.GetAxis("Horizontal"), UnityEngine.Input.GetAxis("Vertical"), 0) * ARROWSPEED;

        if (UnityEngine.Input.GetMouseButtonDown(0) && !ClickDetector.clickDetector.overGUI)
        {
             mouseDown = true;
             lastMousePos = UnityEngine.Input.mousePosition;

        }


        if (mouseDown)
        {
            Vector3 thisMousePos = UnityEngine.Input.mousePosition;
            float horizont = thisMousePos.x - lastMousePos.x;
            float vert = thisMousePos.y - lastMousePos.y;
            movement += new Vector3(-horizont, -vert, movement.z) * DRAGSPEED;

            lastMousePos = thisMousePos;

            if(SimulationController.singleton.selectedFlower != null && SimulationController.singleton.selectedFlower.beingDragged)
            {
                mouseDown = false;
            }

            if (UnityEngine.Input.GetMouseButtonUp(0))
            {
                mouseDown = false;
            }
        }

        Vector3 newPos = transform.position + (movement);

        float x = Mathf.Clamp(newPos.x, xRange.Item1, xRange.Item2);
        float y = Mathf.Clamp(newPos.y, yRange.Item1, yRange.Item2);

        transform.position = new Vector3(x, y, newPos.z);

    }
}
