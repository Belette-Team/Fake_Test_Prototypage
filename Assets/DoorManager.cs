using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Resources;

public class DoorManager : MonoBehaviour
{
    public int woodNeeded;
    public int stoneNeeded;
    public int goldNeeded;
    [SerializeField] TMP_Text woodNeededUI;
    [SerializeField] TMP_Text stoneNeededUI;
    [SerializeField] TMP_Text goldNeededUI;

    public ResourcesManager resourcesManager;

    // Start is called before the first frame update
    void Start()
    {
        resourcesManager = GameManager.Instance.GetComponentInChildren<ResourcesManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void UpdateUIDoor()
    {
        //wood
        if(woodNeeded >= resourcesManager.woodAmount)
        {
            woodNeededUI.material.color = Color.green;
        }
        else
        {
            woodNeededUI.material.color = Color.red;
        }
        woodNeededUI.text = resourcesManager.woodAmount + "/" + woodNeeded;

        //stone
        if (stoneNeeded >= resourcesManager.stoneAmount)
        {
            stoneNeededUI.material.color = Color.green;
        }
        else
        {
            stoneNeededUI.material.color = Color.red;
        }
        stoneNeededUI.text = resourcesManager.stoneAmount + "/" + stoneNeeded;

        //gold
        if (goldNeeded >= resourcesManager.goldAmount)
        {
            goldNeededUI.material.color = Color.green;
        }
        else
        {
            goldNeededUI.material.color = Color.red;
        }
        goldNeededUI.text = resourcesManager.goldAmount + "/" + goldNeeded;

    }
}
