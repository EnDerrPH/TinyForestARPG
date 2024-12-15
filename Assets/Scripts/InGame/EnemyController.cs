using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Events;

public class EnemyController : LivingObjects
{
    [SerializeField] private EnemyData _enemyData;
    [SerializeField] private Transform _damagePopUp;
    private SpriteRenderer _spriteRenderer;
    private MapHandler _mapHandler;
    private Vector2 _moveDirection;
    private float _changeDirectionTime = 2f;
    private float _timeToChangeDirection;
    private Bounds _tilemapBounds;
    private Vector2 _targetPosition;
    private Vector2 lastPosition;
    private bool _isDissolveOut;
    private bool _isDissolveIn = true;
    private Collider2D _boxCollider;
    private float _dissolveAmount = 2f;
    private float _dissolveTimer = .7f;
    private Renderer _renderer;
    private PlayerCharacterData _playerCharacterData;
    public UnityEvent OnDeathEvent;
    public EnemyData EnemyData { get => _enemyData; set { _enemyData = value; } }
    public int HP { get =>_hp; set {_hp = value; } }

    public override void Start()
    {
        base.Start();
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
        _targetPosition = _rb.position + _moveDirection * _moveSpeed * Time.deltaTime;

        _targetPosition.x = Mathf.Clamp(_targetPosition.x, _tilemapBounds.min.x, _tilemapBounds.max.x);
        _targetPosition.y = Mathf.Clamp(_targetPosition.y, _tilemapBounds.min.y, _tilemapBounds.max.y);
        

        _rb.MovePosition(_targetPosition);
        _sortOrderUtilities.SetSortOrder(this.gameObject);
        Vector2 velocity = (_rb.position - lastPosition) / Time.fixedDeltaTime;

        lastPosition = _rb.position;

        float xValue = Mathf.Clamp(velocity.x, -1f, 1f);
        float yValue = Mathf.Clamp(velocity.y, -1f, 1f);

        SetObjectAnimatorFloat(xValue, yValue);
    }

    private void SetEnemyData()
    {
        _mapHandler = GameObject.FindGameObjectWithTag("Tilemap").GetComponent<MapHandler>();
        _enemyData = _mapHandler.EnemyData;
        lastPosition = _rb.position;
        TilemapRenderer tilemapRenderer = GameObject.FindGameObjectWithTag("Tilemap").GetComponent<TilemapRenderer>();
        _tilemapBounds = tilemapRenderer.bounds;
        _timeToChangeDirection = _changeDirectionTime;
        _boxCollider = GetComponent<BoxCollider2D>();
        _renderer = GetComponent<Renderer>();
        _playerCharacterData = GameManager.instance.PlayerCharacterData;
        this.GetComponent<SpriteRenderer>().sprite = _enemyData.StartingSprite;
        _objectAnimator.runtimeAnimatorController = _enemyData.AnimatorController;
        _hp = _enemyData.HP;
        _spriteRenderer = GetComponent<SpriteRenderer>();
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
            _timeToChangeDirection = _changeDirectionTime;
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
