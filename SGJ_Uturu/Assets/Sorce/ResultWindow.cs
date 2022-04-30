using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Uturu
{
    public class ResultWindow : MonoBehaviour
    {
        /// <summary>
        /// リザルト時の表示条件管理データ
        /// </summary>
        [System.Serializable]
        public class ResultData
        {
            public Sprite sprite;
            public int score;
        }

        public Animator Animator { get; private set; } = null;

        [SerializeField, Tooltip("リザルトデータリスト")] private List<ResultData> m_resultDataList = new List<ResultData>();
        [SerializeField, Tooltip("リザルト画像")] private Image m_image = null;
        [SerializeField, Tooltip("スコアテキスト")] private TextMeshProUGUI m_scoreText = null;

        private void Awake()
        {
            Animator = GetComponent<Animator>();
        }

        private void Start()
        {
            ResultData resultData = m_resultDataList[0];
            for(int i = 0; i < m_resultDataList.Count; i++)
            {
                resultData = m_resultDataList[i];
                if(GameManager.Instance.Score < resultData.score)
                {
                    break;
                }
            }
            m_image.sprite = resultData.sprite;
            m_scoreText.text = GameManager.Instance.Score.ToString();
        }

        /// <summary>
        /// アニメーションが終わったかのチェック
        /// </summary>
        /// <returns></returns>
        public bool IsEndAnimation()
        {
            return Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f;
        }
    }
}
