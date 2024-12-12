using UnityEngine;
using TMPro;

public class DamagePopUpHandler : MonoBehaviour
{
    [SerializeField] private TMP_Text _damageText;
    [SerializeField] private float _disappearTimer = 1f;
    [SerializeField] GameObject _parentObject;
    private Vector3 _startingPosition;
    Color StartingColor;
    private int _hitCount = 0;
    public int HitCount { get => _hitCount; set { _hitCount = value; } }

    void Awake()
    {
        _parentObject = transform.parent.gameObject;
    }

    void OnEnable()
    {
        this.transform.position = new Vector3(_parentObject.transform.position.x , _parentObject.transform.position.y +.2f, _parentObject.transform.position.z);
    }
    void Start()
    {
        
        StartingColor = _damageText.color;
    }

    private void Update()
    {
       TextMovement();
    }
    public void SetupDamage(int damageAmount)
    {
        _damageText.SetText(damageAmount.ToString());
    }

    private void TextMovement()
    {
        float moveYSpeed = 1f;
        transform.position += new Vector3(0,moveYSpeed) * Time.deltaTime;
       _disappearTimer -= Time.deltaTime;
        if(_disappearTimer < 0)
        {
            Color textColor = _damageText.color;
            float disappearSpeed = 5f;
           textColor.a -= disappearSpeed * Time.deltaTime;
           _damageText.color = textColor;
            if(textColor.a <= 0)
            {
                RestartText();
            }
        }
    }

    public void RestartPosition()
    {
        if(_hitCount > 1)
        {
            _disappearTimer = 1f;
            this.transform.position = new Vector3(_parentObject.transform.position.x , _parentObject.transform.position.y +.2f, _parentObject.transform.position.z);
            _damageText.color = StartingColor;
        }
    }

    public void RestartText()
    {
        _disappearTimer = 1f;
        _damageText.color = StartingColor;
        this.gameObject.SetActive(false);
    }
}
