using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "SFX_", menuName = "Effects/Variable Volume And Pitch")]
public class VariableVolumePitch : AudioEvent
{
    public AudioClip[] clips;
    [MinMaxRange(0, 1)]
    public RangedFloat volume = new RangedFloat(.75f, 1);
    [MinMaxRange(0, 2)]
    public RangedFloat pitch = new RangedFloat(.75f, 1);

    public override void Play(AudioSource source)
    {
        if (!source)
        {
            Debug.LogError("Audiosource missing, unable to play " + this.name); return;
        }

        if (clips.Length == 0)
        {
            Debug.LogError("No clips in this " + this.name);
            return;
        }
        //source.clip = clips[Random.Range(0, clips.Length)];
        source.volume = Random.Range(volume.minValue, volume.maxValue);
        source.pitch = Random.Range(pitch.minValue, pitch.maxValue);
        //source.Play();
        source.PlayOneShot(clips[Random.Range(0, clips.Length)]);
    }
}
