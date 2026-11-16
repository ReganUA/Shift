using UnityEngine;

public class playerGround : MonoBehaviour
{
    private void LateUpdate() // Використовуємо LateUpdate, щоб спрацювати після фізики та анімацій
    {
        if (transform.position.z != 0)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
        }
    }
}
