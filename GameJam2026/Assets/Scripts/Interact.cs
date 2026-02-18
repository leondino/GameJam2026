using UnityEngine;

public class Interact : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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
        float maxDistance = 50f;
        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance))
        {
            var col = hit.collider; // exact collider hit
            Debug.Log($"Hit collider '{col.name}'");

            if (col.CompareTag("Interactable"))
            {
                Debug.Log($"I'm looking at body part '{col.name}' (parent NPC '{col.transform.parent?.name ?? "(no parent)"}')");
                return;
            }
        }

        Debug.Log("I'm looking at nothing!");
    }
}
