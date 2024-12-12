using UnityEngine;

[CreateAssetMenu(fileName = "ColorUtilities", menuName = "Scriptable Objects/ColorUtilities")]
public class ColorUtilities : ScriptableObject
{
    public void SetObjectAlpha(GameObject gameObject , float colorA)
    {
        gameObject.GetComponent<SpriteRenderer>().material.color = new Color(1f, 1f, 1f, colorA);
    }
}
