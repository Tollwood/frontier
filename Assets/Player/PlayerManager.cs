using System;
using Invector.CharacterController;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    public delegate void OnPlayerChanged(GameObject player);
    public OnPlayerChanged PlayerChanged;

    public vThirdPersonCamera cam;
    public GameObject playerWithCameraPrefab;
    public Transform container;
    public Vector3[] playerPositions;
    public string[] playerNames;
    private string[] inventoryIds;
    private EquipmentPositions[] equipmentPositions;

    private GameObject[] players;
    private int currentIndex;

    // Start is called before the first frame update
    void Start()
    {
        InventoryManager inventoryManager = InventoryManager.Instance;
        players = new GameObject[playerPositions.Length];
        inventoryIds = new string[playerPositions.Length];
        equipmentPositions = new EquipmentPositions[playerPositions.Length];
        for (int i = 0; i< playerPositions.Length; i++)
        {
            GameObject go = Instantiate(playerWithCameraPrefab, playerPositions[i], Quaternion.identity, container);
            go.name = playerNames[i];
            players[i] = go;
            inventoryIds[i] = inventoryManager.AddInventory(10, playerNames[i]).Id;
            equipmentPositions[i] = go.GetComponent<EquipmentPositions>();
        }
        SwitchPlayer(0);
    }

    internal EquipmentPositions GetEquipmentPositions()
    {
        return equipmentPositions[currentIndex];
    }

    internal Inventory GetCurrentInventory()
    {
        return InventoryManager.Instance.GetInventory(inventoryIds[currentIndex]);
    }

    internal GameObject CurrentPlayer()
    {
        return players[currentIndex];
    }

    public void SwitchPlayer()
    {
        int newPlayerIndex;
        if (currentIndex == players.Length - 1)
            newPlayerIndex = 0;
        else
            newPlayerIndex = currentIndex + 1;

        SwitchPlayer(newPlayerIndex);
    }

    public void SwitchPlayer(int index)
    {
        //Destroy(players[currentPlayer].GetComponent<vThirdPersonInput>());
        //Destroy(players[currentPlayer].GetComponent<vThirdPersonController>());
        cam.target = players[index].transform;
        PlayerChanged?.Invoke(players[currentIndex]);
        players[currentIndex].AddComponent<vThirdPersonInput>();
        players[currentIndex].AddComponent<vThirdPersonController>();
    }

}
    