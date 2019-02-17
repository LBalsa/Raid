using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

#region RequireComponent
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(LookAt))]
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
#endregion

[SelectionBase]
public class Enemy : MonoBehaviour, IDestructable
{
    #region Stats
    [Header("Stats")]
    [SerializeField]
    protected float maxHealth = 4;
    protected float health;
    [SerializeField]
    protected float damage = 1;
    [SerializeField]
    protected float attackCooldown = 2.5f;
    protected float attackTimer;
    protected float recoverTimer = 2f;
    protected bool blocking = false;
    [SerializeField]
    protected float maxdistance = 2;
    public bool retreats = true;
    public GameObject weaponPrefab;
    public Transform weaponSlot;
    #endregion

    [Header("Nav")]
    protected float distance;
    [SerializeField]
    protected float moveSpeed = 2.5f;
    [SerializeField]
    protected float rotationSpeed = 180f;
    protected PlayerController target;

    #region Components
    protected Animator anim;
    protected AudioSource aus;
    protected LookAt lookAt;
    protected NavMeshAgent nav;
    protected Rigidbody rb;
    protected Weapon fist;
    protected MainWeapon mainWeapon;

    [HideInInspector]
    public EnemySpawner spawner;
    public FXStructure fX;
    public LootTable lootTable;
    #endregion

    protected Vector2 smoothDeltaPosition = Vector2.zero;
    protected Vector2 velocity = Vector2.zero;

    // States
    protected enum AIState { idle, moving, circling, attacking, blocking, recovering, dead }
    [SerializeField]
    protected AIState state;
    protected AIState stateCtrl;

    protected bool beenHit = false;
    protected bool isBlocking = false;
    protected bool isRetreating = false;
    protected int hits = 0;
    public int maxHitsBlocked = 1;
    protected float timer;


    // Use this for initialization
    void Start()
    {
        Initialise();

        if (!mainWeapon)
        {
            if (weaponPrefab)
            {
                mainWeapon = Instantiate(weaponPrefab, new Vector3(0, 0, 0), Quaternion.identity, weaponSlot).GetComponent<MainWeapon>();
            }
            else
            {
                fist = GetComponentInChildren<Weapon>();
            }
        }
        mainWeapon.SetUp(false, false, damage);
        mainWeapon.Disable();
    }

    protected void Initialise()
    {
        health = maxHealth;
        target = PlayerController.inst;
        state = AIState.idle;
        timer = 0;
        anim = GetComponent<Animator>();
        aus = GetComponent<AudioSource>();
        lookAt = GetComponent<LookAt>();

        nav = GetComponent<NavMeshAgent>();
        nav.speed = moveSpeed;
        nav.autoBraking = true;
        nav.updatePosition = false;
        nav.stoppingDistance = maxdistance;

        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        PlaySound(fX.swosh);

        // Rotate towards target.
        var lookPos = target.transform.position;
        lookPos.y = transform.position.y;
        transform.LookAt(lookPos);
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        // Set player as target.
        if (target)
        {
            distance = Vector3.Distance(transform.position, new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z));
        }
        else
        {
            target = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        }

        if (state != AIState.dead)
        {
            nav.SetDestination(target.transform.position);
            UpdateAnimator();
        }

        switch (state)
        {
            case AIState.idle:
                nav.destination = target.transform.position;
                ChangeState(AIState.moving);
                break;

            case AIState.moving:
                Moving();
                break;
            case AIState.circling:
                Cirling();
                break;
            case AIState.attacking:
                Attacking();
                break;
            case AIState.blocking:
                Blocking();
                break;
            case AIState.recovering:
                Recovering();
                break;
            case AIState.dead:
                break;
            default:
                break;
        }


        if (lookAt) lookAt.lookAtTargetPosition = nav.steeringTarget + transform.forward;
    }

    void UpdateAnimator()
    {
        Vector3 worldDeltaPosition = nav.nextPosition - transform.position;

        float dx = Vector3.Dot(transform.right, worldDeltaPosition);
        float dy = Vector3.Dot(transform.forward, worldDeltaPosition);
        Vector2 deltaPosition = new Vector2(dx, dy);

        // Low-pass filter the deltaMove
        float smooth = Mathf.Min(1.0f, Time.deltaTime / 0.15f);
        smoothDeltaPosition = Vector2.Lerp(smoothDeltaPosition, deltaPosition, smooth);

        // Update velocity if time advances
        if (Time.deltaTime > 1e-5f)
            velocity = smoothDeltaPosition / Time.deltaTime;

        //bool shouldMove = velocity.magnitude > 0.5f && nav.remainingDistance > nav.radius;

        // Update animation parameters
        anim.SetFloat("Strafe", velocity.x);
        anim.SetFloat("Forward", velocity.y);

        // Pull agent towards character or vice versa
        if (worldDeltaPosition.magnitude > nav.radius)
        {
            if (isRetreating)
            {
                nav.Warp(anim.transform.position);
            }
            else
                nav.nextPosition = transform.position + 0.2f * worldDeltaPosition;
            //transform.position = nav.nextPosition - 0.9f * worldDeltaPosition;
        }
    }

    void OnAnimatorMove()
    {
        // Update position to agent position
        //transform.position = nav.nextPosition;

        if (anim)
        {
            Vector3 position = anim.rootPosition;
            position.y = nav.nextPosition.y;
            transform.position = position;
        }
    }

    protected void Retreat(bool on)
    {
        anim.SetBool("Retreate", on);
        isRetreating = on;
    }

    protected virtual void ChangeState(AIState s)
    {
        stateCtrl = state;
        state = s;
    }

    protected virtual void ChangeStateCircle()
    {
        stateCtrl = state;
        state = AIState.circling;
    }

    protected virtual void Moving()
    {
        if (mainWeapon)
        {
            mainWeapon.Disable();
        }
        nav.isStopped = false;
        if (stateCtrl != AIState.circling) { nav.stoppingDistance = maxdistance; }

        if (distance <= maxdistance) { ChangeState(AIState.circling); }
    }

    protected virtual void Cirling()
    {
        // State entry.
        if (stateCtrl != AIState.circling)
        {
            if (mainWeapon)
            {
                mainWeapon.Disable();
            }            //nav.speed = moveSpeed;
            ChangeState(AIState.circling);
            attackTimer = attackCooldown;
        }

        //// Check for player attacks.
        //CheckForAttacks();
        Block();

        // If path behind is blocked.
        if (isRetreating && Physics.Raycast(transform.position, -transform.forward, 0.4f))
        {
            Retreat(false);
        }
        // If player goes out of range.
        else if (distance > maxdistance * 2f)
        {
            Retreat(false);
            ChangeState(AIState.moving);
        }
        // If player moves away.
        else if (distance > maxdistance * 1.35f)
        {
            Retreat(false);
            nav.destination = target.transform.position;
        }
        // End retreat.
        else if (distance <= maxdistance * 1.25 && distance > maxdistance * .75 && isRetreating)
        {
            nav.Warp(anim.transform.position);
            Retreat(false);
        }
        // Retreat if player is too close.
        if (distance < maxdistance * 0.6f && !isRetreating && retreats)
        {
            // Rotate to face player.
            var lookPos = target.transform.position - transform.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 0.4f);

            // Check if can retreat.
            if (!Physics.Raycast(transform.position, -transform.forward, 0.4f))
            {
                Retreat(true);
            }
        }

        AttackCountDown();



    }

    protected virtual void StateExit(AIState state)
    {

    }

    protected void AttackCountDown()
    {
        // Countdown to attack.
        if (attackTimer > 0)
            attackTimer -= Time.deltaTime;
        else if (attackTimer < 0)
            attackTimer = 0;
        else if (attackTimer == 0 && !blocking)
        {
            attackTimer -= Time.deltaTime;
            Retreat(false);
            //BlockEnd();
            ChangeState(AIState.attacking);
        }
    }

    //protected void CheckForAttacks()
    //{
    //    if (target.attacking && !blocking && Random.Range(0, 100) <= (int)health)
    //    {
    //        BlockStart();
    //    }
    //}

    protected virtual void Attacking()
    {
        Vector3 dir = (target.transform.position - transform.position).normalized;
        //float direction = Vector3.Dot(dir, transform.forward);


        //if (target.GetComponent<PlayerController>().busy && stateCtrl != AIState.attacking)
        //{
        //    //if (Random.Range(0, 20) == 1) { anim.SetTrigger("Taunt"); }
        //    attackTimer = 2;
        //    ChangeState(AIState.circling);
        //    return;
        //}

        //ChangeState(AIState.attacking);
        //target.GetComponent<PlayerController>().busy = true;

        nav.stoppingDistance = maxdistance / 2;

        if (distance > maxdistance * 2)
        {
            ChangeState(AIState.moving);
        }
        else if (distance > maxdistance / 2)
        {
            //Move towards target
            //myTransform.position += myTransform.forward * moveSpeed * Time.deltaTime;
        }
        else if (distance <= maxdistance / 2)
        {
            fX.sfx_attack.Play(aus);
            int x = Random.Range(1, 4);
            anim.SetTrigger("Light" + x);

            mainWeapon.Enable();
            ChangeState(AIState.recovering);
            //Invoke("AttackEnd", 1.5f);
        }

        // transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(target.transform.position - transform.position), rotationSpeed * Time.deltaTime);

    }
    protected virtual void Blocking()
    {
        // State entry;
        if (stateCtrl != AIState.blocking)
        {
            hits = 0;
            anim.SetBool("Block", true);
            timer = 3f;
            ChangeState(AIState.blocking);
        }

        // Countdown to stop.

        if (timer > 0)
            timer -= Time.deltaTime;
        else if (timer <= 0)
        {
            anim.SetBool("Block", false);
            ChangeState(AIState.circling);
        }

    }

    protected virtual void Block()
    {
        if (!blocking)
        {
            return;
        }
        // Countdown to stop.

        if (timer > 0)
            timer -= Time.deltaTime;
        else if (timer <= 0)
        {
            BlockEnd();
        }
    }

    protected virtual void BlockStart()
    {
        Debug.Log("block start");
        hits = 0;
        anim.SetLayerWeight(1, 1);

        anim.SetBool("isBlocking", true);
        timer = 2f;
        blocking = true;
    }

    protected virtual void BlockEnd()
    {
        Debug.Log("block end");

        anim.SetLayerWeight(1, 0);
        anim.SetBool("isBlocking", false);
        blocking = false;
    }

    protected virtual void Recovering()
    {
        /*
        if (stateCtrl != AIState.recovering)
        {
            recoverTimer = 2f;
        }
        if (recoverTimer > 0)
        {
            recoverTimer -= Time.deltaTime;
        }
        else if (recoverTimer <= 0)
        {
            ChangeState(AIState.circling);
        }
        */
    }

    //protected virtual void AttackEnd()
    //{
    //    target.GetComponent<PlayerController>().busy = false;
    //    nav.stoppingDistance = maxdistance;
    //    if (mainWeapon)
    //    {
    //        mainWeapon.Disable();
    //    }
    //    ChangeState(AIState.circling);
    //}

    // Other damage.
    public void TakeDamage(float dmg)
    {
        if (mainWeapon)
        {
            mainWeapon.Disable();
        }
        if (state != AIState.dead && health > 0)
        {
            fX.sfx_hurt.Play(aus);
            anim.SetTrigger("Hit");
            CalculateDamage(dmg);
        }
    }

    // Player damage.
    public void TakeDamage(float damage, Collision collision)
    {
        // Return if already been hit in last half second;
        // Used to prevent double hit from single attack.
        if (beenHit) { return; }

        beenHit = true;
        Invoke("ResetHit", 0.5f);
        if (mainWeapon)
        {
            mainWeapon.Disable();
        }

        if (health > 0)
        {
            // If blocking.
            if (blocking && hits < maxHitsBlocked)
            {
                anim.SetTrigger("Block");
                Instantiate(fX.vfx_block, collision.contacts[0].point, Quaternion.identity);
                fX.sfx_block.Play(aus);
                //CalculateDamage(damage / 2);
            }
            else
            {
                BlockEnd();
                Instantiate(fX.vfx_hurt, collision.contacts[0].point, Quaternion.identity);
                fX.sfx_hurt.Play(aus);
                anim.SetTrigger("Hit");
                CalculateDamage(damage);
            }
        }

    }

    protected void ResetHit()
    {
        beenHit = false;
    }

    protected virtual void CalculateDamage(float dmg)
    {
        health -= dmg;

        //if (state == AIState.attacking || stateCtrl == AIState.attacking)
        //{
        //    target.GetComponent<PlayerController>().busy = false;
        //}

        if (health <= 0)
        {
            StartCoroutine("Die");
            return;
        }
        else
        {
            Invoke("ChangeStateCircle", 1);
        }
    }

    public void UpdateHealthBar(int adj)
    {
        health += adj;

        if (health < 0)
            health = 0;

        if (health > maxHealth)
            health = maxHealth;

        if (maxHealth < 1)
            maxHealth = 1;

        //healthBarLength = ( * (health / (float)maxHealth);
    }

    protected IEnumerator Die()
    {
        // Dissable essential components.
        GetComponent<CapsuleCollider>().enabled = false;

        if (mainWeapon) { mainWeapon.Drop(); }
        Retreat(false);
        nav.isStopped = true;
        lootTable.DropLoot((int)maxHealth,transform.position);
        // Update state and report death.
        if (state != AIState.dead && spawner)
        {
            spawner.EnemyDown();
        }
        ChangeState(AIState.dead);

        anim.SetTrigger("Dead");
        fX.sfx_die.Play(aus);

        // Special effects delay.
        yield return new WaitForSeconds(4);
        Instantiate(fX.vfx_despawn, transform.position, Quaternion.identity);
        PlaySound(fX.swosh);
        // Body disappearance delay.
        yield return new WaitForSeconds(0.4f);
        Destroy(this.gameObject);
    }

    protected void PlaySound(AudioClip clip)
    {
        if (aus && clip)
        {
            aus.pitch = 1;
            aus.volume = 1;
            aus.clip = clip;
            aus.Play();
        }
    }


}

