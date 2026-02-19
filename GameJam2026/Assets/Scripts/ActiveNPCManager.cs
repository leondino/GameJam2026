using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class ActiveNPCManager : MonoBehaviour
{
    [SerializeField]
    private int maxNPCs = 25;
    public int maxQueueLength = 5;
    [SerializeField]
    public Transform firstWalkPoint;
    public Transform searchPoint;
    public Transform dancefloorEntryPoint;
    public Transform[] dancefloorPoints = new Transform[20];
    public Transform[] queuePoints = new Transform[5];
    private bool[] dancefloorSpotsOccupied;
    public Queue<GameObject> activeNPCs = new Queue<GameObject>();
    public Queue<GameObject> queueNPCs = new Queue<GameObject>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        dancefloorSpotsOccupied = new bool[dancefloorPoints.Length];
        for (int i = 0; i < dancefloorSpotsOccupied.Length; i++)
        {
            dancefloorSpotsOccupied[i] = false;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //if (InputSystem.actions.FindAction("Interact").WasPressedThisFrame())
        //{
        //    SendToSearch();
        //}
    }

    public void SendToSearch()
    {
        SearchCustomer playerSearch = GameManager.Instance.player.GetComponent<SearchCustomer>();
        if (queueNPCs.Count > 0)
        {
            Debug.Log("Next in line was Send To Search");
            playerSearch.StartSearch(queueNPCs.Dequeue());

            for (int i = 0; i < queueNPCs.Count; i++)
            {
                GameObject npc = queueNPCs.Dequeue();
                npc.GetComponent<WalkObjective>().WalkToPoint(queuePoints[i]);
                queueNPCs.Enqueue(npc);
            }
        }
        else
        {
            Debug.Log("No one in line for next search.");
            playerSearch.currentCustomer = null;
        }
    }

    public void SendToDancefloor(WalkObjective customer)
    {
        GameManager.Instance.SearchCompleteUI.SetActive(false);
        SendToSearch(); //Next in line is sent to searchpoint.
        if (customer != null)
        {
            customer.WalkToPoint(dancefloorEntryPoint);
            customer.goesDancing = true;
            customer.GetComponent<SearchInformation>().ResetBodyInteractablity();
        }
    }

    /// <summary>
    /// Gets an available dancefloor spot for a customer. If all spots are currently occupied, reclaims a spot from a
    /// departing customer.
    /// </summary>
    /// <remarks>This method ensures that a dancefloor spot is always available by either assigning an
    /// unoccupied spot or reclaiming one from a customer who is leaving. Callers should be aware that reclaiming a spot
    /// involves removing an active NPC from the queue and destroying its associated GameObject.</remarks>
    /// <returns>A Transform representing an available dancefloor spot. If all spots are occupied, returns the spot freed by a
    /// customer who has just left.</returns>
    public Transform GetAvailableDancefloorSpot()
    {
        for (int i = 0; i < dancefloorSpotsOccupied.Length; i++)
        {
            if (!dancefloorSpotsOccupied[i])
            {
                dancefloorSpotsOccupied[i] = true;
                return dancefloorPoints[i];
            }
        }
        GameObject leavingCustomer = activeNPCs.Dequeue();
        Transform openSpot = leavingCustomer.GetComponent<WalkObjective>().dancefloorSpot;
        Destroy(leavingCustomer);
        return openSpot; // Returns available spot from recently left customer.
    }

    public void AddToQueue(GameObject customer)
    {
        queueNPCs.Enqueue(customer);
        customer.GetComponent<WalkObjective>().WalkToPoint(queuePoints[queueNPCs.Count - 1]);
    }
}
