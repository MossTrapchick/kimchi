using Unity.Netcode;
using UnityEngine;

public class Stunner : NetworkBehaviour
{
    [SerializeField] bool isThorw = true;
    [SerializeField] GameObject birdsPrefab;
    [SerializeField] bool disableOnStrike = true;
    [SerializeField] ParticleSystem particles;
    [SerializeField] float stunTime = 2;
    SpriteRenderer spriter;
    Rigidbody2D rb;
    public int Damage = 0;
    private void Start()
    {
        spriter = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        if (isThorw) Throw();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Movement movement))
        {
            if (movement.OwnerClientId == OwnerClientId) return;
            if(birdsPrefab!= default)
            {
                Vector3 pos = collision.transform.position;
                pos.y += 4;
                GameObject bird = Instantiate(birdsPrefab, pos, Quaternion.identity);
                Destroy(bird, stunTime);
            }
            movement.Stun(stunTime, movement.OwnerClientId);
            if (Damage > 0) collision.GetComponent<HealthManager>().TakeDamage(Damage);
            if (disableOnStrike) Break();
        }
        else if (disableOnStrike && collision.CompareTag("Ground"))
            Break();

    }
    void Break()
    {
        rb.gravityScale = 0;
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0;
        particles.Play();
        spriter.enabled = false;
        if(IsServer) Destroy(gameObject, 1f);
    }
    public void Throw()
    {
        rb.AddForce(new Vector3(transform.localScale.x > 0 ? 10 : -10, 2.5f, 0), ForceMode2D.Impulse);
    }
}
