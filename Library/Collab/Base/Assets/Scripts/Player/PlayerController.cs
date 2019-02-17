using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[SelectionBase]
public class PlayerController : MonoBehaviour
{

    public static PlayerController inst;
    Animator anim;
    public bool canCombo = false;

    AnimatorStateInfo stateinfo;

    public float movementsensativity = 2;

    public float jumpHeight = 50;

    public Button pause;


    //****************Storing Animator State Hashes****************************
    //Jump State
    int state_Jump = Animator.StringToHash("Base Layer.Jump");
    //Grounded State
    int state_Grounded = Animator.StringToHash("Base Layer.Grounded");
    //Combat State
    int state_Combat = Animator.StringToHash("Base Layer.Combat Mode");

    //Heavy Attacks States
    int state_HeavyAttack1 = Animator.StringToHash("Base Layer.HeavyAttack1");
    int state_HeavyAttack2 = Animator.StringToHash("Base Layer.HeavyAttack2");

    //Light Attacks States
    int state_LightAttack1 = Animator.StringToHash("Base Layer.LightAttack1");
    int state_LightAttack2 = Animator.StringToHash("Base Layer.LightAttack2");
    //****************Storing Animator State Hashes****************************




    //*******Variables for Enenmy StateMachine***************
    public bool busy = false;
    private bool set = false;

    public bool attacking = false;
    int attackingReset = 0;


    //*******  ***************


    [Header("Weapons")]
    public GameObject weaponPrefab;
    public GameObject weaponSocket;

    public MainWeapon mainWeapon; // main weapon
    Weapon fist; // maybe for when there is no weapon equiped, use the fist.
    Weapon foot;

    //*******Variables for Enenmy StateMachine***************

    // Use this for initialization
    void Awake()
    {
        inst = this;
        anim = GetComponent<Animator>();

        if (weaponPrefab)
        {
            mainWeapon = Instantiate(weaponPrefab, weaponSocket.transform.position, Quaternion.identity, weaponSocket.transform).GetComponent<MainWeapon>();
            mainWeapon.transform.localPosition = new Vector3(0, 0, 0);
            mainWeapon.transform.localEulerAngles = new Vector3(0, 0, 0);
            mainWeapon.transform.localScale = new Vector3(0.04f, 0.04f, 0.04f);
        }

        foreach (Weapon weapon in GetComponentsInChildren<Weapon>())
        {
            if (weapon.name.Contains("Foot"))
            {
                foot = weapon;
            }
            if (weapon.name.Contains("Fist"))
            {
                fist = weapon;
            }
            weapon.SetUp(true, false, 2);
        }

    }


    void Update()
    {
        Cursor.lockState = CursorLockMode.Locked;
        stateinfo = anim.GetCurrentAnimatorStateInfo(0);

        Move();
        Combat();
        Jump();


        if (Input.GetKeyDown(KeyCode.Q) || (Input.GetButton("Start")))
        {
            pause.onClick.Invoke();
        }

        if (busy && !set)
        {
            set = true;
            Invoke("ResetBusy", 5f);
        }
    }



    //*************************MOVEMENT**************************************
    void Move()
    {
        //Animate moving farward and backwards
        anim.SetFloat("Forward", Input.GetAxis("Vertical"));
        //Animate moving left and right
        anim.SetFloat("Turnning", Input.GetAxis("Horizontal"));

        //rotate camera left and right with the mouse
        transform.Rotate(0, Input.GetAxis("Mouse X") * 100f * Time.deltaTime, 0);


        if (stateinfo.fullPathHash == state_Grounded)
        {
            //Moving forwardsand backwards
            if (Input.GetAxis("Vertical") > 0)
                transform.Translate(Vector3.forward * (Input.GetAxis("Vertical")) * movementsensativity * Time.deltaTime);
            //Moving right and left
            if (Input.GetAxis("Horizontal") > 0)
                transform.Translate(Vector3.right * (Input.GetAxis("Horizontal")) * Time.deltaTime);

            //rotate camera left and right with joystick
            if (Input.GetAxis("RightJoyStickHorizontal") < 0)
                transform.Rotate(-Vector3.up, (movementsensativity * 40) * Time.deltaTime);
            ////rotate camera left and right with joystick
            if (Input.GetAxis("RightJoyStickHorizontal") > 0)
                transform.Rotate(Vector3.up, (movementsensativity * 40) * Time.deltaTime);
        }
    }
    //*************************MOVEMENT**************************************


    //*************************JUMPING**************************************

    void Jump()
    {

        if (stateinfo.fullPathHash == state_Grounded)
        {
            if ((Input.GetButton("X")) || (Input.GetKey(KeyCode.Space)))
            {
                anim.SetTrigger("Jump");
                Invoke("ResetTriggers", 0.01f);
            }
        }

        if (stateinfo.fullPathHash == state_Jump)
        {
            if (Input.GetAxis("Vertical") > 0)
                transform.Translate(Vector3.forward * (Input.GetAxis("Vertical")) * (movementsensativity) * Time.deltaTime);
        }
    }

    //*************************JUMPING**************************************


    //*************************COMBAT**************************************
    void Combat()
    {


        if ((Input.GetAxis("R2") == 1) || (Input.GetKey(KeyCode.LeftShift)))
        {
            anim.SetBool("inCombat", true);
        }
        else
        {
            anim.SetBool("inCombat", false);
        }


        //HeavyAttacks

        if (stateinfo.fullPathHash == state_Combat)
        {
            if ((Input.GetButton("Triangle")) || (Input.GetKey(KeyCode.Mouse1)))
            {
                attacking = true; // Enemy block purposes.
                anim.SetTrigger("HeavyAttacks");
                Invoke("ResetTriggers", 0.01f);
            }
        }
        if ((stateinfo.fullPathHash == state_HeavyAttack1) && (canCombo == true))
        {
            if ((Input.GetButton("Square")) || (Input.GetKey(KeyCode.Mouse0)))
            {
                attacking = true; // Enemy block purposes.
                anim.SetTrigger("HeavyAttacks");
                Invoke("ResetTriggers", 0.01f);
            }
        }


        if ((stateinfo.fullPathHash == state_HeavyAttack2) && (canCombo == true))
        {
            if ((Input.GetButton("Square")) || (Input.GetKey(KeyCode.Mouse0)))
            {
                attacking = true; // Enemy block purposes.
                anim.SetTrigger("HeavyAttacks");
                Invoke("ResetTriggers", 0.01f);
            }
        }
        //HeavyAttacks




        //LightAttacks
        if (stateinfo.fullPathHash == state_Combat)
        {
            if ((Input.GetButton("Square")) || (Input.GetKey(KeyCode.Mouse0)))
            {
                attacking = true; // Enemy block purposes.
                anim.SetTrigger("LightAttacks");
                Invoke("ResetTriggers", 0.01f);
            }
        }

        if ((stateinfo.fullPathHash == state_LightAttack1) && (canCombo == true))
        {
            if ((Input.GetButton("Triangle")) || (Input.GetKey(KeyCode.Mouse1)))
            {
                attacking = true; // Enemy block purposes.
                anim.SetTrigger("LightAttacks");
                Invoke("ResetTriggers", 0.01f);
            }
        }

        if ((stateinfo.fullPathHash == state_LightAttack2) && (canCombo == true))
        {
            if ((Input.GetButton("Square")) || (Input.GetKey(KeyCode.Mouse0)))
            {
                attacking = true; // Enemy block purposes.
                anim.SetTrigger("LightAttacks");
                Invoke("ResetTriggers", 0.01f);
            }
        }
        //LightAttacks
    }
    //*************************COMBAT**************************************


    //*****************Resetting Triggers*******************
    void ResetTriggers()
    {
        anim.ResetTrigger("LightAttacks");
        anim.ResetTrigger("HeavyAttacks");
        anim.ResetTrigger("Jump");
        attacking = false;
    }

    //*****************Resetting Triggers*******************

    //****************Functions called by the animation event***********************
    void enableRightArm()
    {
        if (mainWeapon)
        {
            mainWeapon.Enable();
        }
        else
            fist.Enable();
    }

    void disableRightArm()
    {
        canCombo = true;
        if (mainWeapon)
        {
            mainWeapon.Disable();
        }
        else
            fist.Disable();
    }

    void cannotCombo()
    {
        canCombo = false;
    }

    void enableRightFoot()
    {
        foot.Enable();
    }

    void disableRightFoot()
    {
        canCombo = true;
        foot.Disable();
    }

    private void ResetBusy()
    {
        busy = false;
        set = false;
    }

    void JumpHeight()
    {
        Debug.Log("Jumping");
        GetComponent<Rigidbody>().AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
    }

    //****************Functions called by the animation event***********************


    //*******Enemy AI**************
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<IAnimal>() != null)
        {
            other.GetComponent<IAnimal>().Aproach(gameObject);
        }
    }


}
