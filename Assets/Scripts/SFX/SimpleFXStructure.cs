using UnityEngine;

[CreateAssetMenu(fileName = "SimpleFX", menuName = "Effects/Simple FX Structure")]
public class SimpleFXStructure : ScriptableObject
{
    [Header("Sound FX")]
    public VariableVolumePitch sfx_hit;
    public VariableVolumePitch sfx_destroy;

    [Header("Visual FX")]
    public GameObject vfx_hit;
    public GameObject vfx_destroy;
}
