using UnityEngine;

namespace SpecialEffects.Structures
{
    [CreateAssetMenu(fileName = "FX", menuName = "Effects/FX Structure")]
    public class FXStructure : ScriptableObject
    {
        [Header("Sound FX")]
        public VariableVolumePitch sfx_attack;
        public VariableVolumePitch sfx_hurt;
        public VariableVolumePitch sfx_block;
        public VariableVolumePitch sfx_die;
        public VariableVolumePitch sfx_taunt;
        public AudioClip swosh;

        [Header("Visual FX")]
        public GameObject vfx_spawn;
        public GameObject vfx_despawn;
        public GameObject vfx_hurt;
        public GameObject vfx_block;

    }
}