using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Uturu
{
    public class PlayerController : MonoBehaviour
    {
        public const float LOOSEN_DELAY = 1.0f;

        [SerializeField, Tooltip("à⁄ìÆéûÇÃSE")] private AudioClip m_moveClip = null;
        [SerializeField, Tooltip("ùÜÇﬁéûÇÃSE")] private AudioClip m_loosenClip = null;

        public RectTransform RectTransform { get; private set; } = null;
        public Animator Animator { get; private set; } = null;
        public AudioSource AudioSource { get; private set; } = null;

        private float m_delayTime = 0f;

        private void Awake()
        {
            RectTransform = GetComponent<RectTransform>();
            Animator = GetComponent<Animator>();
            AudioSource = GetComponent<AudioSource>();
        }

        private void Update()
        {
            if(GameManager.Instance.GameStatus == GameManager.EnumGameStatus.Play)
            {
                if(m_delayTime <= 0f)
                {
                    if (Input.GetKeyDown(KeyCode.A))
                    {
                        GameManager.Instance.ChangePlayerPosition(-1);
                        PlaySe(m_moveClip);
                    }
                    else if (Input.GetKeyDown(KeyCode.D))
                    {
                        GameManager.Instance.ChangePlayerPosition(1);
                        PlaySe(m_moveClip);
                    }
                    else if (Input.GetKeyDown(KeyCode.Space))
                    {
                        GameManager.Instance.LoosenSleeper();
                        Animator.SetTrigger(Animator.StringToHash("Loosen"));
                        PlaySe(m_loosenClip);
                        m_delayTime = LOOSEN_DELAY;
                    }
                }
                m_delayTime -= Time.deltaTime;
            }
        }

        private void PlaySe(AudioClip clip)
        {
            if (AudioSource.isPlaying)
            {
                AudioSource.Stop();
            }

            AudioSource.PlayOneShot(clip);
        }
    }
}
