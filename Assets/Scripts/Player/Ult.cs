using System.Collections;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Ult : NetworkBehaviour
{
    [SerializeField] Transform spawnPos;
    [SerializeField] GameObject apple;
    [SerializeField] float AttackCooldown, WaitBeforeAttack;
    [SerializeField] Animator anim;
    Character character;
    Image CooldownUI;
    Coroutine Cooldown;
    private void Start()
    {
        character = GameManager.Instance.MyCharacter(OwnerClientId);
        if (!IsOwner) return;
        CooldownUI = GameManager.Instance.GetUltUI;
        CooldownUI.transform.parent.gameObject.SetActive(true);
        InputManager.Input.Player.Ultimate.performed += DoUlt;
    }
    void DoUlt(InputAction.CallbackContext ctx)
    {
        if (Cooldown == default)
        {
            ActivateRpc(OwnerClientId);
            Cooldown = StartCoroutine(cooldown());
        }
        else Debug.Log("Attack in cooldown");
    }
    [Rpc(SendTo.Everyone)]
    public void ActivateRpc(ulong owner)
    {
        if (OwnerClientId != owner) return;
        StartCoroutine(throwApple(owner));
    }
    IEnumerator throwApple(ulong owner)
    {
        anim.SetTrigger("Ult");
        yield return new WaitForSeconds(WaitBeforeAttack);
        if (anim.GetBool("IsStaned")) yield break;
        if (IsServer)
        {
            GameObject obj = Instantiate(apple, spawnPos.position, Quaternion.identity);
            obj.transform.localScale = transform.GetChild(0).localScale;
            obj.GetComponent<NetworkObject>().SpawnWithOwnership(owner);
            Stunner stun = obj.GetComponent<Stunner>();
            stun.Damage = character.ultDamage;
            stun.stunTime = character.stunTime;
        }
    }
    private void OnDisable()
    {
        InputManager.Input.Player.Ultimate.performed -= DoUlt;
    }
    IEnumerator cooldown()
    {
        float t = 0;
        while (t < AttackCooldown)
        {
            t += Time.deltaTime;
            CooldownUI.fillAmount = 1 - Mathf.InverseLerp(0, AttackCooldown, t);
            yield return new WaitForEndOfFrame();
        }
        Cooldown = default;
    }
}
