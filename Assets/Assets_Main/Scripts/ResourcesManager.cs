using Mono.Cecil;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum ResourceType
{
    Gold,
    Stone,
    Wood,
}
public class ResourcesManager : MonoBehaviour
{
    //Ressources
    public float goldAmount;
    public float stoneAmount;
    public float woodAmount;

    //UI
    public TMP_Text goldText;
    public TMP_Text stoneText;
    public TMP_Text woodText;

    //Material
    public Material goldMaterial;
    public Material stoneMaterial;
    public Material woodMaterial;

    // Start is called before the first frame update
    void Start()
    {
        RefreshUI();
    }

    // Update is called once per frame
    public void AddResource(ResourceType resourceType, float value)
    {
        switch (resourceType)
        {
            case ResourceType.Gold:
                {
                    goldAmount += value;
                    break;
                }
            case ResourceType.Stone:
                {
                    stoneAmount += value;
                    break;
                }
            case ResourceType.Wood:
                {
                    woodAmount += value;
                    break;
                }
        }

        RefreshUI();
    }

    void RefreshUI()
    {
        goldText.text = "Gold " + goldAmount.ToString();
        stoneText.text = "Stone " + stoneAmount.ToString();
        woodText.text = "Wood " + woodAmount.ToString();
    }
}
