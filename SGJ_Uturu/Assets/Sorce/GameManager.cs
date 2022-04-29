using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Uturu
{
    public class GameManager : MonoBehaviour
    {
        public enum EnumGameStatus
        {
            Init,
            Ready,
            Play,
            Result,

            Max,
        }
        private EnumGameStatus m_gameStatus = EnumGameStatus.Init;

        public const int SLEEPER_NUM = 5;
        public const float STAGE_TIME = 30f;

        [SerializeField, Tooltip("ゲームオブジェクト")] Transform m_gameTransform = null;
        [SerializeField, Tooltip("寝てる人格納エリア")] Transform m_sleeperAreaTransform = null;

        [SerializeField, Tooltip("スコア表示テキスト")] TextMeshProUGUI m_scoreText = null;
        [SerializeField, Tooltip("時間表示テキスト")] TextMeshProUGUI m_timeText = null;

        [SerializeField, Tooltip("プレイヤープレハブ")] PlayerController m_playerPrefab = null;
        [SerializeField, Tooltip("寝てる人プレハブ")] SleeperController m_sleeperPrefab = null;

        private PlayerController m_player = null;
        private List<SleeperController> m_sleeperList = new List<SleeperController>();

        public float m_time = STAGE_TIME;

        private void Awake()
        {
            
        }

        private void Start()
        {
            for(int i = 0; i < SLEEPER_NUM; i++)
            {
                SleeperController sleeper = Instantiate(m_sleeperPrefab, m_sleeperAreaTransform);
                m_sleeperList.Add(sleeper);
            }
            m_player = Instantiate(m_playerPrefab, m_gameTransform);
        }

        private void Update()
        {
            switch (m_gameStatus)
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

            m_timeText.text = string.Format("{0:00}", Mathf.Ceil(m_time));
            m_scoreText.text = "0";
        }

        private void StatusInit()
        {
            m_gameStatus = EnumGameStatus.Ready;
        }

        private void StatusReady()
        {
            m_gameStatus = EnumGameStatus.Play;
        }

        private void StatusPlay()
        {
            m_time -= Time.deltaTime;

            Mathf.Max(m_time, 0f);
            if (m_time <= 0f)
            {
                m_gameStatus = EnumGameStatus.Result;
            }
        }

        private void StatusResult()
        {

        }
    }
}
