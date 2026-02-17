using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ActiveNPCManager : MonoBehaviour
{
    [SerializeField]
    public Transform firstWalkPoint;
    public Transform searchPoint;
    public Transform[] queuePoints = new Transform[5];
    public Queue<GameObject> activeNPCs = new Queue<GameObject>();
    public Queue<GameObject> queueNPCs = new Queue<GameObject>();
    public int maxQueueLength = 5;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (InputSystem.actions.FindAction("Interact").WasPressedThisFrame())
            SendToSearch();
    }

    private void HandleQueue()
    {
        //foreach (var npc in queueNPCs)
        //{
        //    if (activeNPCs.Count < maxQueueLength)
        //    {
        //        activeNPCs.Enqueue(npc);
        //        queueNPCs.Dequeue();
        //    }
        //    else
        //    {
        //        break;
        //    }
        //}
    }

    public void SendToSearch()
    {
        Debug.Log("Next in line was Send To Search");
        GameManager.Instance.player.GetComponent<SearchCustomer>().StartSearch(queueNPCs.Dequeue());

        for (int i = 0; i < queueNPCs.Count; i++)
        {
            GameObject npc = queueNPCs.Dequeue();
            npc.GetComponent<WalkObjective>().WalkToPoint(queuePoints[i]);
            queueNPCs.Enqueue(npc);
        }
    }

    public void AddToQueue(GameObject customer)
    {
        queueNPCs.Enqueue(customer);
        customer.GetComponent<WalkObjective>().WalkToPoint(queuePoints[queueNPCs.Count - 1]);
    }
}
