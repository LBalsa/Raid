using Controllers;
using SpecialEffects.Structures;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Weapons;

namespace Characters.Player
{
    [SelectionBase]
    [RequireComponent(typeof(HealthManager))]
    [RequireComponent(typeof(InteractionManager))]
    [RequireComponent(typeof(InventoryManager))]
    [RequireComponent(typeof(SkillManager))]
    public partial class PlayerController : MonoBehaviour, IAttackable
    {
        public static PlayerController inst;

        [Header("Player Movement")]
        public float extraForwardsMovement = 1.5f; // Adds extra transform to the player movement.
        public float extraSidewaysRotation = 30f; // Adds extra rotation when player runs at an angle.
        public float movementsensativity = 2;
        public float jumpHeight = 50;

        public new GameObject camera;
        public Button pause;

        [Header("Weapons")]
        public GameObject weaponPrefab; // Default weapon prefab.
        public GameObject weaponSocket; // To attach weapons.
        public MainWeapon mainWeapon; // Reference to equipped weapon.
        public Weapon fist; // For when there is no weapon equipped.
        public Weapon foot; // For kicking.
        public float playerDamage = 2f;
        private float holdingTimer = 0f;
        private bool hasWeapon = true;

        #region Combat variables.

        public event Attack OnAttack;
        public event Death OnDeath;

        private bool canCombo = false;
        public bool InCombat { get; private set; } = false;
        public bool IsMoving { get; private set; } = false;
        public bool IsRotating { get; private set; } = false;
        public bool CanMove { get; set; } = true;
        public bool CanBeAttacked { get; private set; } = true;

        #endregion

        #region Storing Animator State Hashes

        private Animator anim;
        private AnimatorStateInfo stateinfo;

        //Jump State
        private int state_Jump = Animator.StringToHash("Base Layer.Jump");
        private int state_JumpForward = Animator.StringToHash("Base Layer.Jump Forward");

        //Grounded State
        private int state_Grounded = Animator.StringToHash("Base Layer.Grounded");

        //Combat State
        private int state_Combat = Animator.StringToHash("Base Layer.Combat Mode");

        //Heavy Attacks States
        private int state_HeavyAttack1 = Animator.StringToHash("Base Layer.HeavyAttack1");
        private int state_HeavyAttack2 = Animator.StringToHash("Base Layer.HeavyAttack2");

        //Light Attacks States
        private int state_LightAttack1 = Animator.StringToHash("Base Layer.LightAttack1");
        private int state_LightAttack2 = Animator.StringToHash("Base Layer.LightAttack2");

        #endregion

        public PlayerFXStructure fx;
        private AudioSource audioSource;

        // Use this for initialization
        private void Awake()
        {
            Debug.Log("Initializing player...");
            inst = this;
            anim = GetComponent<Animator>();
            audioSource = GetComponent<AudioSource>();

            Debug.Log("Instantiating main weapon...");
            if (weaponPrefab)
            {
                mainWeapon = Instantiate(weaponPrefab, weaponSocket.transform.position, Quaternion.identity, weaponSocket.transform).GetComponent<MainWeapon>();
                mainWeapon.transform.localPosition = new Vector3(0, 0, 0);
                mainWeapon.transform.localEulerAngles = new Vector3(0, 0, 0);
                mainWeapon.transform.localScale = new Vector3(0.04f, 0.04f, 0.04f);
            }
            else { Debug.LogError("Player main weapon missing."); }

            foreach (Weapon weapon in GetComponentsInChildren<Weapon>())
            {
                if (weapon.name.Contains("Foot")) { foot = weapon; }
                if (weapon.name.Contains("Fist")) { fist = weapon; }
                weapon.SetUp(true, false, playerDamage);
                Debug.Log(weapon.name + " initialized with " + weapon.damage + " damage.");

            }

            Cursor.lockState = CursorLockMode.Locked;
        }

        private void Start()
        {
            GetComponent<HealthManager>().OnDeath += delegate { enabled = false; OnDeath?.Invoke(true); };
            GetComponent<InteractionManager>().OnBlockingInteraction += delegate { CanMove = false; };
            GetComponent<InteractionManager>().OnFreeInteraction += delegate { CanMove = true; };
            GetComponent<InteractionManager>().OnPickupWeapon += PickupWeapon;
            GameController.inst.OnPause += delegate { enabled = false; ; };
            GameController.inst.OnUnPause += delegate { enabled = true; };
        }

        private void Update()
        {
            //Cursor.lockState = CursorLockMode.Locked;
            stateinfo = anim.GetCurrentAnimatorStateInfo(0);
            if (CanMove)// && !GameController.inst.Paused)
            {
                // Movement.
                MoveUpdate();
                JumpUpdate();

                // Attack triggers.
                CombatUpdate();
                // Handles picking up and dropping of weapons.
                WeaponHandler();

                //// Pause game on Q.
                //if (Input.GetKeyDown(KeyCode.Q) || (Input.GetButton("Start")))
                //{
                //    pause.onClick.Invoke();
                //}
            }
            else
            {
                anim.SetFloat("Forward", 0, 0f, Time.deltaTime * 10);
                anim.SetFloat("Strafe", 0, 0f, Time.deltaTime * 10);
            }
        }


        private void MoveUpdate()
        {
            // Update animator values.
            // moving farward and backwards
            anim.SetFloat("Forward", Input.GetAxis("Vertical"), 1f, Time.deltaTime * 10);
            // moving left and right
            anim.SetFloat("Strafe", Input.GetAxis("Horizontal"), 1f, Time.deltaTime * 10);

            // Player is not moving if there is no input.
            if (Input.GetAxis("Vertical") == 0 && Input.GetAxis("Horizontal") == 0) { IsMoving = false; }

            if (stateinfo.fullPathHash == state_Grounded)
            {
                // Moving forwards and backwards
                if (Input.GetAxis("Vertical") != 0)
                {
                    // If player was stationary, rotate player to camera's forward position;
                    if (!IsMoving)
                    {
                        transform.rotation = Quaternion.Euler(transform.localEulerAngles.x, Camera.main.transform.localEulerAngles.y, transform.localEulerAngles.z);
                    }
                    IsMoving = true;

                    // Add extra movement to compensate for short legs.
                    transform.Translate(Vector3.forward * (Input.GetAxis("Vertical")) * extraForwardsMovement * Time.deltaTime);



                    //// If player was stationary, rotate player to camera's forward position;
                    //if (!IsMoving && !IsRotating)
                    //{
                    //    //transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, Camera.main.transform.localEulerAngles.y, transform.localEulerAngles.z);
                    //    Quaternion camForward = Quaternion.Euler(transform.localEulerAngles.x, Camera.main.transform.localEulerAngles.y, transform.localEulerAngles.z);
                    //    StartCoroutine("RotateToCameraForward", camForward);
                    //    IsMoving = true;
                    //}
                    //if (!IsRotating)
                    //{
                    //    transform.Translate(Vector3.forward * (Input.GetAxis("Vertical")) * extraForwardsMovement * Time.deltaTime);
                    //}
                }

                // Moving right and left
                if (Input.GetAxis("Horizontal") != 0)
                {
                    IsMoving = true;

                    // Add extra movement to compensate for short legs.
                    transform.Translate(Vector3.right * (Input.GetAxis("Horizontal")) * extraForwardsMovement * Time.deltaTime);

                    // Allow for side movement to rotate the player slightly if running forward.
                    if (Input.GetAxis("Vertical") > 0)
                    {
                        transform.Rotate(0, Input.GetAxis("Horizontal") * extraSidewaysRotation * Time.deltaTime, 0);
                    }

                }
            }

            // Only add half of the movement if in combat state.
            else if (stateinfo.fullPathHash == state_Combat)
            {
                // Moving forwards and backwards
                if (Input.GetAxis("Vertical") != 0)
                {
                    transform.Translate(Vector3.forward * (Input.GetAxis("Vertical")) * (extraForwardsMovement / 3) * Time.deltaTime);
                }

                // Moving right and left
                if (Input.GetAxis("Horizontal") != 0)
                {
                    transform.Translate(Vector3.right * (Input.GetAxis("Horizontal")) * (extraForwardsMovement / 3) * Time.deltaTime);
                }
            }

            // Rotate character with the mouse or joystick if player is moving or in combat.
            // Else allow free camera rotation around player.
            if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0 || InCombat)
            {
                transform.Rotate(0, Input.GetAxis("Mouse X") * 100f * Time.deltaTime, 0);

                //rotate camera left and right with joystick
                if (Input.GetAxis("RightJoyStickHorizontal") < 0)
                {
                    transform.Rotate(-Vector3.up, (movementsensativity * 40) * Time.deltaTime);
                }
                //rotate camera left and right with joystick
                if (Input.GetAxis("RightJoyStickHorizontal") > 0)
                {
                    transform.Rotate(Vector3.up, (movementsensativity * 40) * Time.deltaTime);
                }
            }
            else
            {
                //camera.transform.RotateAround(Vector3.zero, transform.up, Input.GetAxis("Mouse X") * 100f * Time.deltaTime);
                //(transform.position, Vector3.up, Input.GetAxis("Mouse X") * 100f * Time.deltaTime);
            }
        }

        private IEnumerator RotateToCameraForward(Quaternion camForward)
        {
            print("Rotate start");
            IsRotating = true;

            int x = 0;
            while (x < 30)
            {
                print("Rotating");
                transform.rotation = Quaternion.RotateTowards(transform.rotation, camForward, 500 * Time.deltaTime);
                x++;
                yield return null;

            }
            print("Rotate finish");
            IsRotating = false;
        }

        private void CombatUpdate()
        {
            // Set combat state.
            if ((Input.GetAxis("R2") == 1) || (Input.GetKey(KeyCode.LeftShift)))
            {
                InCombat = true;
                anim.SetBool("inCombat", true);
            }
            else if (InCombat)
            {
                InCombat = false;
                anim.SetBool("inCombat", false);
            }
            if (Input.GetKey(KeyCode.Q))
            {
                //block
            }


            //HeavyAttacks
            if (stateinfo.fullPathHash == state_Combat)
            {
                if ((Input.GetButton("Triangle")) || (Input.GetKey(KeyCode.Mouse1)))
                {
                    OnAttack?.Invoke();
                    anim.SetTrigger("HeavyAttacks");
                    Invoke("ResetTriggers", 0.01f);
                }
            }

            if ((stateinfo.fullPathHash == state_HeavyAttack1) && (canCombo == true))
            {
                if ((Input.GetButton("Square")) || (Input.GetKey(KeyCode.Mouse0)))
                {
                    OnAttack?.Invoke();
                    anim.SetTrigger("HeavyAttacks");
                    Invoke("ResetTriggers", 0.01f);
                }
            }

            if ((stateinfo.fullPathHash == state_HeavyAttack2) && (canCombo == true))
            {
                if ((Input.GetButton("Square")) || (Input.GetKey(KeyCode.Mouse1)))
                {
                    OnAttack?.Invoke();
                    anim.SetTrigger("HeavyAttacks");
                    Invoke("ResetTriggers", 0.01f);
                }
            }

            //LightAttacks
            if (stateinfo.fullPathHash == state_Combat)
            {
                if ((Input.GetButton("Square")) || (Input.GetKey(KeyCode.Mouse0)))
                {
                    OnAttack?.Invoke();
                    anim.SetTrigger("LightAttacks");
                    Invoke("ResetTriggers", 0.01f);
                }
            }

            if ((stateinfo.fullPathHash == state_LightAttack1) && (canCombo == true))
            {
                if ((Input.GetButton("Triangle")) || (Input.GetKey(KeyCode.Mouse1)))
                {
                    OnAttack?.Invoke();
                    anim.SetTrigger("LightAttacks");
                    Invoke("ResetTriggers", 0.01f);
                }
            }

            if ((stateinfo.fullPathHash == state_LightAttack2) && (canCombo == true))
            {
                if ((Input.GetButton("Square")) || (Input.GetKey(KeyCode.Mouse0)))
                {
                    OnAttack?.Invoke();
                    anim.SetTrigger("LightAttacks");
                    Invoke("ResetTriggers", 0.01f);
                }
            }
        }

        private void JumpUpdate()
        {
            // Jump.
            if (stateinfo.fullPathHash == state_Grounded || stateinfo.fullPathHash == state_Combat)
            {
                if ((Input.GetButton("X")) || (Input.GetKeyDown(KeyCode.Space)))
                {
                    anim.SetTrigger("Jump");
                    Invoke("ResetTriggers", 0.05f);
                }
            }

            // While jumping in place.
            if (stateinfo.fullPathHash == state_Jump)
            {
                if (Input.GetAxis("Vertical") > 0)
                {
                    //  transform.Translate(Vector3.forward * (Input.GetAxis("Vertical")) * (movementsensativity) * Time.deltaTime);
                }
            }

            // While jumping forward.
            if (stateinfo.fullPathHash == state_JumpForward)
            {
                if (Input.GetAxis("Vertical") > 0)
                {
                    transform.Translate(Vector3.forward * (Input.GetAxis("Vertical")) * (movementsensativity) * Time.deltaTime);
                }
            }
        }

        private void WeaponHandler()
        {
            // Weapon drop cooldown, to prevent instant pickup and drop of weapon.
            if (holdingTimer >= 0) { holdingTimer -= Time.deltaTime; }

            // Drop current weapon.
            if (hasWeapon && holdingTimer < 0)
            {
                if (Input.GetKeyDown(KeyCode.R) || (Input.GetButton("Circle")))
                {
                    Debug.Log("Player has dropped " + weaponSocket.transform.GetChild(0).gameObject);
                    fx.Play(audioSource, fx.sfx_dropWeapon);
                    hasWeapon = false; // Enable player to pick up another weapon.
                    anim.speed = 1; // Reset animator speed.

                    mainWeapon.Drop();
                    mainWeapon = null;
                }
            }

        }

        public void OnAttacked()
        {
            CanBeAttacked = false;
            Invoke("ResetCanBeAttacked", 5f);
        }

        // Resets animation triggers.
        private void ResetTriggers()
        {
            anim.ResetTrigger("LightAttacks");
            anim.ResetTrigger("HeavyAttacks");
            anim.ResetTrigger("Jump");
            //Attacking = false;
        }

        #region Animation Events

        // (Dis)Enable weapon combonets, so that weapons can collide
        // at a certain point during the animation, and not whenever.
        private void EnableRightArm()
        {
            // Play attack sound effect if there are any.

            fx.Play(audioSource, fx.sfx_swordAttack);

            if (mainWeapon) { mainWeapon.Enable(); }
            else { fist.Enable(); }
        }

        private void DisableRightArm()
        {
            canCombo = true;
            if (mainWeapon) { mainWeapon.Disable(); }
            else { fist.Disable(); }
        }

        private void EnableRightFoot()
        {
            fx.Play(audioSource, fx.sfx_kickAttack);
            foot.Enable();
        }

        private void DisableRightFoot()
        {
            canCombo = true;
            foot.Disable();
        }


        // Disable trigger after combo window has expired.
        private void CannotCombo()
        {
            canCombo = false;
        }

        private void JumpHeight()
        {
            Debug.Log("Jumping");
            GetComponent<Rigidbody>().AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
        }

        #endregion

        private void ResetCanBeAttacked()
        {
            CanBeAttacked = true;
        }

        public void PickupWeapon(MainWeapon weapon)
        {
            if (hasWeapon)
            {
                mainWeapon.Drop();
            }

            // Prevent player from picking up more weapons.
            hasWeapon = true;
            holdingTimer = 1f;

            // Pick up weapon and assign it as the weapon.
            weapon.Pickup(weaponSocket.transform, true, playerDamage);
            mainWeapon = weapon;

            // Asjust animation speed to weapon weight.
            anim.speed = weapon.speed;

            fx.Play(audioSource, fx.sfx_pickupWeapon);

            //weaponPickupTooltip.SetActive(false);
        }
    }
}