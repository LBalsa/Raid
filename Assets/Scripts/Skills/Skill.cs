using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Skills
{
    public abstract class Skill : ScriptableObject
    {
        public float cooldown = 10;
        public Image loadIcon;

        //public Skill() { name = null; cooldown = 20; ready = false; }
        //public Skill(GameObject character) { caster = character; }
        //public Skill(string n, float cd, GameObject character) { name = n; cooldown = cd; caster = character; }

        //public abstract void Initialize();// { ready = true; timestamp = Time.time + cooldown; }
        public abstract void Trigger(GameObject caster);// { ready = true; timestamp = Time.time + cooldown; }
        public abstract void Ready(GameObject caster); // { ready = true; }

    }
}