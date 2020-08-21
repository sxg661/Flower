﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlowerInfoController : MonoBehaviour
{

    [SerializeField]
    Text titleText;

    [SerializeField]
    Image flowerImage;

    [SerializeField]
    Transform scrollMenuContents;

    [SerializeField]
    GameObject genePanelPrefab;

    FlowerController currentController;

    List<GameObject> genePanels;


    private void Awake()
    {
        genePanels = new List<GameObject>();
    }


    // Start is called before the first frame update
    void Start()
    {
        titleText.text = "No Flower Selected";
        flowerImage.enabled = false;
        currentController = null;
    }

    string FormatCases(string str)
    {
        string strFirst = str.Substring(0, 1).ToUpper();
        string strSecond = str.Substring(1, str.Length - 1).ToLower();
        return strFirst + strSecond;
    }

    // Update is called once per frame
    void Update()
    {
        FlowerController controller = SimulationController.singleton.SelectedFlower;

        if (controller == currentController)
        {
            return;
        }

        ClearGenesList();

        if (controller == null)
        {
            titleText.text = "No Flower Selected";
            flowerImage.enabled = false;
            currentController = null;
            return;
        }

        flowerImage.enabled = true;

        currentController = controller;

        string title = string.Format("{0} {1}", FormatCases(controller.flower.colour.ToString()), FormatCases(controller.flower.type.ToString()));
        titleText.text = title;

        Sprite imageSprite = controller.spriteRenderer.sprite;
        flowerImage.sprite = imageSprite;

        RenderGeneList(controller);
    }

    void ClearGenesList()
    {
        foreach(GameObject obj in genePanels)
        {
            Destroy(obj);
        }
        genePanels = new List<GameObject>();
    }

    void RenderGeneList(FlowerController controller)
    {
        for(int i = 0; i < controller.flower.genesPoss.Length; i++)
        {
            GameObject genePanel = Instantiate(genePanelPrefab, scrollMenuContents);
            GenePanelController panelController = genePanel.GetComponent<GenePanelController>();
            panelController.GiveDetails(controller.flower.genesPoss[i], controller.flower.genesProbs[i], controller.flower.type);
            genePanels.Add(genePanel);
        }
    }
}