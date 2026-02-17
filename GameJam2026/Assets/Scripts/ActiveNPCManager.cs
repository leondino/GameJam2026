using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class ActiveNPCManager : MonoBehaviour
{
    [SerializeField]
    public Transform firstWalkPoint;
    public Transform[] queuePoints = new Transform[5];
    public Queue<GameObject> activeNPCs = new Queue<GameObject>();
    public Queue<GameObject> queueNPCs = new Queue<GameObject>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
