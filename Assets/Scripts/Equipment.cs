using UnityEngine;

/* The base item class. All items should derive from this. */

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Equipment")]
public class Equipment : Item {

    public GameObject Prefab;
    public Vector3 Position;
    public Vector3 Rotation;
}