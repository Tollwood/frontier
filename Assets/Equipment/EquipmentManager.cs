using UnityEngine;

public class EquipmentManager : Singleton<EquipmentManager>
{
    public delegate void OnEquiped(Equipment item);
    public OnEquiped onEquipedCallback;

    public delegate void OnUnEquip(Equipment item);
    public OnEquiped onUnEquipCallback;

    public EquipmentSlot rightHand = new EquipmentSlot();

    public bool Equip(Equipment equipment)
    {
        if (rightHand.type == equipment.type)
        {
            UnEquip(rightHand);
            equipUi(rightHand, equipment);
            if(onEquipedCallback != null)
            {
                onEquipedCallback.Invoke(equipment);
            }

            return true;
        }
        return false;
    }

    public bool UnEquip(EquipmentSlot slot)
    {
        foreach(Transform child in PlayerManager.Instance.GetEquipmentPositions().rightHand)
        {
            Destroy(child.gameObject);
        }
        if(rightHand.equipment != null && onUnEquipCallback != null)
        {
            onUnEquipCallback.Invoke(rightHand.equipment);
        }
        rightHand.equipment = null;
        return true;
    }

    private void equipUi(EquipmentSlot slot, Equipment equipment)
    {
        slot.equipment = equipment;
        GameObject go = Instantiate(equipment.Prefab, PlayerManager.Instance.GetEquipmentPositions().rightHand);
        go.transform.localPosition = equipment.Position;
        go.transform.localEulerAngles = equipment.Rotation;
    }
}
