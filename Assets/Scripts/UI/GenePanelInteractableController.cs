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

    Fraction liklihood;

    bool error;

    RectTransform rTransform;

    readonly int MAX_INPUT = 1000;

    public void SetGeneCode(Gene[] genes, FlowerType type)
    {
        geneCode = genes;
        geneCodeText.text = Gene.FormatForGUI(genes, type);
    }

    public Gene[] GetGenes()
    {
        return geneCode;
    }

    public void SetProb(Fraction prob)
    {
        if(prob.denominator > 100)
        {
            prob.RoundToDenom(100);
        }
        liklihood = prob;
        textField.text = prob.ToString();
        perecentageText.text = perecentageText.text = string.Format("{0:P2}", liklihood.GetDecimal());
    }

    public Fraction GetProb()
    {
        return liklihood;
    }

    public void SetWidth(int width)
    {
        if (rTransform == null)
        {
            rTransform = GetComponent<RectTransform>();
        }

        rTransform.sizeDelta = new Vector2(width, rTransform.rect.height);
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

    public void Error(string message)
    {
        SetFieldCol(errorColour);
        error = true;
        perecentageText.text = message;
    }

    readonly Regex fractionRegEx = new Regex(@"^([0-9]+\/[0-9]+)$");
    readonly Regex decimalRegEx = new Regex(@"^([0-9]*(\.[0-9]*)?)$");

    public void OnEdit()
    {
        SetFieldCol(normalColor);
    }

    public void OnExit()
    {
        Fraction newLiklihood = new Fraction(0, 0);
        string currentText = textField.text;
        if (fractionRegEx.Match(currentText).Length > 0)
        {
            int slashIndex = currentText.IndexOf("/");
            long numer = long.Parse(currentText.Substring(0, slashIndex));
            long denom = long.Parse(currentText.Substring(slashIndex + 1));

            if (numer * denom > MAX_INPUT)
            {
                Error("Too Large");
                return;
            }

            newLiklihood = new Fraction(numer, denom);
        }
        else if(decimalRegEx.Match(currentText).Length > 0)
        {
            if(currentText != ".")
            {
                double value = double.Parse(currentText);
                if (value > MAX_INPUT)
                {
                    Error("Too Large");
                    return;
                }
                newLiklihood = new Fraction(value);
            }
        }
        else
        {
            Error("Bad Format");
            return;
        }


        error = false;
        SetProb(newLiklihood);
    }
}

