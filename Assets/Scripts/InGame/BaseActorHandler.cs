using UnityEngine;
using UnityEngine.InputSystem;

public class BaseActorHandler : MonoBehaviour
{
    [SerializeField] protected HitPrefabHandler _hitPrefab;
    [SerializeField] protected float _moveSpeed;
    protected CharacterData _characterData;
    protected AudioSource _audioSource;
    protected int _currentHP;
    protected Animator _actorAnimator;
    protected Rigidbody2D _rb;
    protected Vector3 _moveInput;
    protected ObjectAngle _objectAngle;
    protected SortOrderUtilities _sortOrderUtilities;
    protected float _prefabOffsetPos = 1f;
    protected float _baseMoveSpeed = 8f;
    public ObjectAngle ObjectAngle { get => _objectAngle; set { _objectAngle = value; } }
    public Animator ActorAnimator { get => _actorAnimator; set { _actorAnimator = value; } }
    public int CurrentHP { get =>_currentHP; set {_currentHP = value; } }
    public HitPrefabHandler HitPrefabHandler { get =>_hitPrefab; set {_hitPrefab = value; } }

    public virtual void OnAwake()
    {

    }

    public virtual void OnEnable()
    {

    }

    public virtual void Disable()
    {
        
    }

    public virtual void Start()
    {
        AddListener();
        SetObjectData();
    }

    public virtual void Update()
    {
        OnAttack();
    }

    public virtual void FixedUpdate()
    {
        OnMove();
    }

    public virtual void SetObjectData()
    {
        _actorAnimator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        _sortOrderUtilities = GameManager.instance.GetSortOrderUtilities();
        _rb = GetComponent<Rigidbody2D>();
        _characterData = GameManager.instance.PlayerCharacterData.CharacterData;
    }

    public virtual void OnMove()
    {

    }

    public virtual void OnAttack()
    {
        if(_actorAnimator.GetBool("IsAttacking"))
        {
            return;
        }
        if(Mouse.current.leftButton.wasPressedThisFrame)
        {
           SetAttack(_characterData.AttackSFX);
        }
    }

    public virtual void SetAttack(AudioClip audioClip)
    {
        PlayOneShot(1f,audioClip);
        _actorAnimator.SetBool("IsAttacking", true); 
        _moveSpeed = 0f;
        _rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    public virtual void AttackDone()
    {
        _actorAnimator.SetBool("IsAttacking", false); 
        _rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        _moveSpeed =  _baseMoveSpeed;
    }

    public virtual void OnDeath()
    {

    }

    public virtual void SetObjectAnimatorFloat(float xValue, float yValue)
    {
        if(_actorAnimator == null)
        {
            return;
        }
        _actorAnimator.SetFloat("moveX", xValue);
        _actorAnimator.SetFloat("moveY", yValue);
    }

    public virtual void InstantiateAttack()
    {
        switch(_objectAngle)
            {
                case ObjectAngle.North:
                    Instantiate(_hitPrefab, new Vector3(this.transform.position.x , this.transform.position.y + _prefabOffsetPos , this.transform.position.z), Quaternion.identity);
                break;
                case ObjectAngle.South:
                    Instantiate(_hitPrefab, new Vector3(this.transform.position.x , this.transform.position.y - _prefabOffsetPos , this.transform.position.z), Quaternion.Euler(0,0,180));
                break;
                case ObjectAngle.West:
                    Instantiate(_hitPrefab, new Vector3(this.transform.position.x - _prefabOffsetPos , this.transform.position.y + .2f, this.transform.position.z), Quaternion.Euler(0,0,90));
                break;
                case ObjectAngle.East:
                    Instantiate(_hitPrefab, new Vector3(this.transform.position.x + _prefabOffsetPos , this.transform.position.y + .2f , this.transform.position.z), Quaternion.Euler(0,0,270));
                break;
            }
    }

    public virtual void SetCharacterAngle()
    {
        if(_moveInput.x > .1f)
        {
            _objectAngle = ObjectAngle.East;
        }
        else if(_moveInput.x < -.1f)
        {
            _objectAngle = ObjectAngle.West;
        }

        if(_moveInput.y > .1f)
        {
            _objectAngle = ObjectAngle.North;
        }
        else if(_moveInput.y < -.1f)
        {
            _objectAngle = ObjectAngle.South;
        }
    }
    
    public virtual void AddListener()
    {

    }

    public virtual void PlayOneShot(float volume , AudioClip audioClip)
    {
        _audioSource.Stop();
        _audioSource.volume = volume;
        _audioSource.PlayOneShot(audioClip);
    }

}
