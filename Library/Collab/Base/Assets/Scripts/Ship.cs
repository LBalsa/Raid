using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//https://unity3d.com/learn/tutorials/topics/scripting/ability-system-scriptable-objects


public class Ship : MonoBehaviour
{

    public static Ship inst = null;

    [System.Serializable]
    public class SkillElement
    {
        public Skill skill;
        [ReadOnly]
        public float remainingCooldown;
        public Image load;
        public Text coolDownText;
    }
    public SkillElement[] skills;
    public AudioClip skillReady;
    public AudioClip error;
    AudioSource aus;

    // Use this for initialization
    void Start()
    {
        inst = this;

        aus = GetComponent<AudioSource>();

        // Initialise skill cooldowns.
        if (skills != null && skills.Length > 0)
        {
            foreach (SkillElement element in skills)
            {
                element.remainingCooldown = element.skill.cooldown;
                //element.skill.Ready(Player.inst.gameObject);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (skills != null && skills.Length > 0)//!GameController.inst.paused)
        {
            KeyPresses();
            Cooldown();
        }
    }

    void KeyPresses()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (skills[0] != null && skills[0].remainingCooldown <= 0)
            {
                Use(skills[0]);
            }
            else { PlaySound(error); }
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (skills[1] != null && skills[1].remainingCooldown <= 0)
            {
                Use(skills[1]);
            }
            else { PlaySound(error); }
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (skills[2] != null && skills[2].remainingCooldown <= 0)
            {
                Use(skills[2]);
            }
            else { PlaySound(error); }
        }
    }

    void Cooldown()
    {
        foreach (SkillElement element in skills)
        {
            if (element.remainingCooldown > 0)
            {
                element.remainingCooldown -= Time.deltaTime;
                float roundedCd = Mathf.Round(element.remainingCooldown);
                element.coolDownText.text = roundedCd.ToString();
                element.load.fillAmount = (element.remainingCooldown / element.skill.cooldown);
            }
            else
            {
                element.load.color = Color.white;
            }
        }
    }

    void Use(SkillElement element)
    {
        element.skill.Trigger(PlayerController.inst.gameObject);
        element.remainingCooldown = element.skill.cooldown;
        element.load.color = Color.yellow;
    }

    void PlaySound(AudioClip clip)
    {
        aus.clip = clip;
        aus.Play();
    }
}
