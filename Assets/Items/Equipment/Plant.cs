using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Plant")]
[System.Serializable]
public class Plant : ScriptableObject
{
    public GameObject prefab;
    public bool harvestable;
    public float timeToLife;
    public Plant nextPlant;
}