using System;
using UnityEngine;

public class EquipmentManager : Singleton<EquipmentManager>
{
    private EquipmentSlot[] rightHand = null;
    private PlayerData[] players = null;
    private int currentIndex = 0;

    private readonly string FILE_NAME = "equipmentManager";

    private void Awake()
    {
        EventManager.StartListening(Events.NewPlayer, OnNewPlayer);
        EventManager.StartListening(Events.OnPlayerChanged, OnPlayerChanged);
        EventManager.StartListening(Events.OnSave, onSave);
        EventManager.StartListening(Events.OnLoad, onLoad);
    }

    public void OnPlayerChanged(object obj)
    {
        PlayerData playerData = (PlayerData)obj;
        UnEquip(rightHand[currentIndex]);
        UpdateCurrentIndex(playerData.id);
        if(rightHand[currentIndex].equipment != null)
        {
            Equip(rightHand[currentIndex].equipment);
        }
    }

    private void UpdateCurrentIndex(string id)
    {
        for(int i = 0; i < players.Length; i++)
        {
            if (players[i].id == id)
                currentIndex = i;
        }
    }

    public void OnNewPlayer(object obj)
    {
        ResizeArray();
        players[players.Length - 1] = (PlayerData)obj;
        rightHand[players.Length - 1] = new EquipmentSlot();
    }

    private void ResizeArray()
    {
        if (rightHand == null)
        {
            rightHand = new EquipmentSlot[1];
            players = new PlayerData[1];
        }
        else
        {
            Array.Resize(ref rightHand, rightHand.Length + 1);
            Array.Resize(ref players, players.Length + 1);

        }
    }

    private void onLoad()
    {
        EquipmentManagerData emd =  SaveManager.Load<EquipmentManagerData>(FILE_NAME);
        currentIndex = emd.currentIndex;
        for(int i =0; i < emd.equipmentSlots.Length; i++)
        {
            SerializableEquipmentSlot ses =  emd.equipmentSlots[i];

            // save load not working
            rightHand[i] = new EquipmentSlot();
            if (ses != null && ses.equipment != null)
            {
                rightHand[i].equipment = Resources.Load<Equipment>(ses.equipment);
                rightHand[i].type = ses.type;
            }

        }
        if(rightHand[currentIndex].equipment != null){
            equipUi(rightHand[currentIndex], rightHand[currentIndex].equipment);
        }
    }

    private void onSave()
    {
        EquipmentManagerData emd = new EquipmentManagerData
        {
            currentIndex = this.currentIndex,
            equipmentSlots = ToSerializableSlots()
        };
        SaveManager.Save(FILE_NAME, emd);
    }

    private SerializableEquipmentSlot[] ToSerializableSlots()
    {
        SerializableEquipmentSlot[] ses = new SerializableEquipmentSlot[rightHand.Length];
        for(int i = 0; i < rightHand.Length; i++)
        {
            ses[i] = rightHand[i].ToSerializableEquipmentSlot();
        }
        return ses;
    }

    public bool Equip(Equipment equipment)
    {
        EquipmentSlot equipmentSlot = rightHand[currentIndex];
        if (equipmentSlot.type == equipment.type)
        {
            UnEquip(equipmentSlot);
            equipUi(equipmentSlot, equipment);
            EventManager.TriggerEvent(Events.OnItemEquip,equipment);
            return true;
        }
        return false;
    }

    internal EquipmentPositions GetEquipmentPositions()
    {
        return PlayerManager.Instance.CurrentPlayer().GetComponent<EquipmentPositions>();
    }

    public bool UnEquip(EquipmentSlot slot)
    {
        foreach(Transform child in GetEquipmentPositions().rightHand)
        {
            Destroy(child.gameObject);
        }
        if(rightHand[currentIndex].equipment != null)
        {
            EventManager.TriggerEvent(Events.OnItemUnEquip, slot.equipment);
        }
        rightHand[currentIndex].equipment = null;
        return true;
    }

    private void equipUi(EquipmentSlot slot, Equipment equipment)
    {
        slot.equipment = equipment;
        GameObject go = Instantiate(equipment.Prefab, GetEquipmentPositions().rightHand);
        go.transform.localPosition = equipment.Position;
        go.transform.localEulerAngles = equipment.Rotation;
    }
}
