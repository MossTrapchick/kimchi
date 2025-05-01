using TMPro;
using UnityEngine;

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
}
