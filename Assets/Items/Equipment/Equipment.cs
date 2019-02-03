using UnityEngine;

/* The base item class. All items should derive from this. */

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Equipment")]
[System.Serializable]
public class Equipment : Item {

    public EquipmentType type;
    public GameObject Prefab;
    public Vector3 Position;
    public Vector3 Rotation;
}