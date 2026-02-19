using UnityEngine;

[CreateAssetMenu(fileName = "ContrabandItem", menuName = "Scriptable Objects/ContrabandItem")]
public class ContrabandData : ScriptableObject
{
    public string itemName;
    public int value;
    public int rarity; // 1-3, 1 being most common, 3 being most rare
    public Sprite icon;
    public GameObject modelPrefab;
}
