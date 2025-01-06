using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using System.Collections.Generic;
public class CharacterController : BaseActorHandler
{
    [SerializeField] protected float _dashPower;
    [SerializeField] protected GameObject _dashGameObject;
    [SerializeField] protected Sprite _dashUp;
    [SerializeField] protected Sprite _dashDown;
    [SerializeField] protected Sprite _dashLeft;
    [SerializeField] protected Sprite _dashRight;
    [SerializeField] private SkillSlotHandler _GlobalSlot;
    [SerializeField] private SkillSlotHandler _SkillSlot;   
    private PlayerCharacterData _playerCharacterData;
    protected Vector2 _lastPosition;
    protected float _stepInterval = .15f;
    private ActionInput _actionInput;
    public UnityEvent OnDashEvent, OnMovementEvent, OnHitEvent;
 
    public override void Start()
    {
        base.Start();
        SetPlayerCharacterData();
        SetActionInput();
    }

    public override void Update()
    {
        base.Update();
    }
    private void SetPlayerCharacterData()
    {
        _playerCharacterData =  GameManager.instance.PlayerCharacterData;
        if(_playerCharacterData == null)
        {
            return;
        }
        _objectAngle = ObjectAngle.South;
        _playerCharacterData.InitializeStats();
        _playerCharacterData.SetPlayerCharacterStats();
        _playerCharacterData.SetSubStats();
        _actorAnimator.runtimeAnimatorController = _playerCharacterData.CharacterData.GetCharacterController();
        _hitPrefab = _playerCharacterData.CharacterData.GetHitPrefab();
        _dashUp = _playerCharacterData.CharacterData.DashUp;
        _dashDown = _playerCharacterData.CharacterData.DashDown;
        _dashLeft = _playerCharacterData.CharacterData.DashLeft;
        _dashRight = _playerCharacterData.CharacterData.DashRight;
        _currentHP = _playerCharacterData.MaxHP;
    }

    private void SetActionInput()
    {
        _actionInput = new ActionInput();
        _actionInput.Enable();
        _actionInput.Player.Dash.performed += OnDash;
    }

    public override void OnMove()
    {
        if(_actorAnimator.GetBool("IsAttacking"))
        {
            return;
        }
        _moveInput = _actionInput.Player.Move.ReadValue<Vector2>();
        if(_moveInput.x != 0 && _moveInput.y != 0)
        {
            _moveInput.y = 0f;
            _moveInput.x = 0f;
            SetObjectAnimatorFloat(0f,0f);
        }
        SetObjectAnimatorFloat(_moveInput.x, _moveInput.y);
        Vector2 movement = _moveInput * _moveSpeed;
        movement *= Time.deltaTime;
        _lastPosition = movement;
        _rb.MovePosition(_rb.position + movement);
        SetCharacterAngle();
        _sortOrderUtilities.SetSortOrder(this.gameObject);
        if (movement.magnitude > 0)
        {
            OnMovementEvent.Invoke();
            PlayFootstepSound();
            _stepInterval -= Time.deltaTime;
            if(_stepInterval <= 0f)
            {
                _lastPosition = this.transform.position;
                _stepInterval = .15f;
            }
        }
    }

    private void OnDash(InputAction.CallbackContext ctx)
    {
        if(_GlobalSlot.IsCooldown)
        {
            return;
        }
        if(ctx.performed)
        {
            int sortingNumber = this.gameObject.GetComponent<SpriteRenderer>().sortingOrder;   
            switch(_objectAngle)
            {
                case ObjectAngle.North:
                    _rb.AddForce(new Vector2(0f ,_dashPower));
                    CreateDashClone(_dashUp, sortingNumber);
                    break;
                case ObjectAngle.South:
                    _rb.AddForce(new Vector2(0f ,-_dashPower));
                    CreateDashClone(_dashDown, sortingNumber);
                    break;
                case ObjectAngle.West:
                    _rb.AddForce(new Vector2(-_dashPower ,0f));
                    CreateDashClone(_dashLeft, sortingNumber);
                    break;
                case ObjectAngle.East:
                    _rb.AddForce(new Vector2(_dashPower ,0f));
                    CreateDashClone(_dashRight, sortingNumber);
                    break;
            }
            PlayOneShot(1f,_characterData.DashSFX);
            OnDashEvent.Invoke();
        }
    }
    private void CreateDashClone(Sprite dashSprite, int sortingNumber)
    {
        int maxClone = 3;
        float speed = 4f;
        float ColorA = .3f;
        for(int i = 0; i < maxClone ; i++)
        {
            speed += 2f;
            SpriteRenderer dashUpClone = Instantiate(_dashGameObject, this.transform.position,Quaternion.identity).GetComponent<SpriteRenderer>();
            dashUpClone.sprite = dashSprite;
            dashUpClone.material.color = new Color(1f, 1f, 1f, ColorA);
            dashUpClone.sortingOrder = sortingNumber;
            DashCloneHandler cloneHandler = dashUpClone.GetComponent<DashCloneHandler>();
            cloneHandler.SetSpeed(speed);
            ColorA += .3f;
            if(_objectAngle == ObjectAngle.North)
            {
                sortingNumber -= 1;
            }
            if(_objectAngle == ObjectAngle.South)
            {
                sortingNumber += 1;
            }
        }
    }

    private void PlayFootstepSound()
    {
        if (!_audioSource.isPlaying)  // Ensure the previous step sound isn't still playing
        {
            // Alternate between the two step sounds
            if (Random.value > 0.5f)  // Randomly choose the clip to play
            {
                _audioSource.clip = _characterData.Step1SFX;
            }
            else
            {
                _audioSource.clip = _characterData.Step2SFX;
            }
            _audioSource.volume = .2f;
            _audioSource.Play();
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.collider.tag == "EnemyWeapon")
        {
            HitPrefabHandler hitPrefabHandler = col.gameObject.GetComponent<EnemyController>().HitPrefabHandler;
           _currentHP -= hitPrefabHandler.Damage;
           OnHitEvent.Invoke();
        }
    }
}
