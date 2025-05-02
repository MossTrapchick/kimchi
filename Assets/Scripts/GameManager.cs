using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : NetworkBehaviour
{
    [SerializeField] GameObject playerPref;
    [SerializeField] Slider MyHealth, EnemyHealth;
    [SerializeField] Image MyImage, EnemyImage;
    [SerializeField] TMP_Text MyName, EnemyName;
    [SerializeField] Image AttackUI;

    [SerializeField] Transform player_1, player_2;

    public static GameManager Instance;

    private bool isSpawnFirst=false;
    public Image GetAttackUI => AttackUI;
    public Slider GetSlider(ulong id) => NetworkManager.LocalClientId == id ? MyHealth : EnemyHealth;

    private void Awake() => Instance = this;
    private void Start()
    {
        MyName.text = LobbyOrchestrator.PlayersInCurrentLobby[IsServer ? 0 : 1].Name;
        EnemyName.text = LobbyOrchestrator.PlayersInCurrentLobby[IsServer ? 1 : 0].Name;
        SpawnRpc(NetworkManager.LocalClientId);
    }

    [Rpc(SendTo.Server)]
    private void SpawnRpc(ulong id)
    {
        GameObject inst = Instantiate(playerPref, isSpawnFirst ? player_2.position : player_1.position, Quaternion.identity);
        inst.GetComponent<NetworkObject>().SpawnWithOwnership(id);
        isSpawnFirst = true;
    }

}
