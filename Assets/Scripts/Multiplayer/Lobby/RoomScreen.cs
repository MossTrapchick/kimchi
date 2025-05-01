using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RoomScreen : MonoBehaviour {
    [SerializeField] private LobbyPlayerPanel _playerPanelPrefab;
    [SerializeField] private Transform _playerPanelParent;
    [SerializeField] private TMP_Text _waitingText, _passwordField;
    [SerializeField] private float _startingTime = 3f;
    private bool isReady = false, allReady = false;
    private string lobbyId;

    private readonly List<LobbyPlayerPanel> _playerPanels = new();

    public static UnityEvent<string> StartPressed = new(); 

    private void OnEnable() {
        foreach (Transform child in _playerPanelParent) Destroy(child.gameObject);
        _playerPanels.Clear();

        LobbyOrchestrator.LobbyPlayersUpdated.AddListener(NetworkLobbyPlayersUpdated);
        MatchmakingService.CurrentLobbyRefreshed.AddListener(OnCurrentLobbyRefreshed);
    }

    private void OnDisable() {
        LobbyOrchestrator.LobbyPlayersUpdated.RemoveListener(NetworkLobbyPlayersUpdated);
        MatchmakingService.CurrentLobbyRefreshed.RemoveListener(OnCurrentLobbyRefreshed);
    }

    public static UnityEvent LobbyLeft = new();

    public void OnLeaveLobby() {
        LobbyLeft?.Invoke();
    }
    Coroutine starting;
    private void NetworkLobbyPlayersUpdated(Dictionary<ulong, LobbyOrchestrator.PlayerData> players) {
        var allActivePlayerIds = players.Keys;

        // Remove all inactive panels
        var toDestroy = _playerPanels.Where(p => !allActivePlayerIds.Contains(p.PlayerId)).ToList();
        foreach (var panel in toDestroy) {
            _playerPanels.Remove(panel);
            Destroy(panel.gameObject);
        }

        foreach (var player in players) {
            var currentPanel = _playerPanels.FirstOrDefault(p => p.PlayerId == player.Key);
            if (currentPanel != null) {
                currentPanel.SetReady(player.Value.IsReady);
            }
            else {
                var panel = Instantiate(_playerPanelPrefab, _playerPanelParent);
                panel.Init(player.Key, player.Value);
                _playerPanels.Add(panel);
            }
        }
        allReady = players.All(p => p.Value.IsReady);

        if (allReady && players.Count == 2)
            starting = (starting == default) ? StartCoroutine(timer()) : default;
        else if (starting != default) StopCoroutine(starting);
    }

    private void OnCurrentLobbyRefreshed(Lobby lobby) {
        lobbyId = lobby.Id;
        _waitingText.text = (lobby.Players.Count < 2) ? $"Waiting second player" : (allReady) ? "Waiting for start" : "Waiting for ready";
        string password = lobby.Data[Constants.PasswordKey].Value;
        _passwordField.gameObject.SetActive(password != "");
        _passwordField.text = $"Password: {password}";
    }
    public void OnReadyClicked()
    {
        isReady = !isReady;
    }
    IEnumerator timer()
    {
        yield return new WaitForSeconds(_startingTime);
        StartPressed?.Invoke(lobbyId);
    }
}