using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace SGJ
{
    public class StartCountDown : MonoBehaviour
    {
        [Label("�J�E���g"), SerializeField]
        private int m_startCount = 3;

        [Label("�J�n�e�L�X�g"), SerializeField]
        private string m_endText = "START!";

        [Label("�J�n�e�L�X�g�\���ŊJ�n"), SerializeField]
        private bool m_isZeroStart = true;

        [Label("�e�L�X�g�I�u�W�F�N�g"), SerializeField]
        private TextMeshProUGUI m_textMesh = null;

        private bool m_isPlay = false;

        private void Start()
        {
            Play();
        }

        public void GameStart()
        {
            GameManager.Instance.GameStart();
        }

        private void Play()
        {
            StartCoroutine(Co_CountDown());
        }

        /// <summary>
        /// �J�E���g�_�E���A�j���[�V����
        /// </summary>
        /// <returns></returns>
        IEnumerator Co_CountDown()
        {
            int count = m_startCount;
            float time = 0f;

            m_textMesh.text = count.ToString();
            while (count >= 0)
            {
                m_textMesh.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, time / 0.5f);
                time += Time.deltaTime;
                yield return new WaitForEndOfFrame();
                if (time > 1f)
                {
                    --count;
                    if(count == -1)
                    {
                        // �X�^�[�g�̕����������Ă���X�^�[�g
                        if (m_isZeroStart == false)
                        {
                            GameStart();
                        }
                        m_textMesh.transform.localScale = Vector3.zero;
                    }
                    else if(count == 0)
                    {
                        // �X�^�[�g�̕������o����X�^�[�g
                        if (m_isZeroStart)
                        {
                            GameStart();
                        }
                        m_textMesh.text = m_endText;
                    }
                    else
                    {
                        m_textMesh.text = count.ToString();
                    }
                    time = 0f;
                }
            }
        }
    }
}
