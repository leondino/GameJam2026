using System;
using UnityEngine;

public class Bossman : Interactable
{
    [SerializeField]
    private float happiness = 100f;
    [SerializeField]
    private float happinessDecreaseRatePerMinute = 10f;

    /// <summary>
    /// Read-only access to current happiness value.
    /// </summary>
    public float Happiness => happiness;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Decrease happiness over time based on the per-minute rate.
        if (happiness > 0f && happinessDecreaseRatePerMinute > 0f)
        {
            float decreasePerSecond = happinessDecreaseRatePerMinute / 60f;
            happiness -= decreasePerSecond * Time.fixedDeltaTime;
        }
        else
            //Game over TODO
            Debug.Log("GAME OVER!!!");
    }

    public override void Interact()
    {
        base.Interact();
        RetrieveItem();
    }

    private void RetrieveItem()
    {
        Inventory playerInventory = GameManager.Instance.player.GetComponent<Inventory>();
        ContrabandData retrievedItem = (ContrabandData)playerInventory.selectedItem;
        playerInventory.RemoveSelectedItem();
        if (happiness < 100)
        {
            happiness += retrievedItem.value;
            happiness = Mathf.Clamp(happiness, 0f, 100f);
        }
    }
}
