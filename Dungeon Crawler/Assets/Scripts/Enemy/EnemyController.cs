using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public delegate void OnEnemyAction(GameObject enemy);
    public OnEnemyAction OnEnemyDeath;

    public enum States
    {
        Follow,
        Hit,
        Stunned,
        Dead,
        allStates
    }

    [Header("General Settings")]
    public GameObject player;
    public float speed;
    public int maxHealth;

    [Header("Animation Settings")]
    public Animator animator;

    [Header("Stun Settings")]
    public int normalMass;
    public int stunMass;
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
    public States currentState;
    public States lastState;
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
        if(GameManager.Get())
        {
            player = GameManager.Get().player;
        }
        
        playerController = player.GetComponent<PlayerController>();
        currentBox = GetComponentInChildren<EnemyDamageBox>();
        currentBox.OnBoxEnterCollider += StartDamage;
        currentBox.OnBoxExitCollider += StopDamage;

        health = maxHealth;
    }

    private void Update()
    {
        switch (currentState)
        {   
            case States.Follow:
                MovementUpdate();
                break;
            case States.Hit:
                DamageUpdate();
                break;
            case States.Stunned:
                StunUpdate();
                break;
            case States.Dead:
                DeathUpdate();
                break;
            default:
                break;
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

    private void StunUpdate()
    {
        if (isStunned)
        {
            stunTimer += Time.deltaTime;

            if (stunTimer >= stunTime)
            {
                // si estabas atacando ataca, o sino run
                if (isActive)
                {
                    if(!isDead)
                    {
                        ChangeState(States.Hit);
                    }
                }
                else
                {
                    if (!isDead)
                    {
                        ChangeState(States.Follow);
                    }
                }

                rig.mass = normalMass;
                isStunned = false;
                stunTimer = 0;
            }
        }
    }

    public void DeathUpdate()
    {
        deathTimer += Time.deltaTime;

        if (deathTimer >= deathTime)
        {
            DestroyAfterAnimation();
        }
    }

    public void ReceiveDamage(float amount)
    {
        health -= amount;
        if (health <= 0)
        {
            health = 0;
            ChangeState(States.Dead);
            Die();
            isDead = true;
        }
        else
        {
            if (canBeStunned)
            {
                ChangeState(States.Stunned);
            }
        }
    }

    public void Die()
    {
        Debug.Log("dead");
        hitboxCollider.enabled = false;
        rig.useGravity = false;

        if (OnEnemyDeath != null)
        {
            OnEnemyDeath(gameObject);
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
            if(!isStunned && !isDead)
            {
                //animator.SetTrigger("Hit");
                ChangeState(States.Hit);
            }

            fireRateTimer = 0;
            isActive = true;
        }
    }

    public void StopDamage(string tag)
    {
        if (tag == "player")
        {
            if(!isStunned && !isDead)
            {
                //animator.SetTrigger("Run");
                ChangeState(States.Follow);
            }
            
            fireRateTimer = 0;
            isActive = false;
        }
    }

    public void ChangeState(States newState)
    {
        lastState = currentState;
        currentState = newState;

        switch (currentState)
        {
            case States.Follow:
                //animator.SetTrigger("Run");
                animator.SetBool("Stun", false);
                animator.SetBool("Follow", true);
                animator.SetBool("Hit", false);
                break;
            case States.Hit:
                //animator.SetTrigger("Hit");
                animator.SetBool("Stun", false);
                animator.SetBool("Follow", false);
                animator.SetBool("Hit", true);
                break;
            case States.Stunned:
                //animator.SetTrigger("Stun");
                animator.SetBool("Stun", true);
                animator.SetBool("Follow", false);
                animator.SetBool("Hit", false);
                isStunned = true;
                rig.mass = stunMass;
                break;
            case States.Dead:
                animator.SetTrigger("Die");
                animator.SetBool("Stun", false);
                animator.SetBool("Follow", false);
                animator.SetBool("Hit", false);
                break;
            default:
                break;
        }
    }

    private void OnDestroy()
    {
        currentBox.OnBoxEnterCollider -= StartDamage;
        currentBox.OnBoxExitCollider -= StopDamage;
    }
}
