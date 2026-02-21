using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Interact : MonoBehaviour
{
    [SerializeField]
    private Image interactCrosshair;
    [SerializeField]
    private float maxDistance = 1f;
    [SerializeField]
    private Sprite interactImage, baseImage, fingerImage, gloveImage, gloveFingerImage;
    [HideInInspector]
    public bool canInteract = true;

    private Interactable currentInteractable;
    [SerializeField]
    private Vector2 interactImageSize = new Vector2(40f, 40f);
    [SerializeField]
    private Vector2 interactPopSize = new Vector2(50f, 50f);
    private Vector2 baseSize;

    private bool interactImageActive = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canInteract = true;
        baseImage = interactCrosshair.sprite;
        // Cache the base rect size and provide a sensible default for the interact size
        baseSize = interactCrosshair.rectTransform.sizeDelta;
    }

    // Update is called once per frame
    void Update()
    {
        LookingAtInteractable();
    }

    private void LookingAtInteractable()
    {
        var cam = Camera.main;
        if (cam == null) return;

        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance))
        {
            var col = hit.collider; // exact collider hit

            if (col.CompareTag("Interactable") && canInteract)
            {
                if (!interactImageActive)
                {
                    //TODO Fix this to check for gloves when that feeature is added and make it with less nested if statements.
                    interactCrosshair.sprite = interactImage;
                    if (col.GetComponent<SearchablePart>())
                    {
                        if (col.GetComponent<SearchablePart>().needsGlove)
                            interactCrosshair.sprite = fingerImage;
                    }
                    interactCrosshair.rectTransform.sizeDelta = interactImageSize;
                    interactImageActive = true;
                }
                currentInteractable = col.GetComponent<Interactable>();
                //Debug.Log($"I'm looking at body part '{col.name}' (parent NPC '{col.transform.parent?.name ?? "(no parent)"}')");
                return;
            }
        }

        // Nothing interactable hit
        interactImageActive = false;
        interactCrosshair.rectTransform.sizeDelta = baseSize;
        interactCrosshair.sprite = baseImage;
        currentInteractable = null;
    }

    public void InteractWith(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (currentInteractable != null)
            {
                StartCoroutine(PopInteractable());
                currentInteractable.Interact();
            }
        }
    }

    /// <summary>
    /// Temporarily increases the size of the interact crosshair to provide visual feedback when an interactable element
    /// is present.
    /// </summary>
    /// <remarks>Use this method to visually indicate to the user that an interactable object is available.
    /// The crosshair size change is brief and intended to draw attention without interrupting gameplay.</remarks>
    /// <returns>An enumerator that yields after 0.1 seconds, allowing the crosshair to revert to its original size.</returns>
    private IEnumerator PopInteractable()
    {
        Debug.Log("Popping interactable crosshair");
        interactCrosshair.rectTransform.sizeDelta = interactPopSize;
        yield return new WaitForSeconds(0.1f);
        interactCrosshair.rectTransform.sizeDelta = interactImageSize;
    }
}
