using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Скорости")]
    public float speed = 5f;
    public float chaseSpeed = 3f;
    [Range(0, 1)] public float knockbackResistance = 0.5f; 

    [Header("Атака")]
    [SerializeField] private float attackDamage = 10f;
    [SerializeField] private float attackCooldown = 1f;

    [Header("Настройки скорости")]
    public float originalSpeed { get; private set; }
    public float originalChaseSpeed { get; private set; }
    private float lastAttackTime;
    private Transform target;
    private bool isAttacking;
    private Rigidbody2D rb;
    float minDistanceToPlayer = 0.3f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        originalSpeed = speed;
        originalChaseSpeed = chaseSpeed;
    }

    private void FixedUpdate()
    {
        if (target != null)
        {
            Vector2 toPlayer = target.position - transform.position;
            if (toPlayer.magnitude > minDistanceToPlayer)
            {
                Vector2 direction = toPlayer.normalized;
                rb.linearVelocity = direction * chaseSpeed;
            }
            else
            {
                rb.linearVelocity = Vector2.zero;
            }
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }



    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isAttacking = true;
            AttackPlayer(other.gameObject);
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (isAttacking && other.gameObject.CompareTag("Player"))
        {
            AttackPlayer(other.gameObject);
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isAttacking = false;
        }
    }

    private void AttackPlayer(GameObject player)
    {
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            player.GetComponent<PlayerHealth>().UpdateHealth(-attackDamage);
            lastAttackTime = Time.time;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            target = other.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            target = null;
        }
    }
}