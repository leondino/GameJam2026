using UnityEngine;
using UnityEngine.InputSystem;

public class SearchCustomer : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {        
            
    }
    public void StartSearch(GameObject npc)
    {
        WalkObjective customer = npc.GetComponent<WalkObjective>();
        customer.WalkToPoint(GameManager.Instance.activeNPCManager.searchPoint);
        customer.queueComplete = true;
    }

    private void SearchComplete()
    {
        GameManager.Instance.activeNPCManager.SendToSearch();
    }
}
