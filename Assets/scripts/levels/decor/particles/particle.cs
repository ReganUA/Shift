using UnityEngine;

public class particle : MonoBehaviour
{
    [HideInInspector] public int PrefabID;

    public float lifeTime;
    public float speed;
    public AnimationCurve curve;

    private float timer;
    private Vector3 initialScale;

    private void Awake()
    {
        initialScale = transform.localScale;
    }

    // Додаємо параметр startTime
    public void OnSpawn(float startTime = 0f)
    {
        timer = startTime;
        UpdateState(); // Відразу оновлюємо стан, щоб не було кадру з неправильним розміром
    }

    private void Update()
    {
        timer += Time.deltaTime;
        UpdateState();
    }

    private void UpdateState()
    {
        float progress = timer / lifeTime;

        if (progress >= 1f)
        {
            Deactivate();
            return;
        }

        // Рух
        transform.Translate(Vector3.up * speed * Time.deltaTime);

        // Масштаб за кривою
        if (curve != null)
        {
            float scaleMultiplier = curve.Evaluate(progress);
            transform.localScale = initialScale * scaleMultiplier;
        }
    }

    private void Deactivate()
    {
        ObjectPoolManager.ReturnToPool(this);
    }
}