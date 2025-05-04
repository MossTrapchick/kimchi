using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Unity.Netcode;

public class HealthManager : NetworkBehaviour
{
    [SerializeField] AudioClip DeathSound;
    private int curentHealth;
    Character character;
    Slider healthSlider;

    private void Start()
    {
        healthSlider = GameManager.Instance.GetSlider(OwnerClientId);
        if (IsOwner) healthSlider.transform.GetChild(0).GetComponent<Image>().color = Color.green;
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
        if (curentHealth <= 0) StartCoroutine(Death());
    }
    IEnumerator Death()
    {
        if (IsOwner) InputManager.Input.Disable();
        SoundPlayer.Play.Invoke(DeathSound);
        GetComponent<Rigidbody2D>().gravityScale = 0;
        GetComponent<Collider2D>().enabled = false;
        GetComponent<Animator>().SetTrigger("Death");
        yield return new WaitForSeconds(10);
        if (IsOwner) GameManager.OnGameOver.Invoke(false);
        else GameManager.OnGameOver.Invoke(true);
    }
}
