using System;
using UnityEngine;

public class Bossman : Interactable
{
    [SerializeField]
    private float happiness = 100f;
    [SerializeField]
    private float happinessDecreaseRatePerMinute = 10f;
    [SerializeField]
    private HappinessBar happinessBar;
    [SerializeField]
    private AudioClip bossmanThanksAudio; 
    [SerializeField]
    // How much the happiness-decrease rate itself increases per minute (difficulty scaling)
    private float difficultyIncreaseRatePerMinute = 0.5f;
    [SerializeField]
    // Toggle difficulty scaling on/off
    private bool enableDifficultyScaling = true;
    [SerializeField]
    // Optional cap for the per-minute decrease rate so it doesn't grow without bound
    private float maxHappinessDecreaseRatePerMinute = 30f;

    /// <summary>
    /// Read-only access to current happiness value.
    /// </summary>
    public float Happiness => happiness;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called on a fixed timestep
    void FixedUpdate()
    {
        // Decrease happiness over time based on the per-minute rate.
        if (happiness > 0f && happinessDecreaseRatePerMinute > 0f)
        {
            float decreasePerSecond = happinessDecreaseRatePerMinute / 60f;
            happiness -= decreasePerSecond * Time.fixedDeltaTime;
            UpdateHappinessBar();

            // Increase the per-minute decrease rate over time to scale difficulty
            if (enableDifficultyScaling && difficultyIncreaseRatePerMinute > 0f && happinessDecreaseRatePerMinute < maxHappinessDecreaseRatePerMinute)
            {
                float increasePerSecond = difficultyIncreaseRatePerMinute / 60f;
                happinessDecreaseRatePerMinute += increasePerSecond * Time.fixedDeltaTime;
                happinessDecreaseRatePerMinute = Mathf.Min(happinessDecreaseRatePerMinute, maxHappinessDecreaseRatePerMinute);
            }
        }
        else
        {
            // Game over TODO
            Debug.Log("GAME OVER!!!");
        }
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
        if (retrievedItem == null) return; // No item selected, do nothing
        playerInventory.RemoveSelectedItem();
        GameManager.Instance.audioPlayManager.PlayAudioClip(bossmanThanksAudio);
        if (happiness < 100)
        {
            happiness += retrievedItem.value;
            happiness = Mathf.Clamp(happiness, 0f, 100f);
            UpdateHappinessBar();
        }
    }

    private void UpdateHappinessBar()
    {
        if (happinessBar != null)
        {
            happinessBar.SetHappiness(happiness);
        }
    }
}
