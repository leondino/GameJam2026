using UnityEngine;

public class SearchablePart : Interactable
{
    [HideInInspector]
    public bool hasContraband = false;
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
        if (hasContraband) {Debug.Log($"Contraband found in {name}!");}
        else {Debug.Log($"No contraband found in {name}."); }
    }
}
