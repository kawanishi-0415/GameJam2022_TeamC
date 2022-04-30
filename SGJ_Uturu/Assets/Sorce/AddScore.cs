using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Uturu
{
    public class AddScore : MonoBehaviour
    {
        private Animator Animator = null;
        public RectTransform RectTransform { get; private set; } = null;

        [SerializeField, Tooltip("スコア表示テキスト")] private TextMeshProUGUI m_scoreText = null;

        private float m_upSpeed = 50f;
        private float m_vecter = 1f;

        private void Awake()
        {
            RectTransform = GetComponent<RectTransform>();
            Animator = GetComponent<Animator>();
        }

        public void SetText(int score)
        {
            if(score < 0)
            {
                m_vecter = -1f;
            }
            m_scoreText.text = score.ToString();
        }

        private void Start()
        {
            StartCoroutine(CoStart(Random.Range(1.0f, 1.5f)));
            Vector2 position = RectTransform.anchoredPosition;
            position.x += Random.Range(0f, 300f);
            position.y -= Random.Range(100f, 130f);
            RectTransform.anchoredPosition = position;
        }

        private void Update()
        {
            RectTransform.anchoredPosition += new Vector2(0f, m_upSpeed * m_vecter * Time.deltaTime);
        }

        private IEnumerator CoStart(float time)
        {
            yield return new WaitForSeconds(time);

            Destroy(gameObject);
        }
    }
}

