using UnityEngine;

public class UIBossmanScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponent<Animator>().SetBool("isDancing", true);
    }
}
