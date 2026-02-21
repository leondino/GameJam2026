using UnityEngine;

public class SearchablePart : Interactable
{
    [HideInInspector]
    public bool hasContraband = false;
    public ContrabandData myContraband;
    public bool needsGlove = false;
    public AudioClip searchedAudio;
    public AudioClip foundContrabandAudio;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override void Interact()
    {
        base.Interact();
        GameObject player = GameManager.Instance.player;
        if (player.GetComponent<Inventory>().selectedItem != null)
        {
            GameManager.Instance.inventoryBarManager.ShowTooltip("You need a free hand to search!.");
            return;
        }

        if (hasContraband) 
        {
            player.GetComponent<SearchCustomer>().FoundContraband(myContraband);
            GameManager.Instance.audioPlayManager.PlayAudioClip(foundContrabandAudio);
            Debug.Log($"{myContraband.name} found in {name}!");
            hasContraband = false;
            myContraband = null;
        }
        else 
        {
            GameManager.Instance.audioPlayManager.PlayAudioClip(searchedAudio);
            Debug.Log($"No contraband found in {name}."); 
        }
    }
}
