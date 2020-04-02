using UnityEngine;

namespace SpecialEffects
{
    public abstract class AudioEvent : ScriptableObject
    {
        public abstract void Play(AudioSource aus);
    }
}