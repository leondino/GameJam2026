using UnityEngine;

public class SearchablePart : Interactable
{
    [HideInInspector]
    public bool hasContraband = false;
    public ContrabandData myContraband;
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
        if (hasContraband) 
        {
            GameManager.Instance.player.GetComponent<SearchCustomer>().FoundContraband(myContraband);
            Debug.Log($"{myContraband.name} found in {name}!");
            hasContraband = false;
            myContraband = null;
        }
        else {Debug.Log($"No contraband found in {name}."); }
    }
}
