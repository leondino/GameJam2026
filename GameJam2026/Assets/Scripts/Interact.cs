using System;
using UnityEngine;
using UnityEngine.UI;

public class Interact : MonoBehaviour
{
    [SerializeField]
    private Image interactCrosshair;
    [SerializeField]
    private float maxDistance = 5f;
    [SerializeField]
    private Sprite interactImage, baseImage, fingerImage, gloveImage, gloveFingerImage;
    [SerializeField]
    private Vector2 interactImageSize = new Vector2(48f, 48f);
    private Vector2 baseSize;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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

            if (col.CompareTag("Interactable"))
            {
                interactCrosshair.sprite = interactImage;
                interactCrosshair.rectTransform.sizeDelta = interactImageSize;
                Debug.Log($"I'm looking at body part '{col.name}' (parent NPC '{col.transform.parent?.name ?? "(no parent)"}')");
                return;
            }

            // Hit something but it's not interactable
            interactCrosshair.sprite = baseImage;
            interactCrosshair.rectTransform.sizeDelta = baseSize;
            return;
        }

        // Nothing hit
        interactCrosshair.sprite = baseImage;
        interactCrosshair.rectTransform.sizeDelta = baseSize;
        Debug.Log("I'm looking at nothing!");
    }
}
