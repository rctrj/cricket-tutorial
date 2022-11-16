using UnityEngine;

namespace Test
{
    public class AnimationRunner : MonoBehaviour
    {
        [SerializeField] private Animator animator;

        public void RunAnimation(string animName)
        {
            animator.Play(animName);
        }
    }
}