using UnityEngine.Events;
using UnityEngine;
using UnityEngine.UI;

public class SkillSlotHandler : MonoBehaviour
{
    [SerializeField] private Image _skillImage;
    [SerializeField] private Image _CDImage;
    [SerializeField] private Skill _skill;
    [SerializeField] private SkillSlotType _skillSlotType;
    private CharacterController _characterController;
    private bool _isCooldown;
    private float _cooldownTimer = 1f;
    private float _skillCooldown;
    public Image CDImage { get => _CDImage; set { _CDImage = value; } }
    public Image SkillImage { get => _skillImage; set { _skillImage = value; } }
    public bool IsCooldown { get => _isCooldown; set { _isCooldown = value; } }

    private void Start()
    {
        _characterController = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController>();
        _characterController.OnDashEvent.AddListener(SetCDTimer);
        SetSkillData();
    }

    private void Update()
    {
        OnSkillCooldown();
    }

    private void OnSkillCooldown()
    {
        if(!_isCooldown)
        {
            return;
        }
        _cooldownTimer -= ( _skillCooldown * Time.deltaTime);
        _CDImage.fillAmount = _cooldownTimer;

        if(_cooldownTimer < 0)
        {
            _isCooldown = false;
            _cooldownTimer = 1f;
        }

    }

    private void SetCDTimer()
    {
        _CDImage.fillAmount = 1;
        _isCooldown = true;
    }

    private void SetSkillData()
    {
        if(_skill == null)
        {
            return;
        }

        _skillImage.sprite = _skill.SkillImage;
        _skillCooldown = _skill.SkillCooldown;
    }
}
