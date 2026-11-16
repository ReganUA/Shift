using UnityEngine;

public class keepAnimation : MonoBehaviour
{
    private Animator _animator;

    void Awake()
    {
        _animator = GetComponent<Animator>();
        // Ця магічна стрічка каже Unity не скидати аніматор при вимкненні
        _animator.keepAnimatorStateOnDisable = true;
    }
}
