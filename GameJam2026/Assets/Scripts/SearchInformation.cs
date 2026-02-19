using NUnit.Framework;
using System.Collections.Generic;
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


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerateContraband()
    {
        foreach (var part in bodyParts)
        {
            part.gameObject.tag = "Interactable";
            if (Random.value < (contrabandChance/TOTAL_BODY_PARTS) && currentContrabandCount < maxContrabandCount)
            {
                part.hasContraband = true;
                currentContrabandCount++;
            }
        }
    }
}
