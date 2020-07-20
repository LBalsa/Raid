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
    }

    private void Update()
    {
        // Return if player is dead.
        if (!HealthManager.inst.IsAlive) { return; }

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
                Use(skills[0]);
            }
            else { PlaySound(error); }
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) || (Input.GetButton("R1")))
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

    private void Cooldown()
    {
        foreach (SkillSlot element in skills)
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

    private void Use(SkillSlot element)
    {
        element.skill.Trigger(PlayerController.inst.gameObject);
        element.remainingCooldown = element.skill.cooldown;
        element.load.color = Color.yellow;
    }

    private void PlaySound(AudioClip clip)
    {
        aus.clip = clip;
        aus.Play();
    }
}
