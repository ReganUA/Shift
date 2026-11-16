using UnityEngine;
using UnityEngine.SceneManagement;

public class spike : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            gameManager.instance.GameEnd();
        }
    }
}
