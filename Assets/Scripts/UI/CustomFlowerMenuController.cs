using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    GameObject scrollMenuPanel;

    [SerializeField]
    GameObject scrollContentsObj;

    [SerializeField]
    GameObject geneScrollMenuCotents;

    [SerializeField]
    GameObject geneUI;

    [SerializeField]
    Image geneUIImage;

    [SerializeField]
    Text doneButtonText;

    [SerializeField]
    Button doneButton;

    [SerializeField]
    Button backButton;

    [SerializeField]
    GameObject flowerPanelPrefab;

    [SerializeField]
    GameObject genePanelPrefab;

    [SerializeField]
    Text errorText;

    [SerializeField]
    GameObject ghostFlowerPrefab;

    List<Action> openPageActions;
    int currentPage;

    int scrollMenuWidth;
    List<GameObject> scrollMenuContents;

    Button selectedButton;

    private void Awake()
    {
    }

    public void Open()
    {
        if(scrollMenuContents != null)
        {
            ClearScrollMenu();
        }
        scrollMenuContents = new List<GameObject>();

        //FIND ALTERNATIVE TO HARD CODING THIS VALUE
        scrollMenuWidth = 382;

        if (openPageActions == null)
        {
            openPageActions = new List<Action> { ChooseType, ChooseColour, CustomiseGenes };
        }

        currentPage = 0;
        myFlower = null;
        doneButton.gameObject.SetActive(false);

        backButton.interactable = false;

        openPageActions[0]();

        parentObject.SetActive(true);

        SimulationController.singleton.guiOpen = true;
        SimulationController.singleton.currentGhost = null;
        SimulationController.singleton.selectedFlower = null;

        //no error text to start with
        errorText.text = "";
    }

    public void Close()
    {
        parentObject.SetActive(false);

        SimulationController.singleton.guiOpen = false;
    }

    /// <summary>
    /// Go to the next page if there is one, or if at end of menu create flower and close menu
    /// </summary>
    public void NextPage()
    {
        int newPage = currentPage + 1;

        //if on last page, finialise flower and exit menu
        if(newPage >= openPageActions.Count)
        {
            //loop through scroll items and get the sum of their probabilities and whether or not there are any errors
            Fraction total = new Fraction(0, 0);
            bool noErrors = true;
            foreach(GameObject item in scrollMenuContents)
            {
                GenePanelInteractableController genePanelCntrl = item.GetComponent<GenePanelInteractableController>();
                total += genePanelCntrl.GetProb();
                noErrors = noErrors && !genePanelCntrl.HasError();
            }

            //if no errors
            if(total.GetDecimal() == 1 && noErrors)
            {
                CreateFlower();
                Close();
            }
            //if a format error
            else if(!noErrors)
            {
                //display error text
                errorText.text = "Incorrect input format - must be fraction in format '<d>/<n>'";
            }
            //if liklihoods do not sum to the correct number
            else if(total.GetDecimal() != 1)
            {
                errorText.text = "Liklihoods must add to one";
            }
        }
        //if not on last page, move to the next page
        else
        {
            backButton.interactable = true;

            if (newPage == openPageActions.Count - 1)
            {
                doneButton.gameObject.SetActive(true);
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

        doneButton.gameObject.SetActive(false);

        currentPage = newPage;
        openPageActions[currentPage]();
    }


    /// <summary>
    /// Create a flower based on the inputs entered by the user on each screen of the menu
    /// </summary>
    private void CreateFlower()
    {
        //from the final screen, work out the gene poss and probs
        Gene[][] genePoss = new Gene[scrollMenuContents.Count()][];
        Fraction[] geneProbs = new Fraction[scrollMenuContents.Count()];
        for(int i = 0; i < scrollMenuContents.Count; i++)
        {
            //get the panel controller
            GameObject genePanel = scrollMenuContents[i];
            GenePanelInteractableController genePanelCntrl = genePanel.GetComponent<GenePanelInteractableController>();

            //add the possibilties and liklihoods to the list
            genePoss[i] = genePanelCntrl.GetGenes();
            geneProbs[i] = genePanelCntrl.GetProb();
        }

        //now create the flower
        myFlower = new Flower(genePoss, geneProbs, myType, myColour);

        //render the flower as a ghost
        GameObject obj = Instantiate(ghostFlowerPrefab, Vector3.zero, Quaternion.identity);
        obj.GetComponent<FlowerGhostController>().flower = myFlower;
    }

    private void ClearScrollMenu()
    {
        foreach(GameObject obj in scrollMenuContents)
        {
            Destroy(obj);
        }
        scrollMenuContents = new List<GameObject>();
    }


    /// <summary>
    /// Called when button is clicked on
    /// </summary>
    /// <param name="controller"></param>
    private void HandleButtonClick(FlowerPanelController controller)
    {
        myType = controller.flower.type;
        myColour = controller.flower.colour;

        NextPage();
    }

    private void ChooseType()
    {
        ClearScrollMenu();

        scrollMenuPanel.SetActive(true);
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
            Button button = panel.GetComponent<Button>();

            controller.GiveInfo(
                scrollMenuWidth, 
                flower, 
                Flower.FormatCases(type.ToString()), 
                () => HandleButtonClick(controller));
            

            scrollMenuContents.Add(panel);
        }

        titleText.text = "Choose Flower Type";
    }

    private void ChooseColour()
    {
        ClearScrollMenu();

        List<FlowerColour> colours = FlowerColourLookup.lookup.geneLookup[myType].Keys.ToList();
        foreach(FlowerColour flowerCol in colours)
        {
            Flower flower = FlowerColourLookup.lookup.GetFlowerWithColour(myType, flowerCol);
            GameObject panel = Instantiate(flowerPanelPrefab, scrollContentsObj.transform);
            FlowerPanelController controller = panel.GetComponent<FlowerPanelController>();
            Button button = panel.GetComponent<Button>();

            controller.GiveInfo(
                    scrollMenuWidth,
                    flower,
                    Flower.FormatCases(flower.GetName()),
                    () => HandleButtonClick(controller));
            
            scrollMenuContents.Add(panel);

        }

        scrollMenuPanel.SetActive(true);
        geneUI.SetActive(false);

        titleText.text = "Choose Colour";
    }

    private void CustomiseGenes()
    {
        scrollMenuPanel.SetActive(false);
        geneUI.SetActive(true);

        ClearScrollMenu();

        myFlower = FlowerColourLookup.lookup.GetFlowerWithColour(myType, myColour);

        Sprite sprite = Resources.Load<Sprite>(myFlower.GetFilePath());
        geneUIImage.sprite = sprite;

        titleText.text = myFlower.GetName();

        for(int i = 0; i < myFlower.genesPoss.Length; i++)
        {
            GameObject genePanel = Instantiate(genePanelPrefab, geneScrollMenuCotents.transform);
            var controller = genePanel.GetComponent<GenePanelInteractableController>();

            controller.SetGeneCode(myFlower.genesPoss[i], myFlower.type);
            controller.SetProb(myFlower.genesProbs[i]);
            controller.SetWidth(scrollMenuWidth - 10);

            scrollMenuContents.Add(genePanel);
        }
    }



}
