using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace SGJ
{
    public class StartCountDown : MonoBehaviour
    {
        [Label("カウント"), SerializeField]
        private int m_startCount = 3;

        [Label("開始テキスト"), SerializeField]
        private string m_endText = "START!";

        [Label("開始テキスト表示で開始"), SerializeField]
        private bool m_isZeroStart = true;

        [Label("テキストオブジェクト"), SerializeField]
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
        /// カウントダウンアニメーション
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
                        // スタートの文字が消えてからスタート
                        if (m_isZeroStart == false)
                        {
                            GameStart();
                        }
                        m_textMesh.transform.localScale = Vector3.zero;
                    }
                    else if(count == 0)
                    {
                        // スタートの文字が出たらスタート
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
