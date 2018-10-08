using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyAI : MonoBehaviour
{
    public bool invincible;

    public GameObject shield;

    public GameObject enableWeapon;

    public bool playerInRange;

    public GameObject hitFX;

    public float startHP;

    private float health;

    public GameObject Ragdoll;

    public Transform player;

    public Transform portal;

    private NavMeshAgent mAgent;

    private Animator mAnimator;

    public GameObject Player;

    public GameObject Portal;

    public float EnemyDistanceRun = 4.0f;

    private bool mIsDead = false;

    public GameObject[] ItemsDeadState = null;

    public Image HPBar;


    void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        Portal = GameObject.FindGameObjectWithTag("Portal");
        player = GameObject.FindGameObjectWithTag("Player").transform;
        portal = GameObject.FindGameObjectWithTag("Portal").transform;
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
    void OnTriggerEnter(Collider punchcoll)
    {
        if (punchcoll.gameObject.name == "Punch")
        {
            {
                if(Player.GetComponent<PlayerController>().IsAttacking && invincible == false)
                {
                    GameObject GC = GameObject.FindGameObjectWithTag("GameController");
                    GC.SendMessage("PlayMiniHit");
                    HPBar.fillAmount = health / startHP;
                    health -= 10;
                    Instantiate(hitFX, transform.position, transform.rotation);
                    mAnimator.SetTrigger("hit");
                }
            }
        }

        if (punchcoll.gameObject.tag == "Forcefield")
        {
            invincible = true;
        }

    }

    void OnTriggerStay(Collider punchcoll)
    {
        if (punchcoll.gameObject.tag == "Forcefield")
        {
            invincible = true;
        }
        else
        {
            invincible = false;
        }
    }


    void OnTriggerExit(Collider punchcoll)
    {
        if (punchcoll.gameObject.tag == "Forcefield")
        {
            invincible = false;
        }
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

        Destroy(GetComponent<CapsuleCollider>());

    }

    void Die()
    {
        mIsDead = true;
        mAgent.enabled = false;
        mAnimator.SetTrigger("die");
        Instantiate(Ragdoll, transform.position, transform.rotation);
        Invoke("ShowItemsDeadState", 1.2f);
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
        if (invincible == true)
        {
            shield.SetActive(true);
        }
        else
        {
            shield.SetActive(false);
        }

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
            float squaredDist = (transform.position + Portal.transform.position).sqrMagnitude;
            float EnemyDistanceRunSqrt = EnemyDistanceRun * EnemyDistanceRun;
            mAgent.SetDestination(portal.position);
        }

        mAnimator.SetBool("walk", IsNavMeshMoving);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Portal")
        {
            Destroy(gameObject);
            Instantiate(hitFX, transform.position, transform.rotation);
        }

    }
}
