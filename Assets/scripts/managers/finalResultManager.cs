using UnityEngine;

public class finalResultManager : MonoBehaviour
{
    public static finalResultManager instance;
    public static float totalTimePlayed = 0f;
    public static int deaths = 0;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    private void Update()
    {
        totalTimePlayed += Time.deltaTime;
    }
}
