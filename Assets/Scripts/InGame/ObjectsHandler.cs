using UnityEngine;
using System.Collections.Generic;


public class ObjectsHandler : MonoBehaviour
{
    [SerializeField] private List<GameObject> _gameObjectsList = new List<GameObject>();
    void Start()
    {
        SetGameObjectList();
        SetSortOrder();
    }

    private void SetGameObjectList()
    {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Object");
        foreach(GameObject gameObject in gameObjects)
        {
            _gameObjectsList.Add(gameObject);
        }

        GameObject[] gameObstacles = GameObject.FindGameObjectsWithTag("Obstacle");
        foreach(GameObject gameObject in gameObstacles)
        {
            _gameObjectsList.Add(gameObject);
        }
    }

    private void SetSortOrder()
    {
        SortOrderUtilities sortOrderUtilities = GameManager.instance.GetSortOrderUtilities();

        foreach(GameObject gameObject in _gameObjectsList)
        {
            sortOrderUtilities.SetSortOrder(gameObject);
        }
    }
}
