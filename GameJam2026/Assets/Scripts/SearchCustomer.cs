using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SearchCustomer : MonoBehaviour
{
    public WalkObjective currentCustomer = null;
    [SerializeField]
    private Image foundContrabandUI;
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
    /// <summary>
    /// 
    /// Initiates the search process for the specified customer by generating contraband information and directing the
    /// customer to a designated search point.
    /// </summary>
    /// <remarks>This method sets the current customer and marks the search queue as complete once the
    /// customer reaches the search point.</remarks>
    /// <param name="customer">The GameObject representing the customer for whom the search is initiated. This object must have a
    /// SearchInformation component to generate contraband.</param>
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

    public void FoundContraband(ContrabandData contraband)
    {
        foundContrabandUI.sprite = contraband.icon;
        foundContrabandUI.transform.parent.GetComponentInChildren<TMP_Text>().text = $"You found\n{contraband.itemName}";
        StartCoroutine(ShowFoundContraband());
        GetComponent<Inventory>().AddItemToInventory(contraband);
    }

    private IEnumerator ShowFoundContraband()
    {
        GetComponentInParent<Interact>().canInteract = false;
        foundContrabandUI.transform.parent.gameObject.SetActive(true);
        yield return new WaitForSeconds(2.5f);
        foundContrabandUI.transform.parent.gameObject.SetActive(false);
        GetComponentInParent<Interact>().canInteract = true;
    }
}
