using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.DualShock;

public class GameManeger : NetworkBehaviour
{

    [SerializeField] GameObject playerPref;

    [SerializeField] Transform player_1, player_2;

    private bool isSpavnFirst=false;



    private void Start()
    {   
        SpawnRpc(NetworkManager.LocalClientId);
    }

    [Rpc(SendTo.Server)]
    private void SpawnRpc(ulong id)
    {
        GameObject inst = Instantiate(playerPref, isSpavnFirst ? player_1 : player_2);
        isSpavnFirst = true;
        inst.GetComponent<NetworkObject>().SpawnWithOwnership(id);
    }

}
