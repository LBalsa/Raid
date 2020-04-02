using UnityEngine;

namespace SpecialEffects.Structures
{
    [CreateAssetMenu(fileName = "FX", menuName = "Effects/Player FX Structure")]
    public class PlayerFXStructure : ScriptableObject
    {
        [Header("Sound FX")]
        public VariableVolumePitch sfx_swordAttack;
        public VariableVolumePitch sfx_fistAttack;
        public VariableVolumePitch sfx_kickAttack;
        public VariableVolumePitch sfx_hurt;
        public VariableVolumePitch sfx_block;
        public VariableVolumePitch sfx_die;
        public VariableVolumePitch sfx_taunt;
        public VariableVolumePitch sfx_whistle;


        public AudioClip sfx_coin;
        public AudioClip sfx_pickupWeapon;
        public AudioClip sfx_dropWeapon;
        public AudioClip swosh;

        [Header("Visual FX")]
        public GameObject vfx_spawn;
        public GameObject vfx_despawn;
        public GameObject vfx_hurt;
        public GameObject vfx_block;

        public void Play(AudioSource source, VariableVolumePitch vvp)
        {
            if (vvp) vvp.Play(source); else Debug.LogWarning("Player FX in " + source.gameObject.name + " not assigned");
        }

        public void Play(AudioSource source, AudioClip auc)
        {
            if (auc) source.PlayOneShot(auc); else Debug.LogWarning("Player FX " + auc + " not assigned");
        }
    }
}