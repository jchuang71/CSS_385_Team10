using UnityEngine;

public class AutoDestroyAfterAnimation : MonoBehaviour
{
    void Start()
    {
        Animator anim = GetComponent<Animator>();
        float animLength = anim.GetCurrentAnimatorStateInfo(0).length;
        Destroy(gameObject, animLength);
    }
}
