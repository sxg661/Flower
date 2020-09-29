using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class GenePanelInteractableController : MonoBehaviour
{
    Gene[] geneCode;

    [SerializeField]
    Text geneCodeText;

    [SerializeField]
    Color normalColor;

    [SerializeField]
    Color errorColour;

    [SerializeField]
    InputField textField;

    [SerializeField]
    Text perecentageText;

    [SerializeField]
    Fraction liklihood;

    bool error;

    public void SetGeneCode(Gene[] genes, FlowerType type)
    {
        geneCode = genes;
        geneCodeText.text = Gene.FormatForGUI(genes, type);
    }

    private void Awake()
    {
        error = false;

    }

    private void Start()
    {

        //TEST
        SetGeneCode(new Gene[] { new Gene('2'), new Gene('1'), new Gene('0') }, FlowerType.COSMOS);
    }

    public void SetFieldCol(Color colour)
    {
        textField.image.color = colour;
    }

    readonly Regex fractionRegEx = new Regex(@"^([0-9]+\/[0-9]+)$");
    readonly Regex decimalRegEx = new Regex(@"^([0-9]*(\.[0-9]*)?)$");

    public void OnEdit()
    {
        SetFieldCol(normalColor);
    }

    public void OnExit()
    {
        Debug.Log("hi");
        string currentText = textField.text;
        if (fractionRegEx.Match(currentText).Length > 0)
        {
            int slashIndex = currentText.IndexOf("/");
            int numer = int.Parse(currentText.Substring(0, slashIndex));
            int denom = int.Parse(currentText.Substring(slashIndex + 1));

            liklihood = new Fraction(numer, denom);
        }
        else if(decimalRegEx.Match(currentText).Length > 0)
        {
            if(currentText != ".")
            {
                float value = float.Parse(currentText);
                liklihood = new Fraction(value);
            }
        }
        else
        {
            SetFieldCol(errorColour);
            error = true;
            perecentageText.text = "Error";
            return;
        }

        error = false;
        textField.text = liklihood.ToString();
        perecentageText.text = string.Format("{0:P2}", liklihood.GetDecimal());


    }
}

