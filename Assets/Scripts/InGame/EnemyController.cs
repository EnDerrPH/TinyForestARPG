using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Events;

public class EnemyController : LivingObjects
{
    [SerializeField] private EnemyData _enemyData;
    [SerializeField] private Transform _damagePopUp;
    private CharacterController _characterController;
    private Transform _playerTransform;
    private SpriteRenderer _spriteRenderer;
    private MapHandler _mapHandler;
    private Vector2 _moveDirection;
    private float _timeToChangeDirection = 2f;
    private float _chaseRange = 8f;
    private Bounds _tilemapBounds;
    private Vector2 _breadCrumbPosition;
    private Vector2 _targetPosition;
    private Vector2 lastPosition;
    private Vector2 _playerPosition;
    private bool _isDissolveOut;
    private bool _isDissolveIn = true;
    private Collider2D _boxCollider;
    private float _dissolveAmount = 2f;
    private float _dissolveTimer = .7f;
    private int _breadCrumbsCount = 0;
    private Renderer _renderer;
    private PlayerCharacterData _playerCharacterData;
    private EnemyState _enemyState;
    public UnityEvent OnDeathEvent;
    public EnemyData EnemyData { get => _enemyData; set { _enemyData = value; } }
    public int HP { get =>_hp; set {_hp = value; } }

    public override void Start()
    {
        base.Start();
        SetPlayerData();
        SetEnemyData();
        SetRandomDirection();
    }

    public override void Update()
    {
        base.Update();
        DissolveIn();
        DissolveOut();
        SetMovementDirection();
    }

    public override void FixedUpdate()
    {
        OnMove();
    }

    public override void OnAttack()
    {
        
    }

    public override void OnMove()
    {
        CheckPlayerDistance();
        if(_enemyState == EnemyState.Roaming)
        {
            _targetPosition = _rb.position + _moveDirection * _moveSpeed * Time.deltaTime;
            _targetPosition.x = Mathf.Clamp(_targetPosition.x, _tilemapBounds.min.x, _tilemapBounds.max.x);
            _targetPosition.y = Mathf.Clamp(_targetPosition.y, _tilemapBounds.min.y, _tilemapBounds.max.y); 
        }
        else
        {
            _breadCrumbPosition = _characterController.BreadCrumbsList[_breadCrumbsCount].transform.position;
            _targetPosition = Vector2.MoveTowards(_rb.position, _breadCrumbPosition, _moveSpeed * Time.deltaTime);
           float breadCrumbDistance = Vector2.Distance(this.transform.position,_breadCrumbPosition);
           if(breadCrumbDistance <= 1f)
           {
                _characterController.BreadCrumbsList[_breadCrumbsCount].gameObject.SetActive(false);
           }
        }

        _rb.MovePosition(_targetPosition);
        _sortOrderUtilities.SetSortOrder(this.gameObject);
        Vector2 velocity = (_rb.position - lastPosition) / Time.fixedDeltaTime;
        lastPosition = _rb.position;

        float xValue = Mathf.Clamp(velocity.x, -1f, 1f);
        float yValue = Mathf.Clamp(velocity.y, -1f, 1f);

        SetObjectAnimatorFloat(xValue, yValue);
        if(_targetPosition == _breadCrumbPosition)
        {
            _breadCrumbsCount += 1;
            if(_breadCrumbsCount >= _characterController.BreadCrumbsList.Count)
            {
                _breadCrumbsCount = 0;
            }
        }
    }

    private void CheckPlayerDistance()
    {
        float playerRange = Vector2.Distance(_playerTransform.position , this.transform.position);
        _enemyState = playerRange <= _chaseRange? EnemyState.Chasing : EnemyState.Roaming;
    }

    private void SetPlayerData()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        _playerTransform = playerObj.transform;
        _characterController = playerObj.GetComponent<CharacterController>();
    }

    private void SetEnemyData()
    {
        _mapHandler = GameObject.FindGameObjectWithTag("Tilemap").GetComponent<MapHandler>();
        _enemyData = _mapHandler.EnemyData;
        lastPosition = _rb.position;
        TilemapRenderer tilemapRenderer = GameObject.FindGameObjectWithTag("Tilemap").GetComponent<TilemapRenderer>();
        _tilemapBounds = tilemapRenderer.bounds;
        _boxCollider = GetComponent<BoxCollider2D>();
        _renderer = GetComponent<Renderer>();
        _playerCharacterData = GameManager.instance.PlayerCharacterData;
        this.GetComponent<SpriteRenderer>().sprite = _enemyData.StartingSprite;
        _objectAnimator.runtimeAnimatorController = _enemyData.AnimatorController;
        _hp = _enemyData.HP;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _enemyState = EnemyState.Roaming;
    }

    private void SetMoveSpeed(float speed)
    {
        _moveSpeed = speed;
    }

    private void SetRandomDirection()
    {
        int randomDirection = Random.Range(0, 4);
        
        switch (randomDirection)
        {
            case 0:
                _moveDirection = Vector2.up;
                break;
            case 1:
                _moveDirection = Vector2.down;
                break;
            case 2:
                _moveDirection = Vector2.left;
                break;
            case 3:
                _moveDirection = Vector2.right;
                break;
        }
    }

    private void CheckHP()
    {
        if(_hp > 0)
        {
            return;
        }
        _moveSpeed = 0;
        _boxCollider.enabled = false;
        _playerCharacterData.GainExperience(_enemyData.Experience);
        OnDeathEvent.Invoke();
        _isDissolveOut = true;
    }

    private void DissolveOut()
    {
        if(!_isDissolveOut)
        {
            return;
        }
        _dissolveAmount += _dissolveTimer * Time.deltaTime;
        Material instanceMaterial = _renderer.material;
        instanceMaterial.SetFloat("_DissolveAmount" , _dissolveAmount);

        if(_dissolveAmount >= 1f)
        {
            _isDissolveOut = false;
            Destroy(this.gameObject);
        }
    }

    private void DissolveIn()
    {
        if(!_isDissolveIn)
        {
            return;
        }
        _dissolveAmount -= _dissolveTimer * Time.deltaTime;
        Material instanceMaterial = _renderer.material;
        instanceMaterial.SetFloat("_DissolveAmount" , _dissolveAmount);

        if(_dissolveAmount <= 0f)
        {
            _isDissolveIn = false;
        }
    }

    private void SetMovementDirection()
    {
        _timeToChangeDirection -= Time.deltaTime;
        if (_timeToChangeDirection <= 0f)
        {
            SetRandomDirection();
            _timeToChangeDirection = 2f;
        }
    }

    private void DamagePopUp(int damageRecieve)
    {
        _damagePopUp.transform.position = this.transform.position;
        _damagePopUp.gameObject.SetActive(true);
        DamagePopUpHandler damagePopUp = _damagePopUp.GetComponent<DamagePopUpHandler>();
        damagePopUp.SetupDamage(damageRecieve);
        damagePopUp.HitCount += 1;
        damagePopUp.RestartPosition();
    }

    private void CalculateDamageRecieve(int damageReceive)
    {
        int totalDamage = (int)(damageReceive - (_enemyData.Defence * .3));
        DamagePopUp(totalDamage);
       _hp =_hp - totalDamage;
        _moveSpeed = 3f;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag == "Weapon")
        {
            PlayOneShot(1f,_enemyData.HitSFX);
            _moveSpeed = 0f;
            int damageReceive = (int)_playerCharacterData.AttackPower;
            CalculateDamageRecieve(damageReceive);
            CheckHP();
        }
    }
}
