using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameManager : NetworkBehaviour
{
    [SerializeField] SceneFader fader;
    [SerializeField] GameObject GameOverWindow;
    [SerializeField] Slider LeftHealth, RightHealth;
    [SerializeField] Image LeftImage, RightImage;
    [SerializeField] TMP_Text LeftName, RightName;
    [SerializeField] Image AttackUI;

    [SerializeField] Transform player_1, player_2;
    public static UnityEvent<bool> OnGameOver = new();
    Character[] Characters;
    public Character MyCharacter(ulong id) => 
        Characters[LobbyOrchestrator.PlayersInCurrentLobby.Find(player => player.id == id).CharacterId];

    public static GameManager Instance;

    private bool isSpawnFirst=false;
    public Image GetAttackUI => AttackUI;
    public Slider GetSlider(ulong id) => LobbyOrchestrator.PlayersInCurrentLobby[0].id==id ? LeftHealth : RightHealth;

    private void Awake()
    {
        Instance = this;
        Characters = Resources.LoadAll<Character>("Characters");
        OnGameOver.AddListener(GameOver);
    }
    private void Start()
    {
        fader.FAdeIn(SceneFader.FadeType.PlainBlack);
        LeftImage.sprite = Characters[LobbyOrchestrator.PlayersInCurrentLobby[0].CharacterId].Icon;
        RightImage.sprite = Characters[LobbyOrchestrator.PlayersInCurrentLobby[1].CharacterId].Icon;
        LeftName.text = LobbyOrchestrator.PlayersInCurrentLobby[0].Name;
        RightName.text = LobbyOrchestrator.PlayersInCurrentLobby[1].Name;
        SpawnRpc(NetworkManager.LocalClientId);
    }

    [Rpc(SendTo.Server)]
    private void SpawnRpc(ulong id)
    {
        GameObject inst = Instantiate(MyCharacter(id).prefab, isSpawnFirst ? player_2.position : player_1.position, Quaternion.identity);
        inst.GetComponent<NetworkObject>().SpawnWithOwnership(id);
        isSpawnFirst = true;
    }
    void GameOver(bool isWin)
    {
        InputManager.Input.Disable();
        GameOverWindow.SetActive(true);
    }
}
