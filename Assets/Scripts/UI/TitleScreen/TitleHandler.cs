using TMPro;
using UnityEngine;

public class TextEffectsExample : MonoBehaviour
{
    [SerializeField] private TMP_Text tmpText;

    private float currentFaceDilate = -1f;

    private float targetFaceDilate = -0.1f;

    public float dilateSpeed = 0.1f;

    public float underlaySoftnessDuration = 2.0f;

    private float currentUnderlaySoftness = 1.0f;

    private float targetUnderlaySoftness = 0.0f;

    private float underlaySoftnessTransitionSpeed;

    void Start()
    {
        if (tmpText == null)
        {
            tmpText = GetComponent<TMP_Text>();
        }

        Material mat = tmpText.fontMaterial;
        underlaySoftnessTransitionSpeed = (currentUnderlaySoftness - targetUnderlaySoftness) / underlaySoftnessDuration;
    }

    void Update()
    {
        if (tmpText != null)
        {
            if (currentFaceDilate < targetFaceDilate)
            {
                currentFaceDilate += dilateSpeed * Time.deltaTime;
            }
            Material mat = tmpText.fontMaterial;

            mat.SetFloat("_FaceDilate", currentFaceDilate);

            if (currentUnderlaySoftness > targetUnderlaySoftness)
            {
                currentUnderlaySoftness -= underlaySoftnessTransitionSpeed * Time.deltaTime;
                currentUnderlaySoftness = Mathf.Max(currentUnderlaySoftness, targetUnderlaySoftness);
            }
            mat.SetFloat("_UnderlaySoftness", currentUnderlaySoftness);
        }
    }
}
