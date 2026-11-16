using System.Collections.Generic;
using UnityEngine;

public class particlesSpawner : MonoBehaviour
{
    [Header("Settings")]
    public particle prefab;
    public float spawnSpeed = 0.1f;

    [Header("Particle Appearance")]
    public float particleLifeTime = 2f;
    public float particleSpeed = 3f;
    public AnimationCurve particleSizeOverLife = AnimationCurve.Linear(0, 1, 1, 0);

    [Header("Spawn Zone (Local Space)")]
    public Vector3[] spawnZone = new Vector3[2];

    private float spawnSpeedCounter;

    private void Start()
    {
        // Ініціалізація пулу
        List<particle> spawnedParticles = ObjectPoolManager.instance.Prewarm(prefab, 50);
        foreach (particle p in spawnedParticles)
        {
            p.transform.SetParent(transform);
        }

        // ВИКЛИК: Створюємо стовп при старті
        PreWarmPillar(25f, 40); // Довжина 25, кількість частинок 40
    }

    private void PreWarmPillar(float length, int count)
    {
        for (int i = 0; i < count; i++)
        {
            // progress вздовж довжини стовпа (від 0 до 1)
            float progress = (float)i / count;

            // Вираховуємо позицію
            float currentDistance = progress * length;
            float xNoise = Random.Range(spawnZone[0].x, spawnZone[1].x);
            float zNoise = Random.Range(spawnZone[0].z, spawnZone[1].z);

            Vector3 localPos = new Vector3(xNoise, currentDistance, zNoise);
            Vector3 worldPos = transform.TransformPoint(localPos);

            particle p = ObjectPoolManager.SpawnObject(prefab, worldPos, transform.rotation);

            // Передаємо налаштування
            p.lifeTime = particleLifeTime;
            p.speed = particleSpeed;
            p.curve = particleSizeOverLife;

            // РОЗРАХУНОК ЧАСУ:
            // Якщо частинка пролетіла currentDistance зі швидкістю particleSpeed,
            // значить вона вже "живе" стільки секунд:
            float timeAlreadyLived = currentDistance / particleSpeed;

            // Запускаємо частинку з уже пройденим часом
            p.OnSpawn(timeAlreadyLived);

            p.transform.SetParent(transform);
        }
    }
    private void Update()
    {
        if (spawnSpeedCounter <= 0f)
        {
            SpawnParticle();
            spawnSpeedCounter = spawnSpeed;
        }
        else
        {
            spawnSpeedCounter -= Time.deltaTime;
        }
    }

    private void SpawnParticle()
    {
        // 1. Вираховуємо випадкову точку в ЛОКАЛЬНИХ координатах
        float x = Random.Range(Mathf.Min(spawnZone[0].x, spawnZone[1].x), Mathf.Max(spawnZone[0].x, spawnZone[1].x));
        float y = Random.Range(Mathf.Min(spawnZone[0].y, spawnZone[1].y), Mathf.Max(spawnZone[0].y, spawnZone[1].y));
        float z = Random.Range(Mathf.Min(spawnZone[0].z, spawnZone[1].z), Mathf.Max(spawnZone[0].z, spawnZone[1].z));

        Vector3 localPos = new Vector3(x, y, z);

        // 2. transform.TransformPoint конвертує локальну точку в світову з урахуванням повороту та масштабу
        Vector3 worldPos = transform.TransformPoint(localPos);

        // Спавнимо об'єкт у правильній світовій позиції
        // Передаємо transform.rotation, якщо хочеш, щоб частки теж були повернуті як спавнер
        particle spawnedParticle = ObjectPoolManager.SpawnObject(prefab, worldPos, transform.rotation);

        spawnedParticle.lifeTime = particleLifeTime;
        spawnedParticle.speed = particleSpeed;
        spawnedParticle.curve = particleSizeOverLife;
        spawnedParticle.OnSpawn();

        spawnedParticle.transform.SetParent(transform);
    }

    private void OnDrawGizmosSelected()
    {
        // Встановлюємо матрицю Гізмос рівною матриці об'єкта
        // Це змушує всі наступні Gizmos малюватися в локальному просторі об'єкта
        Gizmos.matrix = transform.localToWorldMatrix;

        Gizmos.color = Color.green;

        // Тепер центр і розмір рахуються відносно (0,0,0) об'єкта
        Vector3 localCenter = (spawnZone[0] + spawnZone[1]) / 2;
        Vector3 localSize = new Vector3(
            Mathf.Abs(spawnZone[0].x - spawnZone[1].x),
            Mathf.Abs(spawnZone[0].y - spawnZone[1].y),
            Mathf.Abs(spawnZone[0].z - spawnZone[1].z)
        );

        Gizmos.DrawWireCube(localCenter, localSize);

        // Скидаємо матрицю назад до світової, щоб не зламати інші Гізмос
        Gizmos.matrix = Matrix4x4.identity;
    }
}