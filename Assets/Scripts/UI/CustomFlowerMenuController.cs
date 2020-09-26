﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomFlowerMenuController : MonoBehaviour
{
    FlowerType myType;
    FlowerColour myColour;
    Flower myFlower;

    [SerializeField]
    GameObject parentObject;

    [SerializeField]
    Text titleText;

    [SerializeField]
    GameObject scrollMenuObject;

    [SerializeField]
    GameObject scrollContentsObj;

    [SerializeField]
    GameObject geneUI;

    [SerializeField]
    Text doneButtonText;
    [SerializeField]
    Button doneButton;

    [SerializeField]
    Button backButton;

    [SerializeField]
    GameObject flowerPanelPrefab;

    List<Action> openPageActions;
    int currentPage;

    int scrollMenuWidth;
    List<GameObject> scrollMenuContents;

    private void Awake()
    {
        //TEMPORARY------------------------------

        myType = FlowerType.COSMOS;
        myColour = FlowerColour.YELLOW;
        //---------------------------------------
    }

    public void Open()
    {
        if(scrollMenuContents != null)
        {
            ClearScrollMenu();
        }
        scrollMenuContents = new List<GameObject>();

        //FIND ALTERNATIVE TO HARD CODING THIS VALUE
        scrollMenuWidth = 282;

        if (openPageActions == null)
        {
            openPageActions = new List<Action> { ChooseType, ChooseColour, CustomiseGenes };
        }

        currentPage = 0;
        myFlower = null;
        doneButtonText.text = "Next";

        backButton.interactable = false;

        openPageActions[0]();

        parentObject.SetActive(true);

        SimulationController.singleton.guiOpen = true;
        SimulationController.singleton.currentGhost = null;
        SimulationController.singleton.selectedFlower = null;
    }

    public void Close()
    {
        parentObject.SetActive(false);

        SimulationController.singleton.guiOpen = false;
    }

    public void NextPage()
    {
        int newPage = currentPage + 1;

        if(newPage >= openPageActions.Count)
        {
            CreateFlower();
            Close();
        }
        else
        {
            backButton.interactable = true;

            if (newPage == openPageActions.Count - 1)
            {
                doneButtonText.text = "Done";
            }

            currentPage = newPage;
            openPageActions[currentPage]();

        }
    }

    public void PreviousPage()
    {
        int newPage = currentPage - 1;
        
        if(newPage < 0)
        {
            throw new Exception("Cannot go to page at index " + newPage);
        }

        if(newPage == 0)
        {
            backButton.interactable = false;
        }

        doneButtonText.text = "Next";

        currentPage = newPage;
        openPageActions[currentPage]();
    }



    private void CreateFlower()
    {

    }

    private void ClearScrollMenu()
    {
        foreach(GameObject obj in scrollMenuContents)
        {
            Destroy(obj);
        }
        scrollMenuContents = new List<GameObject>();
    }

    private void ChooseType()
    {
        ClearScrollMenu();

        scrollMenuObject.SetActive(true);
        geneUI.SetActive(false);

        var types = Enum.GetValues(typeof(FlowerType));

        foreach (FlowerType type in types)
        {
            if (type == FlowerType.NONE)
            {
                continue;
            }
 
            Flower flower = FlowerColourLookup.lookup.GetFlowerWithColour(type, FlowerColour.WHITE);
            GameObject panel = Instantiate(flowerPanelPrefab, scrollContentsObj.transform);
            FlowerPanelController controller = panel.GetComponent<FlowerPanelController>();
            controller.GiveInfo(scrollMenuWidth, flower, Flower.FormatCases(type.ToString()), null);
            scrollMenuContents.Add(panel);
        }

        titleText.text = "Choose Flower Type";
    }

    private void ChooseColour()
    {
        ClearScrollMenu();

        scrollMenuObject.SetActive(true);
        geneUI.SetActive(false);

        titleText.text = string.Format("Choose {0} Colour", Flower.FormatCases(myType.ToString()));
    }

    private void CustomiseGenes()
    {
        scrollMenuObject.SetActive(false);
        geneUI.SetActive(true);

        myFlower = FlowerColourLookup.lookup.GetFlowerWithColour(myType, myColour);

        titleText.text = myFlower.GetName();
    }



}
