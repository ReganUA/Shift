using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneManager : MonoBehaviour
{
    public static sceneManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public static void ReloadScene(float delay = 0)
    {
        LoadScene(SceneManager.GetActiveScene().name, delay);
    }
    public static void LoadScene(string scene, float delay)
    {
        if(delay > 0)
        {
            instance.StartCoroutine(instance.LoadSceneCoroutine(scene, delay));
        }
        else
        {
            instance.LoadSceneImmediate(scene);
        }
    }
    private IEnumerator LoadSceneCoroutine(string scene, float delay)
    {
        yield return new WaitForSeconds(delay);
        LoadSceneImmediate(scene);
    }
    private void LoadSceneImmediate(string scene)
    {
        SceneManager.LoadScene(scene);
    }
}
