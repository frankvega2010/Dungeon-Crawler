using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public delegate void OnEnemyAction(GameObject enemy);
    public OnEnemyAction OnEnemyDeath;

    [Header("General Settings")]
    public GameObject player;
    public float speed;
    public int maxHealth;

    /*[Header("Spawn Settings")]
    public bool isInGroup;*/

    [Header("Animation Settings")]
    public Animator animator;

    [Header("Stun Settings")]
    public bool canBeStunned;
    public float stunTime;
    public bool isStunned;
    private float stunTimer;

    [Header("Damage Settings")]
    public float damage;
    public float fireRate;
    private bool isActive;
    private bool canDamage;
    private float fireRateTimer;

    [Header("Distance Settings")]
    public float stopDistance;

    [Header("Death Settings")]
    public float deathTime;
    public float deathTimer;

    [Header("Check Variables")]
    public float health;
    public bool isDead;
    public CapsuleCollider hitboxCollider;
    public Rigidbody rig;
    public PlayerController playerController;
    public EnemyDamageBox currentBox;

    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody>();
        hitboxCollider = GetComponent<CapsuleCollider>();
        animator = GetComponentInChildren<Animator>();
        player = GameManager.Get().player;
        playerController = player.GetComponent<PlayerController>();
        currentBox = GetComponentInChildren<EnemyDamageBox>();
        currentBox.OnBoxEnterCollider += StartDamage;
        currentBox.OnBoxExitCollider += StopDamage;

        health = maxHealth;
    }

    private void Update()
    {
        if(!isDead)
        {
            if (isStunned)
            {
                stunTimer += Time.deltaTime;

                if (stunTimer >= stunTime)
                {
                    // si estabas atacando ataca, o sino run
                    if(isActive)
                    {
                        animator.SetTrigger("Hit");
                    }
                    else
                    {
                        animator.SetTrigger("Run");
                    }
                    
                    isStunned = false;
                    stunTimer = 0;
                }
            }
            else
            {
                MovementUpdate();
                DamageUpdate();
            }
        }
        else
        {
            deathTimer += Time.deltaTime;

            if(deathTimer >= deathTime)
            {
                DestroyAfterAnimation();
            }
        }
    }

    public void MovementUpdate()
    {
        Vector3 lookPos = player.transform.position - transform.position;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = rotation;

        float distance = Vector3.Distance(transform.position, player.transform.position);

        if (distance >= stopDistance)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        }
    }

    public void DamageUpdate()
    {
        if (isActive)
        {
            if(!isStunned)
            {
                fireRateTimer += Time.deltaTime;

                if (fireRateTimer >= fireRate)
                {
                    fireRateTimer = 0;
                    canDamage = true;
                }

                if (canDamage)
                {
                    Debug.Log("Damage Given to player : " + damage);

                    playerController.ReceiveDamage(damage);
                    //Make damage to player when the box collider hits it.

                    canDamage = false;
                }
            }
        }
    }

    public void ReceiveDamage(float amount)
    {
        health -= amount;
        if (health <= 0)
        {
            health = 0;
            Debug.Log("dead");
            // Execute things to avoid taking damage
            hitboxCollider.enabled = false;
            rig.useGravity = false;
            animator.SetTrigger("Die");
            isDead = true;

            if(OnEnemyDeath != null)
            {
                OnEnemyDeath(gameObject);
            }
        }
        else
        {
            if (canBeStunned)
            {
                animator.SetTrigger("Stun");
                isStunned = true;
            }
        }
        
    }

    public void DestroyAfterAnimation()
    {
        Destroy(gameObject);
    }

    public void StartDamage(string tag)
    {
        if (tag == "player")
        {
            if(!isStunned)
            {
                animator.SetTrigger("Hit");
            }
            
            fireRateTimer = 0; // Maybe change it to zero to avoid instant damage?
            isActive = true;
        }
    }

    public void StopDamage(string tag)
    {
        if (tag == "player")
        {
            if(!isStunned)
            {
                animator.SetTrigger("Run");
            }
            
            fireRateTimer = 0;
            isActive = false;
        }
    }

    private void OnDestroy()
    {
        currentBox.OnBoxEnterCollider -= StartDamage;
        currentBox.OnBoxExitCollider -= StopDamage;
    }
}
