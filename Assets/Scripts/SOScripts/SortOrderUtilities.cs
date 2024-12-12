using UnityEngine;

[CreateAssetMenu(fileName = "SortOrderUtilities", menuName = "Scriptable Objects/SortOrderUtilities")]
public class SortOrderUtilities : ScriptableObject
{
    public void SetSortOrder(GameObject gameObject)
    {
        if(gameObject.GetComponent<SpriteRenderer>() == null)
        {
            return;
        }
        SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.sortingOrder = Mathf.RoundToInt(-gameObject.transform.position.y * 3f);
    }
}
