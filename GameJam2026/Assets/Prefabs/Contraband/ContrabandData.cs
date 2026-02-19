using UnityEngine;

[CreateAssetMenu(fileName = "ContrabandItem", menuName = "Scriptable Objects/ContrabandItem")]
public class ContrabandData : ScriptableObject
{
    public string itemName;
    public int value;
    public Sprite icon;
    public GameObject modelPrefab;
}
