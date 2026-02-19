using UnityEngine;

public class Interactable : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    /// <summary>
    /// Override this in derived classes to implement custom interaction behaviour.
    public virtual void Interact()
    {
        // Default implementation does nothing. Override in subclasses.
    }

}
