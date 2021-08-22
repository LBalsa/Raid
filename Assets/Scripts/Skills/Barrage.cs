using SpecialEffects;
using UnityEngine;

namespace Skills
{
    [CreateAssetMenu(menuName = "Skills/Barrage")]
    public class Barrage : Skill
    {
        [Header("CannonBall")]
        public float damage = 1;
        public float explosionRadius = 2;
        public float shots = 10;
        [MinMaxRange(2, 10)]
        public RangedFloat dropHeight = new RangedFloat(4, 5);
        public int multiplier = 1;
        public GameObject cannonball;

        [Header("Special Effects")]
        public VariableVolumePitch cannonSFX;
        public VariableVolumePitch fallSFX;
        public VariableVolumePitch explosionSFX;
        public GameObject explosionVFX;

        public override void Ready(GameObject caster)
        {
            throw new System.NotImplementedException();
        }

        public override void Trigger(GameObject caster)
        {
            if (cannonball)
            {
                var shotCouNt = shots;
                // Skill level multiplier, increments of 5.
                if (multiplier > 1)
                {
                    //multiplier--;
                    shotCouNt += 5 * multiplier;
                }

                // Instantiate cannonballs in a random circle above player.
                float delay = 1;
                for (int i = 1; i < shots + 1; i++)
                {
                    GameObject go = Instantiate(cannonball, new Vector3(
                        caster.transform.position.x + Random.Range(-2f, 2f),
                        caster.transform.position.y + Random.Range(dropHeight.minValue, dropHeight.maxValue),
                        caster.transform.position.z + Random.Range(-2f, 2f)), Quaternion.identity) as GameObject;//.GetComponent<CannonBall>();

                    CannonBall cb = go.GetComponent<CannonBall>();
                    if (!cb)
                    {
                        go.AddComponent<CannonBall>();
                        cb = go.GetComponent<CannonBall>();
                    }
                    // Set up the cannonball parameters;
                    cb.Set(delay, damage, explosionRadius, cannonSFX, fallSFX, explosionSFX, explosionVFX);
                    delay++;
                }
            }
        }
    }
}
