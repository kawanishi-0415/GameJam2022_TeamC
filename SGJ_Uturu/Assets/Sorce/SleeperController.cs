using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Uturu
{
    public class SleeperController : MonoBehaviour
    {
        [System.Serializable]
        public class SleeperStatusData
        {
            public Sprite face = null;
            public float angle = 0f;
            public AudioClip audio = null;
            public int point = 0;
            public bool isGameOver = false;
            public Image balloonImage = null;
            public float balloonTime = 0f;
        }

        /// <summary>左右の足管理列挙型</summary>
        public enum EnumMoveFoot
        {
            RightFoot,
            LeftFoot,

            Count,
        }
        /// <summary>移動している足</summary>
        private int m_moveFoot = 0;

        [SerializeField, Tooltip("顔イメージ")] private Image m_faceImage = null;
        [SerializeField, Tooltip("足オブジェクト")] private List<GameObject> m_footObjList = new List<GameObject>();

        [SerializeField, Tooltip("1秒に増える角度(基準値)")] private float m_baseAngle = 5.0f;
        [SerializeField, Tooltip("1秒に増える角度(誤差)")] private float m_diffAngle = 2.0f;

        [SerializeField, Tooltip("開始時の角度(最小)")] private float m_initValueMin = 0f;
        [SerializeField, Tooltip("開始時の角度(最大)")] private float m_initValueMax = 30f;

        /// <summary>現在の足の角度</summary>
        private float m_angle = 0f;

        [SerializeField, Tooltip("各ステータス管理")] public List<SleeperStatusData> m_statusList = new List<SleeperStatusData>();

        /// <summary>足のつり具合</summary>
        private float m_value = 0f;

        public RectTransform RectTransform { get; private set; } = null;
        public AudioSource AudioSource { get; private set; } = null;

        private void Awake()
        {
            RectTransform = GetComponent<RectTransform>();
            AudioSource = GetComponent<AudioSource>();
        }

        // Start is called before the first frame update
        private void Start()
        {
            Init();
        }

        /// <summary>
        /// 足の状態初期化処理
        /// </summary>
        public void Init()
        {
            m_value = Random.Range(m_initValueMin, m_initValueMax);
            SetFootValue();
            SetAngle();
            ChangeFoot();

            SleeperStatusData sleeperStatus = GetCurrentStatus();
            m_faceImage.sprite = sleeperStatus.face;

            for (int i = 0; i < m_statusList.Count; i++)
            {
                if(m_statusList[i].balloonImage != null)
                {
                    m_statusList[i].balloonImage.enabled = false;
                }
            }
        }

        private void SetAngle()
        {
            m_angle = Random.Range(m_baseAngle - m_diffAngle / 2, m_baseAngle + m_diffAngle / 2);
        }

        private void ChangeFoot()
        {
            m_moveFoot = Random.Range(0, (int)EnumMoveFoot.Count);
        }

        private SleeperStatusData m_prevStatus = null;
        private Coroutine m_coroutineChangeState = null;
        private void Update()
        {
            if (GameManager.Instance.GameStatus == GameManager.EnumGameStatus.Play)
            {
                m_value += m_angle * Time.deltaTime;
                SetFootValue();

                // 顔変更
                SleeperStatusData sleeperStatus = GetCurrentStatus();
                m_faceImage.sprite = sleeperStatus.face;
                if (sleeperStatus.isGameOver)
                {
                    if (m_prevStatus.balloonImage != null)
                    {
                        m_prevStatus.balloonImage.enabled = false;
                    }
                    sleeperStatus.balloonImage.enabled = true;
                    AudioSource.PlayOneShot(sleeperStatus.audio);
                    GameManager.Instance.SetScreenMask(false);
                    GameManager.Instance.ChangeGameState(GameManager.EnumGameStatus.Result);
                }
                else if(m_prevStatus != sleeperStatus)
                {
                    if(m_coroutineChangeState == null && sleeperStatus.balloonImage != null)
                    {
                        m_coroutineChangeState = StartCoroutine(CoChangeState(sleeperStatus));
                    }
                }
                m_prevStatus = sleeperStatus;
            }
        }
        private IEnumerator CoChangeState(SleeperStatusData sleeperStatus)
        {
            if(m_prevStatus.balloonImage != null)
            {
                m_prevStatus.balloonImage.enabled = false;
            }

            sleeperStatus.balloonImage.enabled = true;
            if(sleeperStatus.balloonTime > 0f)
            {
                yield return new WaitForSeconds(sleeperStatus.balloonTime);
                sleeperStatus.balloonImage.enabled = false;
            }
            else
            {
                yield return null;
            }

            m_coroutineChangeState = null;
        }

        private void SetFootValue()
        {
            int mirror = m_moveFoot == (int)EnumMoveFoot.RightFoot ? -1 : 1;
            m_footObjList[m_moveFoot].transform.localEulerAngles = new Vector3(0f, 0f, m_value * mirror);
        }

        private SleeperStatusData GetCurrentStatus()
        {
            SleeperStatusData sleeperStatus = m_statusList[0];
            for (int i = 0; i < m_statusList.Count; i++)
            {
                sleeperStatus = m_statusList[i];
                if (m_value < m_statusList[i].angle)
                {
                    break;
                }
            }
            return sleeperStatus;
        }

        public int LoosenFoot()
        {
            SleeperStatusData sleeperStatus = GetCurrentStatus();
            int point = sleeperStatus.point + (int)Mathf.Ceil(m_value);
            AudioSource.PlayOneShot(sleeperStatus.audio);

            Init();
            return point;
        }
    }
}
