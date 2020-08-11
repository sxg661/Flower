using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GenePanelController : MonoBehaviour
{
    [SerializeField]
    Text geneText;

    [SerializeField]
    Text fractionText;

    [SerializeField]
    Text percentageText;


    Gene[] genes;
    Fraction liklihood;
    FlowerType type;

    public void GiveDetails(Gene[] genes, Fraction liklihood, FlowerType type)
    {
        this.genes = genes;
        this.liklihood = liklihood;
        this.type = type;
    }

    // Start is called before the first frame update
    void Start()
    {
        geneText.text = Gene.FormatForGUI(genes, type);
        fractionText.text = liklihood.ToString();
        percentageText.text = string.Format("{0:P2}", liklihood.GetDecimal());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
