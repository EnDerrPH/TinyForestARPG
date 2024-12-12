using UnityEngine;

public abstract class DamageHandler : MonoBehaviour
{
    protected ObjectAngle _objectAngle;
    protected CharacterController _characterController;
    protected GameObject _characterObj;
    protected float _destroyTimer = 1f;

    public virtual void Start()
    {
        _characterObj = GameObject.FindGameObjectWithTag("Player");
        _characterController =  _characterObj.GetComponent<CharacterController>();
        _objectAngle = _characterController.ObjectAngle;
    }

    public virtual void Update()
    {
        DestroySelf();
    }

    public void DestroySelf()
    {
        _destroyTimer -= Time.deltaTime;
        if(_destroyTimer <= 0)
        {
           Destroy(this.gameObject);
        }
    }
    public virtual void OnCollisionEnter2D(Collision2D col)
    {
        Destroy(this.gameObject);
    }
}
