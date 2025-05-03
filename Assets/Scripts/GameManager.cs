using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : NetworkBehaviour
{
    [SerializeField] Slider MyHealth, EnemyHealth;
    [SerializeField] Image MyImage, EnemyImage;
    [SerializeField] TMP_Text MyName, EnemyName;
    [SerializeField] Image AttackUI;

    [SerializeField] Transform player_1, player_2;
    Character[] Characters;
    public Character MyCharacter(ulong id) => 
        Characters[LobbyOrchestrator.PlayersInCurrentLobby.Find(player => player.id == id).CharacterId];

    public static GameManager Instance;

    private bool isSpawnFirst=false;
    public Image GetAttackUI => AttackUI;
    public Slider GetSlider(ulong id) => NetworkManager.LocalClientId == id ? MyHealth : EnemyHealth;

    private void Awake()
    {
        Instance = this;
        Characters = Resources.LoadAll<Character>("Characters");
    }
    private void Start()
    {
        MyImage.sprite = Characters[LobbyOrchestrator.PlayersInCurrentLobby[IsServer ? 0 : 1].CharacterId].Icon;
        EnemyImage.sprite = Characters[LobbyOrchestrator.PlayersInCurrentLobby[IsServer ? 1 : 0].CharacterId].Icon;
        MyName.text = LobbyOrchestrator.PlayersInCurrentLobby[IsServer ? 0 : 1].Name;
        EnemyName.text = LobbyOrchestrator.PlayersInCurrentLobby[IsServer ? 1 : 0].Name;
        SpawnRpc(NetworkManager.LocalClientId);
    }

    [Rpc(SendTo.Server)]
    private void SpawnRpc(ulong id)
    {
        GameObject inst = Instantiate(MyCharacter(id).prefab, isSpawnFirst ? player_2.position : player_1.position, Quaternion.identity);
        inst.GetComponent<NetworkObject>().SpawnWithOwnership(id);
        isSpawnFirst = true;
    }

}
