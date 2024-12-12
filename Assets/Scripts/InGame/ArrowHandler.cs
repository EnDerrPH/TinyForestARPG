using UnityEngine;

public class ArrowHandler : DamageHandler
{
    private float _speed = 15f;

    public override void Start()
    {
        base.Start();
        this.gameObject.GetComponent<SpriteRenderer>().sortingOrder = _characterObj.GetComponent<SpriteRenderer>().sortingOrder;
        _destroyTimer = 2f;
    }

    public override void Update()
    {
        DestroySelf();
        ArrowMovement();
    }

    private void ArrowMovement()
    {
        switch(_objectAngle)
        {
                case ObjectAngle.North:
                    transform.position += new Vector3(0, _speed * Time.deltaTime, 0);
                    break;
                case ObjectAngle.South:
                    transform.position += new Vector3(0, -_speed * Time.deltaTime, 0);
                    break;
                case ObjectAngle.West:
                    transform.position += new Vector3(-_speed * Time.deltaTime, 0, 0);
                    break;
                case ObjectAngle.East:
                    transform.position += new Vector3(_speed * Time.deltaTime, 0, 0);
                    break;
        }
    }
}
