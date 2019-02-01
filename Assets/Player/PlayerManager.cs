using System;
using Invector.CharacterController;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    public delegate void OnPlayerChanged(GameObject player);
    public OnPlayerChanged PlayerChanged;

    InventoryManager inventoryManager;

    public vThirdPersonCamera cam;
    public GameObject playerWithCameraPrefab;
    public Transform container;
    public Vector3[] playerPositions;
    public string[] playerNames;
    private string[] inventoryIds;

    private GameObject[] players;
    private int currentPlayer;

    // Start is called before the first frame update
    void Start()
    {
        inventoryManager = GetComponent<InventoryManager>();
        players = new GameObject[playerPositions.Length];
        inventoryIds = new string[playerPositions.Length];
        for (int i = 0; i< playerPositions.Length; i++)
        {
            GameObject go = Instantiate(playerWithCameraPrefab, playerPositions[i], Quaternion.identity, container);
            go.name = playerNames[i];
            players[i] = go;
            inventoryIds[i] = inventoryManager.AddInventory(10).Id;
        }
        currentPlayer = 0;
        cam.target = players[currentPlayer].transform;
        players[currentPlayer].AddComponent<vThirdPersonInput>();
        players[currentPlayer].AddComponent<vThirdPersonController>();
    }

    internal Inventory GetCurrentInventory()
    {
        return inventoryManager.GetInventory(inventoryIds[currentPlayer]);
    }

    public void SwitchPlayer()
    {
        Destroy(players[currentPlayer].GetComponent<vThirdPersonInput>());
        Destroy(players[currentPlayer].GetComponent<vThirdPersonController>());
        if (currentPlayer == players.Length - 1)
            currentPlayer = 0;
        else
            currentPlayer++;
        cam.target = players[currentPlayer].transform;
        PlayerChanged?.Invoke(players[currentPlayer]);
        players[currentPlayer].AddComponent<vThirdPersonInput>();
        players[currentPlayer].AddComponent<vThirdPersonController>();
    }

}
    