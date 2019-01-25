using UnityEngine;

public class Equip : MonoBehaviour
{
    public Transform rightHand;
    public Equipment equipment;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            GameObject go = Instantiate(equipment.Prefab, rightHand);
            go.transform.localPosition = equipment.Position;
            go.transform.localEulerAngles = equipment.Rotation; 
        }

    }
}
