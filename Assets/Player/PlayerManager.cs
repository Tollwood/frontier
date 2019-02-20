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
        EventManager.StartListening(Events.OnSave, onSave);
        EventManager.StartListening(Events.OnLoad, onLoad);
        Load(pmd, true);
    }

    private void Load(PlayerManagerData data, bool isNew = false)
    {
        pmd = data;
        DestroyCurrentPlayers();
        players = new GameObject[data.players.Length];
        for (int i = 0; i < data.players.Length; i++)
        {
            players[i] = AddPlayer(data.players[i]);
        }
        SwitchPlayer(data.currentPlayerIndex);
    }

    private GameObject AddPlayer(PlayerData playerData)
    {
        GameObject go = Instantiate(playerWithCameraPrefab, playerData.position.ToVector3(), Quaternion.identity, container);
        go.name = playerData.name;
        if (playerData.id == "")
        {
            playerData.id = Guid.NewGuid().ToString();
            EventManager.TriggerEvent(Events.NewPlayer, playerData);
        }
        return go;
    }

    private void DestroyCurrentPlayers()
    {
        if (players != null)
        {
            foreach (GameObject go in players)
            {
                Destroy(go);
            }
        }
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
        EventManager.TriggerEvent(Events.OnPlayerChanged,pmd.players[index]);
        players[index].GetComponent<vThirdPersonInput>().enabled = true;
        players[index].GetComponent<vThirdPersonController>().enabled = true;
        pmd.currentPlayerIndex = index;
    }

    public void onSave()
    {
        for(int i = 0; i < players.Length; i++)
        {
            pmd.players[i].position.x = players[i].transform.position.x;
            pmd.players[i].position.y = players[i].transform.position.y;
            pmd.players[i].position.z = players[i].transform.position.z;
        }
        SaveManager.Save(FILE_NAME, pmd);
    }

    public void onLoad()
    {
        Load(SaveManager.Load<PlayerManagerData>(FILE_NAME));
    }
}