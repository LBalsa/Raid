using Characters.Player;
using UnityEngine;
using Weapons;

namespace Characters.Enemies
{
    public class RangedEnemy : Enemy
    {
        [Header("Assets")]
        public GameObject enemyArm;
        public GameObject throwingWeapon;
        public GameObject axe;
        public float projectileSpeed = 0.001f;
        [Range(20f,70f)]
        public float projectileAngle = 45f;

        protected override void Start()
        {
            axe.SetActive(true);
            Initialise();
        }

        private void Fire()
        {
            Vector3 target = PlayerController.inst.transform.position;

            // Hide hand axe;
            axe.SetActive(false);

            // Instantiate & setup throwing axe.
            GameObject weapon = Instantiate(throwingWeapon, enemyArm.transform.position,enemyArm.transform.rotation);
            weapon.GetComponent<MainWeapon>().SetUp(false, true, stats.damage);
            //b.GetComponent<Rigidbody>().AddForce((target.transform.position - enemyArm.transform.position) * projSpeed);
            weapon.GetComponent<Rigidbody>().AddTorque(weapon.transform.forward * -500);
            weapon.GetComponent<MainWeapon>().Throw();
            weapon.transform.LookAt(target);

            // Throw axe by setting its initial velocity.
            weapon.GetComponent<Rigidbody>().velocity = CalculateThrowVector(weapon.transform.position, target);

            // Make hand axe reapear in hand.
            Invoke(nameof(ActivateAxe), 2.0f);
        }

        private Vector3 CalculateThrowVector(Vector3 from, Vector3 to)
        {
            // Calculate distance between target and source.
            float dist = Vector3.Distance(from, new Vector3(to.x, to.y+1f, to.z));
            // Calculate initival velocity required to hit target.
            float Vi = Mathf.Sqrt(dist * -Physics.gravity.y / (Mathf.Sin(Mathf.Deg2Rad * projectileAngle * 2)));
            float Vy, Vz;   // y,z components of the initial velocity

            Vy = Vi * Mathf.Sin(Mathf.Deg2Rad * projectileAngle);
            Vz = Vi * Mathf.Cos(Mathf.Deg2Rad * projectileAngle);

            // Local space velocity vector.
            Vector3 localVelocity = new Vector3(0f, Vy, Vz);

            // Convert to global space.
            Vector3 globalVelocity = transform.TransformVector(localVelocity);

            return globalVelocity;
        }

        private float SomeUnusedMethodWithThisNameINeedCalculateShootAngle()
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

        protected override void Attacking()
        {
            Vector3 dir = (target.transform.position - transform.position).normalized;

            ChangeState(AIState.attacking);
            if (distance > stats.maxdistance * 2)
            {
                ChangeState(AIState.moving);
            }

            anim.SetTrigger("Throw");
            ChangeState(AIState.recovering);
        }
    }
}