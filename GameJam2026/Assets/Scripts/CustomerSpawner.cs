using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    [Header("Spawning")]
    [SerializeField]
    private float spawnInterval = 30f;
    private float spawnTimer = 0f;
    [SerializeField]
    private Transform spawnPoint;
    public List<GameObject> possibleCustomers = new List<GameObject>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void SpawnCustomer()
    {
        // Handle periodic customer spawning

        spawnTimer += Time.fixedDeltaTime;
        if (spawnTimer >= spawnInterval)
        {
            spawnTimer = 0f;
            //TODO Check queue length before spawning new customer
            if (possibleCustomers == null || possibleCustomers.Count == 0)
            {
                Debug.LogWarning("CustomerSpawner: no possible customers assigned.");
                return;
            }

            int randomIndex = UnityEngine.Random.Range(0, possibleCustomers.Count);
            var prefab = possibleCustomers[randomIndex];
            if (prefab == null)
            {
                Debug.LogWarning($"CustomerSpawner: selected prefab at index {randomIndex} is null.");
                return;
            }

            GameObject newNpc = Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
            GameManager.Instance.activeNPCManager.activeNPCs.Enqueue(newNpc);
        }
    }
}
