using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashCloneHandler : MonoBehaviour
{
    [SerializeField] Vector3 _targetPosition;
    private float _speed;
    private float _destroyTimer = .5f;

    public float Speed { get => _speed; set { _speed = value; } }

    void Update()
    {
        _targetPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
        MoveClone();
        _speed += Time.deltaTime;
        _destroyTimer -= Time.deltaTime;
        if(_destroyTimer <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    private void MoveClone()
    {
        float step = _speed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position,_targetPosition, step);
    }

    public void SetSpeed(float speed)
    {
        _speed = speed;
    }

}
