using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [SerializeField] TMP_InputField nameField;
    [SerializeField] GameObject buttons;
    private void Start()
    {
        nameField.text = PlayerPrefs.HasKey("Name") ? PlayerPrefs.GetString("Name") : "Player";
    }
    public void Exit()
    {
        Application.Quit();
    }
    public void ToggleWindow(GameObject window)
    {
        bool active = !window.activeSelf;
        window.SetActive(active);
        buttons.SetActive(!active);
    }
    public void SavePlayerName(string name)
    {
        PlayerPrefs.SetString("Name", name);
    }

    public void SceneChanger(string name)
    {
        SceneManager.LoadScene(name);
    }

    public async void CloseServer()
    {
        NetworkManager.Singleton.Shutdown();
        await MatchmakingService.LeaveLobby();
    }
}
