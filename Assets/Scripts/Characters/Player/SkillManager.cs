using Characters.Player;
using Skills;
using UnityEngine;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour
{
    public static SkillManager inst = null;

    [System.Serializable]
    public class SkillSlot
    {
        public Skill skill;
        [ReadOnly]
        public float remainingCooldown;
        public Image load;
        public Text coolDownText;
    }

    public SkillSlot[] skills;
    public AudioClip skillReady;
    public AudioClip error;
    private AudioSource aus;

    private void Start()
    {
        inst = this;

        aus = GetComponent<AudioSource>();

        // Initialise skill cooldowns.
        if (skills != null && skills.Length > 0)
        {
            foreach (SkillSlot element in skills)
            {
                element.remainingCooldown = element.skill.cooldown;
            }
        }

        GetComponent<HealthManager>().OnDeath += delegate { enabled = false; };
    }

    private void Update()
    {
        if (skills != null && skills.Length > 0)//!GameController.inst.paused)
        {
            KeyPresses();
            Cooldown();
        }
    }

    private void KeyPresses()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) || (Input.GetButton("L1")))
        {
            if (skills[0] != null && skills[0].remainingCooldown <= 0)
            {
                UseSkill(skills[0]);
            }
            else { SkillNotReady(); }
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) || (Input.GetButton("R1")))
        {
            if (skills[1] != null && skills[1].remainingCooldown <= 0)
            {
                UseSkill(skills[1]);
            }
            else { SkillNotReady(); }
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (skills[2] != null && skills[2].remainingCooldown <= 0)
            {
                UseSkill(skills[2]);
            }
            else { SkillNotReady(); }
        }
    }

    private void Cooldown()
    {
        foreach (SkillSlot skill in skills)
        {
            if (skill.remainingCooldown > 0)
            {
                skill.remainingCooldown -= Time.deltaTime;
                float roundedCd = Mathf.Round(skill.remainingCooldown);
                skill.coolDownText.text = roundedCd.ToString();
                skill.load.fillAmount = (skill.remainingCooldown / skill.skill.cooldown);
            }
            else
            {
                skill.load.color = Color.white;
            }
        }
    }

    private void UseSkill(SkillSlot skillSlot)
    {
        skillSlot.skill.Trigger(PlayerController.inst.gameObject);
        skillSlot.remainingCooldown = skillSlot.skill.cooldown;
        skillSlot.load.color = Color.yellow;
    }

    private void SkillNotReady()
    {
        PlaySound(error);
    }
    private void PlaySound(AudioClip clip)
    {
        aus.clip = clip;
        aus.Play();
    }
}
