using UnityEngine;
using System.Collections.Generic;

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager instance;

    // Використовуємо Dictionary, де ключ - ID префаба, а значення - Стек вільних об'єктів
    private static Dictionary<int, Stack<particle>> inactivePools = new Dictionary<int, Stack<particle>>();

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public static particle SpawnObject(particle prefab, Vector3 position, Quaternion rotation)
    {
        int key = prefab.gameObject.GetInstanceID();

        // Перевіряємо, чи є такий пул і чи є в ньому вільні об'єкти
        if (inactivePools.TryGetValue(key, out Stack<particle> pool) && pool.Count > 0)
        {
            particle obj = pool.Pop(); // Швидко дістаємо останній

            obj.transform.position = position;
            obj.transform.rotation = rotation;
            obj.gameObject.SetActive(true);

            // Викликаємо метод ініціалізації на самому партіклі (якщо треба скинути таймери)
            obj.OnSpawn();
            return obj;
        }

        // Якщо пустий - створюємо новий
        particle newObj = Instantiate(prefab, position, rotation);
        newObj.PrefabID = key; // Запам'ятовуємо, з якого пулу цей об'єкт
        return newObj;
    }

    public static void ReturnToPool(particle obj)
    {
        obj.gameObject.SetActive(false);
        int key = obj.PrefabID;

        if (!inactivePools.ContainsKey(key))
        {
            inactivePools[key] = new Stack<particle>();
        }

        inactivePools[key].Push(obj); // Кладемо назад у купу вільних
    }

    private void OnDestroy()
    {
        // Очищаємо словник, щоб не тримати посилання на видалені об'єкти
        inactivePools.Clear();
    }
    public List<particle> Prewarm(particle prefab, int count)
    {
        int key = prefab.gameObject.GetInstanceID();
        if (!inactivePools.ContainsKey(key))
            inactivePools[key] = new Stack<particle>();
        List<particle> prewarmedObjects = new List<particle>();

        for (int i = 0; i < count; i++)
        {
            particle obj = Instantiate(prefab);
            obj.PrefabID = key;
            obj.gameObject.SetActive(false);
            inactivePools[key].Push(obj);
            prewarmedObjects.Add(obj);
        }
        return prewarmedObjects;
    }
}