using UnityEngine;
using UnityEngine.SceneManagement;

public class UIHandler : MonoBehaviour
{

    public virtual void Start()
    {
        AddListeners();
    }

    public virtual void AddListeners()
    {

    }

    public virtual void Open(GameObject screen)
    {
        screen.SetActive(true);
    }

    public virtual void Close(GameObject screen)
    {
        screen.SetActive(false);
    }

    public virtual void LoadScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }
}
