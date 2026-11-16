using UnityEditor;
using UnityEngine;

public class levelEnd : MonoBehaviour
{
    public string nextLevelName;
    public GameObject playerLevelFinishEffect;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            sceneManager.LoadScene(nextLevelName, 2);
            playerLevelFinishEffect.SetActive(true);
            playerLevelFinishEffect.transform.SetParent(null);
            playerLevelFinishEffect.transform.position = gameManager.instance.player.transform.position;
            gameManager.instance.player.gameObject.SetActive(false);
            musicManager.instance.levelCompleted.Play();
        }
    }
}
