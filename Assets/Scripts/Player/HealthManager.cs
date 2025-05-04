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
    bool dead = false;
    Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
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
        anim.SetTrigger("Death");
        SoundPlayer.Play.Invoke(DeathSound);
        if (character.name == "Skeleton" && !dead)
        {
            yield return new WaitForSeconds(4);
            if (IsOwner) InputManager.Input.Enable();
            anim.SetBool("IsDeath", true);
            curentHealth = character.health / 2;
            healthSlider.value = curentHealth;
            dead = true;
            Debug.Log("dwad");
        }
        else
        {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            rb.gravityScale = 0;
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0;
            GetComponent<Collider2D>().enabled = false;
            yield return new WaitForSeconds(5);
            if (IsOwner) GameManager.OnGameOver.Invoke(false);
            else GameManager.OnGameOver.Invoke(true);
        }
    }
}
