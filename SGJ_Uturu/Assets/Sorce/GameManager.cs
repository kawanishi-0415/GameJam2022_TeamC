using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Uturu
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager m_instance = null;
        public static GameManager Instance { get { return m_instance; } }

        public enum EnumGameStatus
        {
            Init,
            Ready,
            Play,
            Result,

            Max,
        }
        public EnumGameStatus GameStatus { get; private set; } = EnumGameStatus.Init;

        public const int SLEEPER_NUM = 5;
        public const float STAGE_TIME = 30f;
        public const float MORNING_TIME = 10f;

        [SerializeField, Tooltip("ゲームオブジェクト")] private Transform m_gameTransform = null;
        [SerializeField, Tooltip("寝てる人格納エリア")] private Transform m_sleeperAreaTransform = null;

        [SerializeField, Tooltip("スコア表示テキスト")] private TextMeshProUGUI m_scoreText = null;
        [SerializeField, Tooltip("時間表示テキスト")] private TextMeshProUGUI m_timeText = null;

        [SerializeField, Tooltip("窓用背景マスク")] private Image m_windowMaskImage = null;
        [SerializeField, Tooltip("画面用マスク")] private Image m_screenMaskImage = null;

        [SerializeField, Tooltip("プレイヤープレハブ")] private PlayerController m_playerPrefab = null;
        [SerializeField, Tooltip("寝てる人プレハブ")] private SleeperController m_sleeperPrefab = null;

        private PlayerController m_player = null;
        private List<SleeperController> m_sleeperList = new List<SleeperController>();

        public float m_time = STAGE_TIME;
        public int m_point = 0;
        private int m_playerPositionIndex = 0;

        public AudioSource AudioSource { get; private set; } = null;

        private void Awake()
        {
            if(m_instance == null)
            {
                m_instance = this;
            }
            AudioSource = GetComponent<AudioSource>();
        }

        private void Start()
        {
            StartCoroutine(CoStart());
        }
        private IEnumerator CoStart()
        {
            for (int i = 0; i < SLEEPER_NUM; i++)
            {
                SleeperController sleeper = Instantiate(m_sleeperPrefab, m_sleeperAreaTransform);
                m_sleeperList.Add(sleeper);
            }
            m_player = Instantiate(m_playerPrefab, m_gameTransform);
            yield return null;

            SetWindowMaskImage();

            SetPlayerPosition(0);
        }

        private void SetWindowMaskImage()
        {
            Color color = m_windowMaskImage.color;
            color.a = m_time / MORNING_TIME;
            m_windowMaskImage.color = color;
        }

        private void SetPlayerPosition(int index)
        {
            Vector3 position = m_player.RectTransform.position;
            position.x = m_sleeperList[index].RectTransform.position.x;
            m_player.RectTransform.position = position;

            m_playerPositionIndex = index;
        }

        public void ChangePlayerPosition(int add)
        {
            int index = m_playerPositionIndex + add;
            if (index >= m_sleeperList.Count) index = 0;
            else if (index < 0) index = m_sleeperList.Count - 1;

            SetPlayerPosition(index);
        }

        public void LoosenSleeper()
        {
            m_point += m_sleeperList[m_playerPositionIndex].LoosenFoot();
        }

        private void Update()
        {
            switch (GameStatus)
            {
                case EnumGameStatus.Init:
                    StatusInit();
                    break;
                case EnumGameStatus.Ready:
                    StatusReady();
                    break;
                case EnumGameStatus.Play:
                    StatusPlay();
                    break;
                case EnumGameStatus.Result:
                    StatusResult();
                    break;
            }
            SetWindowMaskImage();

            m_timeText.text = string.Format("{0:00}", Mathf.Ceil(m_time));
            m_scoreText.text = string.Format("{0:0}", m_point);
        }

        private void StatusInit()
        {
            GameStatus = EnumGameStatus.Ready;
        }

        private void StatusReady()
        {
            GameStatus = EnumGameStatus.Play;
            AudioSource.Play();
        }

        private void StatusPlay()
        {
            m_time -= Time.deltaTime;

            Mathf.Max(m_time, 0f);
            if (m_time <= 0f)
            {
                GameStatus = EnumGameStatus.Result;
            }
        }

        private void StatusResult()
        {

        }
    }
}
