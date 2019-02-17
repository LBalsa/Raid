using UnityEngine;
using UnityEngine.UI;

[SelectionBase]
public class PlayerController : MonoBehaviour
{
    public static PlayerController inst;

    public float extraMovement = 1.5f; // Adds extra transform to the player movement.
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

    #region Combat variables.

    private bool canCombo = false;
    public bool InCombat { get; private set; } = false;
    public bool IsMoving { get; private set; } = false;
    public bool CanMove { get; private set; } = true;
    public bool CanBeAttacked { get; set; } = false;
    public bool Attacking { get; set; } = false;
    private bool set = false;

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

    // Use this for initialization
    private void Awake()
    {
        inst = this;
        anim = GetComponent<Animator>();
        //camera = GetComponentInChildren<Camera>().gameObject;

        if (weaponPrefab)
        {
            mainWeapon = Instantiate(weaponPrefab, weaponSocket.transform.position, Quaternion.identity, weaponSocket.transform).GetComponent<MainWeapon>();
            mainWeapon.transform.localPosition = new Vector3(0, 0, 0);
            mainWeapon.transform.localEulerAngles = new Vector3(0, 0, 0);
            mainWeapon.transform.localScale = new Vector3(0.04f, 0.04f, 0.04f);
        }

        foreach (Weapon weapon in GetComponentsInChildren<Weapon>())
        {
            if (weapon.name.Contains("Foot")) { foot = weapon; }
            if (weapon.name.Contains("Fist")) { fist = weapon; }

            weapon.SetUp(true, false, 2);
        }

    }

    private void Update()
    {
        //Cursor.lockState = CursorLockMode.Locked;
        stateinfo = anim.GetCurrentAnimatorStateInfo(0);
        if (CanMove)
        {
            MoveUpdate();
            CameraUpdate();
            CombatUpdate();
            JumpUpdate();
        }

        // Pause game on Q.
        if (Input.GetKeyDown(KeyCode.Q) || (Input.GetButton("Start")))
        {
            pause.onClick.Invoke();
        }

        // Reset whether the player can be attacked.
        if (CanBeAttacked && !set)
        {
            set = true;
            Invoke("ResetCanBeAttacked", 5f);
        }
    }


    private void MoveUpdate()
    {
        // Update animator values.
        // moving farward and backwards
        anim.SetFloat("Forward", Input.GetAxis("Vertical"), 1f, Time.deltaTime * 10);
        // moving left and right
        anim.SetFloat("Turnning", Input.GetAxis("Horizontal"), 1f, Time.deltaTime * 10);


        // Add extra movement to compensate for short legs.
        if (stateinfo.fullPathHash == state_Grounded)
        {
            // Moving forwards and backwards
            if (Input.GetAxis("Vertical") != 0)
            {
                transform.Translate(Vector3.forward * (Input.GetAxis("Vertical")) * extraMovement * Time.deltaTime);
            }

            // Moving right and left
            if (Input.GetAxis("Horizontal") != 0)
            {
                transform.Translate(Vector3.right * (Input.GetAxis("Horizontal")) * extraMovement * Time.deltaTime);
            }
        }
        // Only add half of the movement if in combat state.
        else if (stateinfo.fullPathHash == state_Combat)
        {
            // Moving forwards and backwards
            if (Input.GetAxis("Vertical") != 0)
            {
                transform.Translate(Vector3.forward * (Input.GetAxis("Vertical")) * (extraMovement / 3) * Time.deltaTime);
            }

            // Moving right and left
            if (Input.GetAxis("Horizontal") != 0)
            {
                transform.Translate(Vector3.right * (Input.GetAxis("Horizontal")) * (extraMovement / 3) * Time.deltaTime);
            }
        }
    }

    private void CameraUpdate()
    {
        //rotate camera left and right with the mouse or joystick.
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


        //HeavyAttacks
        if (stateinfo.fullPathHash == state_Combat)
        {
            if ((Input.GetButton("Triangle")) || (Input.GetKey(KeyCode.Mouse1)))
            {
                Attacking = true; // Enemy block purposes.
                anim.SetTrigger("HeavyAttacks");
                Invoke("ResetTriggers", 0.01f);
            }
        }

        if ((stateinfo.fullPathHash == state_HeavyAttack1) && (canCombo == true))
        {
            if ((Input.GetButton("Square")) || (Input.GetKey(KeyCode.Mouse0)))
            {
                Attacking = true; // Enemy block purposes.
                anim.SetTrigger("HeavyAttacks");
                Invoke("ResetTriggers", 0.01f);
            }
        }

        if ((stateinfo.fullPathHash == state_HeavyAttack2) && (canCombo == true))
        {
            if ((Input.GetButton("Square")) || (Input.GetKey(KeyCode.Mouse1)))
            {
                Attacking = true; // Enemy block purposes.
                anim.SetTrigger("HeavyAttacks");
                Invoke("ResetTriggers", 0.01f);
            }
        }

        //LightAttacks
        if (stateinfo.fullPathHash == state_Combat)
        {
            if ((Input.GetButton("Square")) || (Input.GetKey(KeyCode.Mouse0)))
            {
                Attacking = true; // Enemy block purposes.
                anim.SetTrigger("LightAttacks");
                Invoke("ResetTriggers", 0.01f);
            }
        }

        if ((stateinfo.fullPathHash == state_LightAttack1) && (canCombo == true))
        {
            if ((Input.GetButton("Triangle")) || (Input.GetKey(KeyCode.Mouse1)))
            {
                Attacking = true; // Enemy block purposes.
                anim.SetTrigger("LightAttacks");
                Invoke("ResetTriggers", 0.01f);
            }
        }

        if ((stateinfo.fullPathHash == state_LightAttack2) && (canCombo == true))
        {
            if ((Input.GetButton("Square")) || (Input.GetKey(KeyCode.Mouse0)))
            {
                Attacking = true; // Enemy block purposes.
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
            if ((Input.GetButton("X")) || (Input.GetKey(KeyCode.Space)))
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


    // Resets animation triggers.
    private void ResetTriggers()
    {
        anim.ResetTrigger("LightAttacks");
        anim.ResetTrigger("HeavyAttacks");
        anim.ResetTrigger("Jump");
        Attacking = false;
    }

    #region Animation Events

    // (Dis)Enable weapon combonets, so that weapons can collide
    // at a certain point during the animation, and not whenever.
    private void EnableRightArm()
    {
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
        CanBeAttacked = false;
        set = false;
    }

    //*******Enemy AI**************
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<IAnimal>() != null)
        {
            other.GetComponent<IAnimal>().Aproach(gameObject);
        }
    }


}
