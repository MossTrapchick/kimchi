using UnityEngine;
public class AuthenticationManager : MonoBehaviour {
    [SerializeField] MenuController menuController;
    [SerializeField] GameObject LobbyWindow;
    public async void LoginAnonymously() {
        await Authentication.Login();
        menuController.ToggleWindow(LobbyWindow);
    }
}