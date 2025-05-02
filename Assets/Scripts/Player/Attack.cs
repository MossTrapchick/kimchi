using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Attack : NetworkBehaviour
{
    [SerializeField] float AttackCooldown, WaitBeforeAttack;
    [SerializeField] Image CooldownUI;
    [SerializeField] Movement enemy;
    [SerializeField] Animator anim;
    Coroutine Cooldown;
    private void Start()
    {
        InputManager.Input.Player.Attack.performed += startAttack;
    }
    private void OnDisable()
    {
        InputManager.Input.Player.Attack.performed -= startAttack;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.TryGetComponent(out enemy);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out enemy)) enemy = default;
    }
    void startAttack(InputAction.CallbackContext ctx)
    {
        if (!IsOwner)
        {
            return;
        }
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
            enemy.gameObject.SetActive(false);
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
