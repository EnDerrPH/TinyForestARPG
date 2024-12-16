using UnityEngine;
using UnityEngine.InputSystem;

public class LivingObjects : MonoBehaviour
{
    [SerializeField] protected GameObject _classHitPrefab;
    [SerializeField] protected float _moveSpeed;
    protected CharacterData _characterData;
    protected AudioSource _audioSource;
    protected int _hp;
    protected Animator _objectAnimator;
    protected Rigidbody2D _rb;
    protected ObjectAngle _objectAngle;
    protected SortOrderUtilities _sortOrderUtilities;
    protected float _prefabOffsetPos = .7f;
    public ObjectAngle ObjectAngle { get => _objectAngle; set { _objectAngle = value; } }
    public Animator ObjectAnimator { get => _objectAnimator; set { _objectAnimator = value; } }

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

    private void SetObjectData()
    {
        _objectAnimator = GetComponent<Animator>();
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
        if(_objectAnimator.GetBool("NormalAttack"))
        {
            return;
        }
        if(Mouse.current.leftButton.wasPressedThisFrame)
        {
            PlayOneShot(1f,_characterData.AttackSFX);
            _objectAnimator.SetBool("NormalAttack", true); 
            _moveSpeed = 0f;
            _rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }

    public virtual void OnDeath()
    {

    }

    public virtual void SetObjectAnimatorFloat(float xValue, float yValue)
    {
        if(_objectAnimator == null)
        {
            return;
        }
        _objectAnimator.SetFloat("moveX", xValue);
        _objectAnimator.SetFloat("moveY", yValue);
    }

    public virtual void InstantiateAttack()
    {
        switch(_objectAngle)
            {
                case ObjectAngle.North:
                    Instantiate(_classHitPrefab, new Vector3(this.transform.position.x , this.transform.position.y + _prefabOffsetPos , this.transform.position.z), Quaternion.identity);
                break;
                case ObjectAngle.South:
                    Instantiate(_classHitPrefab, new Vector3(this.transform.position.x , this.transform.position.y - _prefabOffsetPos , this.transform.position.z), Quaternion.Euler(0,0,180));
                break;
                case ObjectAngle.West:
                    Instantiate(_classHitPrefab, new Vector3(this.transform.position.x - _prefabOffsetPos , this.transform.position.y + .2f, this.transform.position.z), Quaternion.Euler(0,0,90));
                break;
                case ObjectAngle.East:
                    Instantiate(_classHitPrefab, new Vector3(this.transform.position.x + _prefabOffsetPos , this.transform.position.y + .2f , this.transform.position.z), Quaternion.Euler(0,0,270));
                break;
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
