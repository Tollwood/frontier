
[System.Serializable]
public class EquipmentSlot
{
    public Equipment equipment = null;
    public EquipmentType type;

    public SerializableEquipmentSlot ToSerializableEquipmentSlot()
    {
        if (equipment == null)
            return null;
        return new SerializableEquipmentSlot {
            equipment = this.equipment.name,
            type = this.type };
    }
}
