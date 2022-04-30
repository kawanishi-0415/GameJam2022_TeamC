using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Uturu
{
    public class ReadyWindow : MonoBehaviour
    {
        public Animator Animator { get; private set; } = null;

        private void Awake()
        {
            Animator = GetComponent<Animator>();
        }

        public bool IsEndAnimation()
        {
            return Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f;
        }
    }
}
