using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    #region Private Members

    private Animator _animator;

    private CharacterController _characterController;

    private float Gravity = 20.0f;

    private Vector3 _moveDirection = Vector3.zero;

    private InventoryItemBase mCurrentItem = null;

    private HealthBar mHealthBar;

    private HealthBar mFoodBar;

    private int startHealth;

    private int startMana;

    #endregion

    #region Public Members

    public AudioClip Juca;

    AudioSource audioSource;

    public float Speed = 10.0f;

    public float RotationSpeed = 240.0f;

    public Inventory Inventory;

    public GameObject Hand;

    public HUD Hud;

    public GameObject BattleCryFX;

    #endregion

    // Use this for initialization
    void Start()
    {
        _animator = GetComponent<Animator>();

        _characterController = GetComponentInChildren<CharacterController>();
        Inventory.ItemUsed += Inventory_ItemUsed;
        Inventory.ItemRemoved += Inventory_ItemRemoved;

        mHealthBar = Hud.transform.Find("Bars_Panel/HealthBar").GetComponent<HealthBar>();
        mHealthBar.Min = 0;
        mHealthBar.Max = Health;
        startHealth = Health;

    }

    #region Inventory

    private void Inventory_ItemRemoved(object sender, InventoryEventArgs e)
    {
        InventoryItemBase item = e.Item;

        GameObject goItem = (item as MonoBehaviour).gameObject;
        goItem.SetActive(true);
        goItem.transform.parent = null;

    }

    private void SetItemActive(InventoryItemBase item, bool active)
    {
        GameObject currentItem = (item as MonoBehaviour).gameObject;
        currentItem.SetActive(active);
        currentItem.transform.parent = active ? Hand.transform : null;
    }

    private void Inventory_ItemUsed(object sender, InventoryEventArgs e)
    {
        if (e.Item.ItemType != EItemType.Consumable)
        {
            // If the player carries an item, un-use it (remove from player's hand)
            if (mCurrentItem != null)
            {
                SetItemActive(mCurrentItem, false);
            }

            InventoryItemBase item = e.Item;

            // Use item (put it to hand of the player)
            SetItemActive(item, true);

            mCurrentItem = e.Item;
        }

    }

    //movelist
    private int Attack_2_Hash = Animator.StringToHash("Base Layer.Attack_Spin");
    private int Attack_3_Hash = Animator.StringToHash("Base Layer.Attack_Jump");
    public int Combo_1_Hash = Animator.StringToHash("Base Layer.Combo_1");
    public int Combo_2_Hash = Animator.StringToHash("Base Layer.Combo_2");
    public int Combo_3_Hash = Animator.StringToHash("Base Layer.Combo_3");


    public bool IsAttacking
    {
        get
        {
            AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.fullPathHash == Attack_2_Hash || stateInfo.fullPathHash == Attack_3_Hash || stateInfo.fullPathHash == Combo_1_Hash || stateInfo.fullPathHash == Combo_2_Hash || stateInfo.fullPathHash == Combo_3_Hash)
            {
                return true;
            }
            return false;
        }
    }


    #endregion

    #region Health & Mana

    [Tooltip("Amount of health")]
    public int Health = 100;


    public bool IsDead
    {
        get
        {
            return Health == 0;
        }
    }

    public bool IsArmed
    {
        get
        {
            return true;
        }
    }


    public void usePotion(int amount)
    {
        Health += amount;
        if (Health > startHealth)
        {
            Health = startHealth;
        }

        mHealthBar.SetValue(Health);
    }

    public void TakeDamage(int amount)
    {
        Health -= amount;
        GameObject GC = GameObject.FindGameObjectWithTag("GameController");
        GC.SendMessage("PlayJucaDMG");
        if (Health < 0)
            Health = 0;

        mHealthBar.SetValue(Health);

        if (IsDead)
        {
            _animator.SetTrigger("death");
            GC.SendMessage("PlayJucaDeath");
           GC.SendMessage("IvokeGameOver");
        }

    }

    #endregion



    // Update is called once per frame
    void Update()
    {
       if (IsAttacking == true)
        {
            _animator.SetBool("CanMove", false);
            Speed = 0;
            RotationSpeed = 0;
        }
        else {
            _animator.SetBool("CanMove", true);
            Speed = 10.0f;
            RotationSpeed = 240f;
        }

        if (!IsDead)
        {
            // Interact with the item
            if (mInteractItem != null)
            {
                // Common interact method
                mInteractItem.OnInteract();

                if (mInteractItem is InventoryItemBase)
                {
                    Inventory.AddItem(mInteractItem as InventoryItemBase);
                    (mInteractItem as InventoryItemBase).OnPickup();
                }

                Hud.CloseMessagePanel();

                mInteractItem = null;
            }

            #region Combo 1

            // Combo 1 - 1
            if (Input.GetMouseButtonDown(0) && IsAttacking == false)
            {
                // Dont execute click if mouse pointer is over uGUI element
                if (!EventSystem.current.IsPointerOverGameObject())
                {
                    _animator.SetTrigger("Combo_1");
                    GameObject GC = GameObject.FindGameObjectWithTag("GameController");
                    GC.SendMessage("PlayJucaAttack1");
                }
            }

            if (Input.GetMouseButtonDown(0) && IsAttacking == true)
            {
                // Dont execute click if mouse pointer is over uGUI element
                if (!EventSystem.current.IsPointerOverGameObject())
                {
                    _animator.SetTrigger("Combo_1");
                    GameObject GC = GameObject.FindGameObjectWithTag("GameController");
                    GC.SendMessage("PlayJucaAttack2");
                }
            }

            # endregion

            // Heavy Attack
            if (mCurrentItem != null && Input.GetMouseButtonDown(1) && IsAttacking == false)
            {
                if (!EventSystem.current.IsPointerOverGameObject())
                {
                    _animator.SetTrigger("attack_2");
                }
            }

            // Jump Attack
            if (mCurrentItem != null && Input.GetKeyDown(KeyCode.Space) && IsAttacking == false)
            {
                if (!EventSystem.current.IsPointerOverGameObject())
                {
                    _animator.SetTrigger("attack_jump");
                }
            }


            // Get Input for axis
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            // Calculate the forward vector
            Vector3 camForward_Dir = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
            Vector3 move = v * camForward_Dir + h * Camera.main.transform.right;

            if (move.magnitude > 1f) move.Normalize();

            // Calculate the rotation for the player
            move = transform.InverseTransformDirection(move);

            // Get Euler angles
            float turnAmount = Mathf.Atan2(move.x, move.z);

            transform.Rotate(0, turnAmount * RotationSpeed * Time.deltaTime, 0);

            if (_characterController.isGrounded)
            {
                _animator.SetBool("run", move.magnitude > 0);

                _moveDirection = transform.forward * move.magnitude;

                _moveDirection *= Speed;

            }

            _moveDirection.y -= Gravity * Time.deltaTime;

            _characterController.Move(_moveDirection * Time.deltaTime);
        }
    }

    private InteractableItemBase mInteractItem = null;

    private void OnTriggerEnter(Collider other)
    {
        InteractableItemBase item = other.GetComponent<InteractableItemBase>();
        if (item != null)
        {
            mInteractItem = item;

            Hud.OpenMessagePanel(mInteractItem);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        InteractableItemBase item = other.GetComponent<InteractableItemBase>();
        if (item != null)
        {
            Hud.CloseMessagePanel();
            mInteractItem = null;
        }
    }

    void Dance()
    {
        _animator.SetTrigger("Winner");
    }

}
