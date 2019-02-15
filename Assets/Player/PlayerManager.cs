using System;
using Invector.CharacterController;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    public Transform container;
    public GameObject playerWithCameraPrefab;

    private GameObject[] players;
    private readonly string FILE_NAME = "playerManager";
    private InventoryManager inventoryManager;

    [SerializeField]
    public PlayerManagerData pmd;

    public vThirdPersonCamera cam;

    // Start is called before the first frame update
    void Start()
    {
        inventoryManager = InventoryManager.Instance;
        EventManager.StartListening(Events.OnSave, onSave);
        EventManager.StartListening(Events.OnLoad, onLoad);
        Load(pmd);
    }

    private void Load(PlayerManagerData data)
    {
        players = new GameObject[data.players.Length];

        for (int i = 0; i < data.players.Length; i++)
        {
            PlayerData playerData = data.players[i];
            GameObject go = Instantiate(playerWithCameraPrefab, playerData.position.ToVector3(), Quaternion.identity, container);
            go.name = playerData.name;
            players[i] = go;
            players[i].GetComponent<vThirdPersonInput>().enabled = false;
            players[i].GetComponent<vThirdPersonController>().enabled = false;
            if(data.players[i].inventoryId != null)
                data.players[i].inventoryId = inventoryManager.AddInventory(10, playerData.name).Id;
        }
        SwitchPlayer(data.currentPlayerIndex);
    }

    internal EquipmentPositions GetEquipmentPositions()
    {
        return players[pmd.currentPlayerIndex].GetComponent< EquipmentPositions>();
    }

    internal Inventory GetCurrentInventory()
    {
        return InventoryManager.Instance.GetInventory(pmd.players[pmd.currentPlayerIndex].inventoryId);
    }

    internal GameObject CurrentPlayer()
    {
        return players[pmd.currentPlayerIndex];
    }

    public void SwitchPlayer()
    {
        int previousIndex = pmd.currentPlayerIndex;
        int newPlayerIndex;
        if (pmd.currentPlayerIndex == players.Length - 1)
            newPlayerIndex = 0;
        else
            newPlayerIndex = pmd.currentPlayerIndex + 1;

        SwitchPlayer(newPlayerIndex);
    }

    public void SwitchPlayer(int index)
    {
        players[pmd.currentPlayerIndex].GetComponent<vThirdPersonInput>().enabled = false;
        players[pmd.currentPlayerIndex].GetComponent<vThirdPersonController>().enabled = false;
        cam.SetMainTarget(players[index].transform);
        EventManager.TriggerEvent(Events.OnPlayerChanged,players[index]);
        players[index].GetComponent<vThirdPersonInput>().enabled = true;
        players[index].GetComponent<vThirdPersonController>().enabled = true;
        pmd.currentPlayerIndex = index;
    }

    public void onSave()
    {
        SaveManager.Save(FILE_NAME, pmd);
    }


    public void onLoad()
    {
        pmd = SaveManager.Load(FILE_NAME);
        // recreate players on load
    }
}
    