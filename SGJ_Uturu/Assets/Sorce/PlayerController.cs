using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Uturu
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private AudioClip m_moveClip = null;
        [SerializeField] private AudioClip m_loosenClip = null;

        public RectTransform RectTransform { get; private set; } = null;
        public AudioSource AudioSource { get; private set; } = null;

        private void Awake()
        {
            RectTransform = GetComponent<RectTransform>();
            AudioSource = GetComponent<AudioSource>();
        }

        private void Update()
        {
            if(GameManager.Instance.GameStatus == GameManager.EnumGameStatus.Play)
            {
                if (Input.GetKeyDown(KeyCode.A))
                {
                    GameManager.Instance.ChangePlayerPosition(-1);
                    PlaySe(m_moveClip);
                }
                else if(Input.GetKeyDown(KeyCode.D))
                {
                    GameManager.Instance.ChangePlayerPosition(1);
                    PlaySe(m_moveClip);
                }
                else if(Input.GetKeyDown(KeyCode.Space))
                {
                    GameManager.Instance.LoosenSleeper();
                    PlaySe(m_loosenClip);
                }
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
