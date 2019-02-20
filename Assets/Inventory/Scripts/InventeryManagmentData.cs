using System.Collections.Generic;

[System.Serializable]
internal class InventeryManagmentData
{
    public Dictionary<string, Inventory> inventories = new Dictionary<string, Inventory>();
    public string currentPlayerId;
}