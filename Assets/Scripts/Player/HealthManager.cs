using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Unity.Netcode;

public class HealthManager : NetworkBehaviour
{
    private int curentHealth;
    Character character;
    Slider healthSlider;

    private void Start()
    {
        healthSlider = GameManager.Instance.GetSlider(OwnerClientId);
        character = GameManager.Instance.MyCharacter(OwnerClientId);
        curentHealth = character.health;
        healthSlider.maxValue = character.health;
        healthSlider.value = character.health;
    }

    public void TakeDamage(int damage) => addDamageRpc(damage,OwnerClientId);

    [Rpc(SendTo.Everyone)]
    private void addDamageRpc(int damage, ulong id)
    {
        if (id != OwnerClientId) return;
        curentHealth -= damage;
        healthSlider.value = curentHealth;
    }
}
