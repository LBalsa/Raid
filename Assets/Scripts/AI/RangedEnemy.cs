using UnityEngine;

public class RangedEnemy : Enemy
{
    [Header("Assets")]
    public GameObject enemyArm;
    public GameObject throwingWeapon;
    public GameObject axe;
    public float projectileSpeed = 0.001f;
    [Range(20f,70f)]
    public float projectileAngle = 45f;

    private void Start()
    {
        axe.SetActive(true);
        Initialise();
    }

    public void Fire()
    {
        //Hide fixed axe;
        axe.SetActive(false);
        // Instantiate & setup throwing axe.
        GameObject b = Instantiate(throwingWeapon, enemyArm.transform.position,enemyArm.transform.rotation);
        b.GetComponent<Weapon>().SetUp(false, true, damage);
        //b.GetComponent<Rigidbody>().AddForce((target.transform.position - enemyArm.transform.position) * projSpeed);

        b.GetComponent<Rigidbody>().AddTorque(b.transform.forward * -500);
        b.GetComponent<Weapon>().Throw();

        // Calculate distance between target and source.
        Vector3 target = PlayerController.inst.transform.position;
        float dist = Vector3.Distance(b.transform.position, new Vector3(target.x, target.y+1f, target.z));

        // Rotate axe to face the target.
        transform.LookAt(target);

        // Calculate initival velocity required to hit target.
        float Vi = Mathf.Sqrt(dist * -Physics.gravity.y / (Mathf.Sin(Mathf.Deg2Rad * projectileAngle * 2)));
        float Vy, Vz;   // y,z components of the initial velocity

        Vy = Vi * Mathf.Sin(Mathf.Deg2Rad * projectileAngle);
        Vz = Vi * Mathf.Cos(Mathf.Deg2Rad * projectileAngle);

        // Local space velocity vector.
        Vector3 localVelocity = new Vector3(0f, Vy, Vz);

        // Convert to global space.
        Vector3 globalVelocity = transform.TransformVector(localVelocity);

        // Throw axe by setting its initial velocity.
        b.GetComponent<Rigidbody>().velocity = globalVelocity;

        // Make fixed axe reapear in hand.
        Invoke("ActivateAxe", 2.0f);
    }

    private float CalculateShootAngle()
    {
        float distance = (target.transform.position - transform.position).magnitude;

        float aSinParameter = (Physics.gravity.y * distance) / (projectileSpeed * projectileSpeed);
        if (aSinParameter > 1.0f)
        {
            Debug.Log("The target is out of range!"); return 0;
        }
        else
        {
            return 0.5f * Mathf.Asin(aSinParameter);
        }
    }

    private void ActivateAxe()
    {
        axe.SetActive(true);
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }


    protected override void Moving()
    {
        base.Moving();
    }

    // Circling
    protected override void Cirling()
    {
        base.Cirling();
    }

    // Attacking
    protected override void Attacking()
    {

        Vector3 dir = (target.transform.position - transform.position).normalized;

        ChangeState(AIState.attacking);
        if (distance > maxdistance * 2)
        {
            ChangeState(AIState.moving);
        }

        anim.SetTrigger("Throw");
        ChangeState(AIState.recovering);

    }

    //protected override void AttackEnd()
    //{
    //    ChangeState(AIState.circling);
    //}

}
