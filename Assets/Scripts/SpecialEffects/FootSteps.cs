using UnityEngine;
using Utility;

namespace SpecialEffects
{
    public class FootSteps : MonoBehaviour
    {

        public bool playFootsteps;
        public bool detectTerrain = false;
        [SerializeField]
        private AudioClip[] lightClips;
        [SerializeField]
        private AudioClip[] normalClips;
        [SerializeField]
        private AudioClip[] heavyClips;


        [SerializeField]
        private AudioClip[] stoneClips;
        [SerializeField]
        private AudioClip[] mudClips;
        [SerializeField]
        private AudioClip[] grassClips;

        public AudioSource audioSource;
        private TerrainDetector terrainDetector;
        private Animator animator;

        private void Awake()
        {
            if (!audioSource)
            {
                AudioSource[] aus = GetComponents<AudioSource>();
                if (aus[1])
                {
                    audioSource = aus[1];
                }
            }
            if (detectTerrain)
            {
                terrainDetector = new TerrainDetector();
            }
            animator = GetComponent<Animator>();
        }

        private void Jump()
        {
            AudioClip clip = GetRandomLightClip();
            audioSource.PlayOneShot(clip);
        }

        private void Land()
        {
            AudioClip clip = GetRandomHeavyClip();
            audioSource.PlayOneShot(clip);
        }

        private void Step()
        {
            if (ForwardCheck())
            {
                AudioClip clip = GetRandomNormalClip();
                audioSource.PlayOneShot(clip);
            }
        }

        private void StepSlow()
        {
            if (ForwardCheck())
            {
                AudioClip clip = GetRandomLightClip();
                audioSource.PlayOneShot(clip);
            }
        }

        private void StepFast()
        {
            if (ForwardCheck())
            {
                AudioClip clip = GetRandomHeavyClip();
                audioSource.PlayOneShot(clip);
            }
        }

        private void SideStepSlow()
        {
            if (SideCheck())
            {
                AudioClip clip = GetRandomLightClip();
                audioSource.PlayOneShot(clip);
            }
        }

        private void SideStep()
        {
            if (SideCheck())
            {
                AudioClip clip = GetRandomNormalClip();
                audioSource.PlayOneShot(clip);
            }
        }

        private void SideStepFast()
        {
            if (SideCheck())
            {
                AudioClip clip = GetRandomHeavyClip();
                audioSource.PlayOneShot(clip);
            }
        }

        // Motions checkers
        private bool ForwardCheck()
        {
            // Prevent blend tree from firing multiple blended events.
            if (Mathf.Abs(animator.GetFloat("Strafe")) < Mathf.Abs(animator.GetFloat("Forward")))
            {
                // Prevent sound extending after stopping.
                if (animator.GetFloat("Forward") > 0.5f || animator.GetFloat("Forward") < -0.5f)
                {
                    return true;
                }
                else { return false; }
            }
            else { return false; }
        }
        private bool SideCheck()
        {
            // Prevent blend tree from firing multiple blended events.
            if (Mathf.Abs(animator.GetFloat("Strafe")) > Mathf.Abs(animator.GetFloat("Forward")))
            {
                // Prevent sound extending after stopping.
                if (animator.GetFloat("Strafe") > 0.5f || animator.GetFloat("Strafe") < -0.5f)
                {
                    return true;
                }
                else { return false; }
            }
            else { return false; }
        }


        // Clip getters
        private AudioClip GetRandomLightClip()
        {
            return lightClips[UnityEngine.Random.Range(0, lightClips.Length)];
        }
        private AudioClip GetRandomNormalClip()
        {
            return normalClips[UnityEngine.Random.Range(0, normalClips.Length)];
        }
        private AudioClip GetRandomHeavyClip()
        {
            return heavyClips[UnityEngine.Random.Range(0, heavyClips.Length)];
        }


        private AudioClip GetRandomClip()
        {


            int terrainTextureIndex = terrainDetector.GetActiveTerrainTextureIdx(transform.position);


            switch (terrainTextureIndex)
            {
                case 0:
                    return stoneClips[UnityEngine.Random.Range(0, stoneClips.Length)];
                case 1:
                    return mudClips[UnityEngine.Random.Range(0, mudClips.Length)];
                case 2:
                default:
                    return grassClips[UnityEngine.Random.Range(0, grassClips.Length)];
            }

        }
    }
}