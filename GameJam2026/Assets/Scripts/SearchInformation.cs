using NUnit.Framework;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class SearchInformation : MonoBehaviour
{
    private const float TOTAL_BODY_PARTS = 17;

    [SerializeField]
    private int maxContrabandCount = 1;
    private int currentContrabandCount = 0;
    [SerializeField]
    [Tooltip("Total chance to have contraband when generating, will be devided by amount of body parts (17). Value should be between 0 and 1.")]
    private float contrabandChance = 0.5f;
    [SerializeField]
    private List<SearchablePart> bodyParts = new List<SearchablePart>();
    [SerializeField]
    private List<ContrabandData> possibleContrabandItems = new List<ContrabandData>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Randomly assigns contraband items to body parts, subject to specified chance and maximum count constraints.
    /// </summary>
    public void GenerateContraband()
    {
        //Switch to interactable tag for all body parts
        foreach (var part in bodyParts)
        {
            part.gameObject.tag = "Interactable";
        }

        //Randomly assign contraband to body parts based on the contrabandChance and maxContrabandCount
        float randomValue = Random.value;
        Debug.Log($"Generated random value: {randomValue}");
        while (randomValue < contrabandChance && currentContrabandCount < maxContrabandCount)
        {
            SearchablePart part = bodyParts[Random.Range(0, bodyParts.Count)];
            part.hasContraband = true;
            //Assign a random contraband item to the part, with a rarity that is less than or equal to a random max rarity (between 1 and 3)
            int maxRarity = Random.Range(1, 4);
            ContrabandData contraband = possibleContrabandItems[Random.Range(0, possibleContrabandItems.Count)];
            Debug.Log($"Tried generating {contraband.name} with max rarity {maxRarity}");
            while (contraband.rarity > maxRarity) 
            {
                contraband = possibleContrabandItems[Random.Range(0, possibleContrabandItems.Count)];
                Debug.Log($"Tried generating {contraband.name}");
            }
            Debug.Log($"Assigned contraband {contraband.itemName} with rarity {contraband.rarity} to part {part.name}");
            part.myContraband = contraband;
            randomValue = Random.value;
            currentContrabandCount++;
        }
        if (currentContrabandCount == 0) Debug.Log("No contraband generated, try increasing the contraband chance or max contraband count.");
    }

    public void ResetBodyInteractablity()
    {
        foreach (var part in bodyParts)
        {
            part.gameObject.tag = "Untagged";
        }
    }
}
