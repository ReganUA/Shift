using UnityEngine;

public class button : MonoBehaviour
{
    public string sceneToLoadOnStart;
    public void LoadSceneOnStartButton()
    {
        sceneManager.LoadScene(sceneToLoadOnStart, 0);
    }
}
