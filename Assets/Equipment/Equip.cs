using UnityEngine;

public class Equip : MonoBehaviour
{
    public Transform parent;
    public Equipment equipment;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            GameObject go = Instantiate(equipment.Prefab, parent);
            go.transform.localPosition = equipment.Position;
            go.transform.localEulerAngles = equipment.Rotation; 
        }
    }
}
