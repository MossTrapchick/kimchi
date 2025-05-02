using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Attack : NetworkBehaviour
{
    [SerializeField] int Damage;
    [SerializeField] float AttackCooldown, WaitBeforeAttack;
    [SerializeField] Image CooldownUI;
    [SerializeField] HealthManager enemy;
    [SerializeField] Animator anim;
    Coroutine Cooldown;
    private void Start()
    {
        if (NetworkManager.LocalClientId == OwnerClientId) CooldownUI = GameManager.Instance.GetAttackUI;
        if (!IsOwner) return;
        InputManager.Input.Player.Attack.performed += startAttack;
    }
    private void OnDisable()
    {
        if (!IsOwner) return;
        InputManager.Input.Player.Attack.performed -= startAttack;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out HealthManager health) && collision.transform != transform.parent.parent)
            enemy = health;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out HealthManager health) && collision.transform != transform.parent.parent)
            enemy = default;
    }
    void startAttack(InputAction.CallbackContext ctx)
    {
        if (Cooldown == default)
        {
            StartCoroutine(attack());
            Cooldown = StartCoroutine(cooldown());
        }
        else Debug.Log("Attack in cooldown");
    }
    IEnumerator attack()
    {
        Debug.Log("Prepare for attack");
        anim.SetTrigger("Attack");
        yield return new WaitForSeconds(WaitBeforeAttack);
        if (enemy != default)
        {
            enemy.TakeDamage(Damage);
            Debug.Log("enemy attacked");
        }
        else Debug.Log("miss");
    }
    IEnumerator cooldown()
    {
        float t = 0;
        while(t < AttackCooldown)
        {
            t+= Time.deltaTime;
            CooldownUI.fillAmount = Mathf.InverseLerp(0, AttackCooldown, t);
            yield return new WaitForEndOfFrame();
        }
        Cooldown = default;
    }
}
