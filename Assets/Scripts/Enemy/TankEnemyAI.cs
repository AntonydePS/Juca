using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class TankEnemyAI : MonoBehaviour
{
    public GameObject shield;

    public GameObject enableWeapon;

    public bool playerInRange;

    public GameObject hitFX;

    public float startHP;

    private float health;

    public GameObject Ragdoll;

    public Transform player;

    public Transform bridge;

    private NavMeshAgent mAgent;

    private Animator mAnimator;

    public GameObject Player;

    public GameObject Bridge;

    public float EnemyDistanceRun = 4.0f;

    private bool mIsDead = false;

    public GameObject[] ItemsDeadState = null;

    public Image HPBar;


    void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        Bridge = GameObject.FindGameObjectWithTag("Bridge");
        player = GameObject.FindGameObjectWithTag("Player").transform;
        bridge = GameObject.FindGameObjectWithTag("Bridge").transform;
        mAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }


    // Use this for initialization
    void Start()
    {
        playerInRange = false;

        enableWeapon.GetComponent<DamageSource>().enabled = false;

        health = startHP;

        mAgent = GetComponent<NavMeshAgent>();

        mAnimator = GetComponent<Animator>();
    }

    private bool IsNavMeshMoving
    {
        get
        { 
            return mAgent.velocity.magnitude > 0.1f;
        }
    }

    // TakeDamage
    void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.name == "Punch")
        {
            {
                if (Player.GetComponent<PlayerController>().IsAttacking)
                {
                    GameObject GC = GameObject.FindGameObjectWithTag("GameController");
                    GC.SendMessage("PlayTankHit");
                    HPBar.fillAmount = health / startHP;
                    health -= 10;
                    Instantiate(hitFX, transform.position, transform.rotation);
                    mAnimator.SetTrigger("hit");

                }
            }
        }
        coll.GetComponent<Collider>().gameObject.GetComponent<InteractableItemBase>();
    }

    void RangeCheck()
    {
        if (player)
        {
            float distance = Vector3.Distance(player.position, transform.position);
            playerInRange = (distance < 10);
        }
    }


    void ShowItemsDeadState()
    {
        // Activate the items
        foreach(var item in ItemsDeadState)
        {
            item.SetActive(true);
        }
    }

    void Die()
    {
        mIsDead = true;
        mAgent.enabled = false;
        mAnimator.SetTrigger("die");
        Invoke("ShowItemsDeadState", 1);
        Instantiate(Ragdoll, transform.position, transform.rotation);
        Destroy(gameObject);
    }


    void FixedUpdate()
    {
        RangeCheck();

        if (health <= 0)
        {
            Die();
        }

        if (playerInRange == true)
        {
            Attack();
        }
    }


    void Attack()
    {
        enableWeapon.GetComponent<DamageSource>().enabled = true;
        mAnimator.SetTrigger("attack");
    }


    // Update is called once per frame
    void Update()
    {

        if (mIsDead)
            return;

        if (playerInRange == true)
        { 
            float squaredDist = (transform.position + Player.transform.position).sqrMagnitude;
            float EnemyDistanceRunSqrt = EnemyDistanceRun * EnemyDistanceRun;
            mAgent.SetDestination(player.position);
        }

        else if (playerInRange == false)
        {
            float squaredDist = (transform.position + Bridge.transform.position).sqrMagnitude;
            float EnemyDistanceRunSqrt = EnemyDistanceRun * EnemyDistanceRun;
            mAgent.SetDestination(bridge.position);
        }

        mAnimator.SetBool("walk", IsNavMeshMoving);

    }
}
