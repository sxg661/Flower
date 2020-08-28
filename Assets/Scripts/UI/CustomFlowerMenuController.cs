using System;
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
    GameObject scrollUI;

    [SerializeField]
    GameObject geneUI;

    [SerializeField]
    Text doneButtonText;
    [SerializeField]
    Button doneButton;

    [SerializeField]
    Button backButton;

    List<Action> openPageActions;
    int currentPage;

    private void Awake()
    {
        //TEMPORARY------------------------------

        myType = FlowerType.COSMOS;
        myColour = FlowerColour.YELLOW;
        //---------------------------------------
    }

    public void Open()
    {
        if(openPageActions == null)
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

    private void ChooseType()
    {
        scrollUI.SetActive(true);
        geneUI.SetActive(false);

        titleText.text = "Choose Flower Type";
    }

    private void ChooseColour()
    {
        scrollUI.SetActive(true);
        geneUI.SetActive(false);

        titleText.text = string.Format("Choose {0} Colour", Flower.FormatCases(myType.ToString()));
    }

    private void CustomiseGenes()
    {
        scrollUI.SetActive(false);
        geneUI.SetActive(true);

        myFlower = FlowerColourLookup.lookup.GetFlowerWithColour(myType, myColour);

        titleText.text = myFlower.GetName();
    }



}
