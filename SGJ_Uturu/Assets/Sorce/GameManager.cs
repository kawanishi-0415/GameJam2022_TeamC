using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Uturu
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager m_instance = null;
        public static GameManager Instance { get { return m_instance; } }

        /// <summary>
        /// ゲームの進行状況管理列挙型
        /// </summary>
        public enum EnumGameStatus
        {
            Init,
            Ready,
            Play,
            Result,

            Max,
        }
        public EnumGameStatus GameStatus { get; private set; } = EnumGameStatus.Init;

        /// <summary>寝ている人の数</summary>
        public const int SLEEPER_NUM = 5;
        /// <summary>1プレイの制限時間</summary>
        public const float STAGE_TIME = 30f;
        /// <summary>明るくなりだす時間</summary>
        public const float MORNING_TIME = 10f;

        [SerializeField, Tooltip("ゲームオブジェクト")] private Transform m_gameTransform = null;
        [SerializeField, Tooltip("寝てる人格納エリア")] private Transform m_sleeperAreaTransform = null;
        [SerializeField, Tooltip("UIエリア")] private Transform m_uiTransform = null;

        [SerializeField, Tooltip("スコア表示テキスト")] private TextMeshProUGUI m_scoreText = null;
        [SerializeField, Tooltip("時間表示テキスト")] private TextMeshProUGUI m_timeText = null;

        [SerializeField, Tooltip("窓用背景マスク")] private Image m_windowMaskImage = null;
        [SerializeField, Tooltip("画面用マスク")] private Image m_screenMaskImage = null;

        [SerializeField, Tooltip("プレイヤープレハブ")] private PlayerController m_playerPrefab = null;
        [SerializeField, Tooltip("寝てる人プレハブ")] private SleeperController m_sleeperPrefab = null;
        [SerializeField, Tooltip("レディWindowプレハブ")] private ReadyWindow m_readyWindowPrefab = null;
        [SerializeField, Tooltip("リザルトWindowプレハブ")] private ResultWindow m_resultWindowPrefab = null;

        [SerializeField, Tooltip("ゲーム中BGM")] private AudioClip m_gameClip = null;
        [SerializeField, Tooltip("リザルトBGM")] private AudioClip m_resultClip = null;

        /// <summary>プレイヤー</summary>
        private PlayerController m_player = null;
        /// <summary>寝ている人</summary>
        private List<SleeperController> m_sleeperList = new List<SleeperController>();

        /// <summary>制限時間</summary>
        public float m_time = STAGE_TIME;
        /// <summary>点数</summary>
        public int Score { get; set; } = 0;
        /// <summary>点数</summary>
        private int m_playerPosIndex = 0;

        /// <summary>レディ用コルーチン</summary>
        private Coroutine m_coroutineReady = null;
        /// <summary>リザルト用コルーチン</summary>
        private Coroutine m_coroutineResult = null;

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
            for (int i = 0; i < SLEEPER_NUM; i++)
            {
                SleeperController sleeper = Instantiate(m_sleeperPrefab, m_sleeperAreaTransform);
                m_sleeperList.Add(sleeper);
            }
            m_player = Instantiate(m_playerPrefab, m_gameTransform);

            SetWindowMaskImage();
            SetScreenMask(true);

            StartCoroutine(CoStart());
        }

        /// <summary>
        /// 整列後にプレイヤー位置を決める為、1F遅らせてます
        /// </summary>
        /// <returns></returns>
        private IEnumerator CoStart()
        {
            yield return null;

            SetPlayerPosition(0);
        }

        /// <summary>
        /// 窓の外の明るさ設定
        /// </summary>
        /// <returns></returns>
        private void SetWindowMaskImage()
        {
            Color color = m_windowMaskImage.color;
            color.a = m_time / MORNING_TIME;
            m_windowMaskImage.color = color;
        }

        /// <summary>
        /// 画面全体を暗くしている画像のOn/Off
        /// </summary>
        /// <returns></returns>
        public void SetScreenMask(bool flag)
        {
            m_screenMaskImage.enabled = flag;
        }

        /// <summary>
        /// BGM切り替え(ゲームプレイ中/リザルト)
        /// </summary>
        /// <returns></returns>
        private void ChangeBGM(AudioClip clip)
        {
            if (AudioSource.isPlaying)
            {
                AudioSource.Stop();
            }

            if(clip != null)
            {
                AudioSource.clip = clip;
                AudioSource.Play();
            }
        }

        /// <summary>
        /// BGM切り替え(ゲームプレイ中/リザルト)
        /// </summary>
        /// <returns></returns>
        private void SetPlayerPosition(int index)
        {
            Vector3 position = m_player.RectTransform.position;
            position.x = m_sleeperList[index].RectTransform.position.x;
            m_player.RectTransform.position = position;

            m_playerPosIndex = index;
        }

        /// <summary>
        /// プレイヤー位置の切り替え
        /// X座標は寝ている人の位置に合わせる
        /// </summary>
        /// <returns></returns>
        public void ChangePlayerPosition(int add)
        {
            int index = m_playerPosIndex + add;
            if (index >= m_sleeperList.Count) index = 0;
            else if (index < 0) index = m_sleeperList.Count - 1;

            SetPlayerPosition(index);
        }

        /// <summary>
        /// 寝ている人の足をほぐす
        /// </summary>
        /// <returns></returns>
        public void LoosenSleeper()
        {
            Score += m_sleeperList[m_playerPosIndex].LoosenFoot();
        }

        /// <summary>
        /// ゲーム内ステータスの切り替え
        /// </summary>
        /// <returns></returns>
        public void ChangeGameState(EnumGameStatus status)
        {
            GameStatus = status;
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
            m_scoreText.text = string.Format("{0:0}", Score);
        }

        /// <summary>
        /// 初期化処理
        /// </summary>
        /// <returns></returns>
        private void StatusInit()
        {
            m_time = STAGE_TIME;
            Score = 0;

            SetWindowMaskImage();
            SetScreenMask(true);
            SetPlayerPosition(0);

            for(int i = 0; i < m_sleeperList.Count; i++)
            {
                m_sleeperList[i].Init();
            }

            ChangeBGM(null);

            GameStatus = EnumGameStatus.Ready;
        }

        /// <summary>
        /// ゲーム開始処理
        /// </summary>
        /// <returns></returns>
        private void StatusReady()
        {
            if (m_coroutineReady == null)
            {
                m_coroutineReady = StartCoroutine(CoReady());
            }
        }
        private IEnumerator CoReady()
        {
            ReadyWindow readyWindow = Instantiate(m_readyWindowPrefab, m_uiTransform);
            yield return null;

            while (!readyWindow.IsEndAnimation())
            {
                yield return null;
            }
            yield return new WaitForSeconds(0.5f);

            while (!Input.GetKeyDown(KeyCode.Space))
            {
                yield return null;
            }

            Destroy(readyWindow.gameObject);

            GameStatus = EnumGameStatus.Play;
            AudioSource.Play();
            ChangeBGM(m_gameClip);

            m_coroutineReady = null;
        }

        /// <summary>
        /// ゲーム処理
        /// </summary>
        /// <returns></returns>
        private void StatusPlay()
        {
            m_time -= Time.deltaTime;

            Mathf.Max(m_time, 0f);
            if (m_time <= 0f)
            {
                GameStatus = EnumGameStatus.Result;
            }
        }

        /// <summary>
        /// リザルト処理
        /// </summary>
        /// <returns></returns>
        private void StatusResult()
        {
            if(m_coroutineResult == null)
            {
                m_coroutineResult = StartCoroutine(CoResult());
            }
        }

        /// <summary>
        /// リザルトのコルーチン
        /// </summary>
        /// <returns></returns>
        private IEnumerator CoResult()
        {
            ChangeBGM(m_resultClip);
            ResultWindow resultWindow = Instantiate(m_resultWindowPrefab, m_uiTransform);
            yield return null;

            while (!resultWindow.IsEndAnimation())
            {
                yield return null;
            }
            yield return new WaitForSeconds(0.5f);

            while (!Input.GetKeyDown(KeyCode.Space))
            {
                yield return null;
            }

            Destroy(resultWindow.gameObject);

            //GameStatus = EnumGameStatus.Init;
            SceneManager.LoadScene("Title");
            m_coroutineResult = null;
        }
    }
}
