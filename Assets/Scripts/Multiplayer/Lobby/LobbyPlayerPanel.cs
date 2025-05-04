using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyPlayerPanel : MonoBehaviour {
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text _nameText, _statusText;
    [SerializeField] private string ready, notReady;

    public ulong PlayerId { get; private set; }

    public void Init(ulong playerId, LobbyOrchestrator.PlayerData data) {
        PlayerId = playerId;
        _nameText.text = data.Name;
    }

    public void SetReady(bool isReady) {
        _statusText.text = (isReady) ? ready : notReady;
        _statusText.color = (isReady) ? Color.green : Color.red;
    }
    public void SetIcon(Sprite icon) => this.icon.sprite = icon;
}