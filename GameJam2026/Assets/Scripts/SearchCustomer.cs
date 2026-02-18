using UnityEngine;
using UnityEngine.InputSystem;

public class SearchCustomer : MonoBehaviour
{
    public WalkObjective currentCustomer = null;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ActiveNPCManager NPCManager = GameManager.Instance.activeNPCManager;
        if (NPCManager.queueNPCs.Count > 0 && currentCustomer == null)
        {
            NPCManager.SendToSearch();
        }
    }
    public void StartSearch(GameObject customer)
    {
        currentCustomer = customer.GetComponent<WalkObjective>();
        currentCustomer.WalkToPoint(GameManager.Instance.activeNPCManager.searchPoint);
        currentCustomer.queueComplete = true;
    }

    public void SearchComplete(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Debug.Log("Search Complete");
            GameManager.Instance.activeNPCManager.SendToDancefloor(currentCustomer);
        }
    }
}
