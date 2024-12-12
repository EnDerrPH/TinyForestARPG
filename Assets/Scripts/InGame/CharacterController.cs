using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
public class CharacterController : LivingObjects
{
    [SerializeField] protected float _dashPower;
    [SerializeField] protected GameObject _dashGameObject;
    [SerializeField] protected Sprite _dashUp;
    [SerializeField] protected Sprite _dashDown;
    [SerializeField] protected Sprite _dashLeft;
    [SerializeField] protected Sprite _dashRight;
    [SerializeField] private SkillSlotHandler _GlobalSlot;
    [SerializeField] private SkillSlotHandler _SkillSlot;   
    protected Vector3 _moveInput;
    private ActionInput _actionInput;
    private bool _isMoving;
    public UnityEvent OnDashEvent;
 
    public override void Start()
    {
        base.Start();
        SetPlayerCharacterData();
        SetActionInput();
    }

    public override void Update()
    {
        base.Update();
        CheckIfMoving();
    }

    private void SetPlayerCharacterData()
    {
        PlayerCharacterData playerCharacterData =  GameManager.instance.PlayerCharacterData;
        if(playerCharacterData == null)
        {
            return;
        }
        _objectAngle = ObjectAngle.South;
        playerCharacterData.InitializeStats();
        playerCharacterData.SetPlayerCharacterStats();
        playerCharacterData.SetSubStats();
        _objectAnimator.runtimeAnimatorController = playerCharacterData.CharacterData.GetCharacterController();
        _classHitPrefab = playerCharacterData.CharacterData.GetCharacterHit();
        _dashUp = playerCharacterData.CharacterData.DashUp;
        _dashDown = playerCharacterData.CharacterData.DashDown;
        _dashLeft = playerCharacterData.CharacterData.DashLeft;
        _dashRight = playerCharacterData.CharacterData.DashRight;
        _hp = playerCharacterData.MaxHP;
    }

    private void SetActionInput()
    {
        _actionInput = new ActionInput();
        _actionInput.Enable();
        _actionInput.Player.Dash.performed += OnDash;
    }

    private void SetNormalAttack()
    {
        _objectAnimator.SetBool("NormalAttack", false); 
        _rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        _moveSpeed =  8f;
    }


    private void CheckIfMoving()
    {
        if(_moveInput.x == 0 && _moveInput.y == 0)
        {
            _isMoving = false;
            return;
        }
        _isMoving = true;
    }

    public override void OnMove()
    {
        if(_objectAnimator.GetBool("NormalAttack"))
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
        _rb.MovePosition(_rb.position + movement);
        SetCharacterAngle();
        _sortOrderUtilities.SetSortOrder(this.gameObject);
    }

    private void SetCharacterAngle()
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
            PlayOneShot(.3f,_characterData.DashAudio);
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

    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.collider.tag == "Cave")
        {
           //enter CaveDungeon
        }
    }
}
