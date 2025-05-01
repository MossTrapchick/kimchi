using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CreateLobbyScreen : MonoBehaviour {
    [SerializeField] private TMP_InputField _passwordInput;

    public static UnityEvent<LobbyData> LobbyCreated = new();

    public void OnCreateClicked() {
        var lobbyData = new LobbyData {
            Name = "None",/// name of player
            Password = _passwordInput.text
        };

        LobbyCreated?.Invoke(lobbyData);
    }
    public void OnClickPassword(string value)
    {
        if(value == "")
            _passwordInput.text = String.Join("", new List<int>() { 0, 0, 0, 0 }
            .Select(digit => UnityEngine.Random.Range(0, 9)));
    }
}

public struct LobbyData {
    public string Name;
    public string Password;
}