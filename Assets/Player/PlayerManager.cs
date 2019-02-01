using Invector.CharacterController;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    public delegate void OnPlayerChanged(GameObject player);
    public OnPlayerChanged PlayerChanged;

    public vThirdPersonCamera cam;
    public GameObject playerWithCameraPrefab;
    public Transform container;
    public Vector3[] playerPositions;
    public string[] playerNames;

    private GameObject[] players;
    private int currentPlayer;


    // Start is called before the first frame update
    void Start()
    {
        players = new GameObject[playerPositions.Length];

        for (int i = 0; i< playerPositions.Length; i++)
        {
            GameObject go = Instantiate(playerWithCameraPrefab, playerPositions[i], Quaternion.identity, container);
            go.name = playerNames[i];
            players[i] = go;
        }
        currentPlayer = 0;
        cam.target = players[currentPlayer].transform;
        players[currentPlayer].AddComponent<vThirdPersonInput>();
        players[currentPlayer].AddComponent<vThirdPersonController>();
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
    