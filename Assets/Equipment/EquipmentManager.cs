using UnityEngine;

public class EquipmentManager : Singleton<EquipmentManager>
{
    public EquipmentSlot rightHand = new EquipmentSlot();

    public bool Equip(Equipment equipment)
    {
        if (rightHand.type == equipment.type)
        {
            UnEquip(rightHand);
            equipUi(rightHand, equipment);
            EventManager.TriggerEvent(Events.OnItemEquip,equipment);
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
        if(rightHand.equipment != null)
        {
            EventManager.TriggerEvent(Events.OnItemUnEquip, rightHand.equipment);
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
