using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Unity.Netcode;

public class HealthManager : NetworkBehaviour
{
    private int curentHealth;

    [SerializeField] Players player;

    [SerializeField] Slider mainSlider;

    private void Start()
    {
        mainSlider = GameManager.Instance.GetSlider(OwnerClientId);
        curentHealth = player.health;
        mainSlider.maxValue = player.health;
        mainSlider.value = player.health;
    }

    public void TakeDamage(int damage) => addDamageRpc(damage,OwnerClientId);

    [Rpc(SendTo.Everyone)]
    private void addDamageRpc(int damage, ulong id)
    {
        if (id != OwnerClientId) return;
        curentHealth -= damage;
        mainSlider.value = curentHealth;
    }
}
