using UnityEngine;

/* The base item class. All items should derive from this. */

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Seed")]
[System.Serializable]
public class Seed : Equipment
{
    public Plant plant;
}