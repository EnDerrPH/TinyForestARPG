using UnityEngine;

public class HitPrefabHandler : MonoBehaviour
{
    [SerializeField] private int _damage;

    public int Damage { get => _damage; set { _damage = value; } }

    public void SetDamage(int Damage)
    {
        _damage = Damage;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag == "Player")
        {
            this.gameObject.SetActive(false);
        }
    }
}
