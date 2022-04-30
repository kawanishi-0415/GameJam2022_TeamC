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
            public bool isAngry = false;
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
        [SerializeField, Tooltip("怒った顔のスプライト")] private Sprite m_angryFace = null;
        [SerializeField, Tooltip("怒った顔の時間")] private float m_angryTime = 1.0f;

        /// <summary>怒っているか？</summary>
        private bool m_isAngry = false;

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
            SetFootAngle();
            SetInitAngle();
            ChangeMoveFoot();

            SleeperStatusData sleeperStatus = GetCurrentStatus();
            ChangeFace(sleeperStatus);

            for (int i = 0; i < m_statusList.Count; i++)
            {
                if(m_statusList[i].balloonImage != null)
                {
                    m_statusList[i].balloonImage.enabled = false;
                }
            }
        }

        /// <summary>
        /// 足の角度を初期化
        /// </summary>
        private void SetInitAngle()
        {
            m_angle = Random.Range(m_baseAngle - m_diffAngle / 2, m_baseAngle + m_diffAngle / 2);
        }

        /// <summary>
        /// 動く足を再設定
        /// </summary>
        private void ChangeMoveFoot()
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
                SetFootAngle();

                // 顔変更
                SleeperStatusData sleeperStatus = GetCurrentStatus();
                ChangeFace(sleeperStatus);
                if (sleeperStatus.isGameOver)
                {
                    if (m_prevStatus.balloonImage != null)
                    {
                        m_prevStatus.balloonImage.enabled = false;
                    }
                    sleeperStatus.balloonImage.enabled = true;
                    GameManager.Instance.AddScore(sleeperStatus.point);
                    GameManager.Instance.StopSleeperVoice();
                    AudioSource.PlayOneShot(sleeperStatus.audio);
                    GameManager.Instance.SetScreenMask(false);
                    GameManager.Instance.ChangeGameState(GameManager.EnumGameStatus.Result);
                }
                else if(m_prevStatus != sleeperStatus)
                {
                    if(m_coroutineChangeState == null && sleeperStatus.balloonImage != null)
                    {
                        m_coroutineChangeState = StartCoroutine(CoBalloonDisp(sleeperStatus));
                    }
                }
                m_prevStatus = sleeperStatus;
            }
        }

        /// <summary>
        /// 顔画像の変更
        /// </summary>
        /// <param name="sleeperStatus"></param>
        private void ChangeFace(SleeperStatusData sleeperStatus)
        {
            if (m_isAngry)
            {
                m_faceImage.sprite = m_angryFace;
            }
            else
            {
                m_faceImage.sprite = sleeperStatus.face;
            }
        }

        public void StopVoice()
        {
            if (AudioSource.isPlaying)
            {
                AudioSource.Stop();
            }
        }

        /// <summary>
        /// 吹き出しの表示処理
        /// </summary>
        /// <param name="sleeperStatus"></param>
        /// <returns></returns>
        private IEnumerator CoBalloonDisp(SleeperStatusData sleeperStatus)
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

        /// <summary>
        /// 足の表示角度を設定
        /// </summary>
        /// <returns></returns>
        private void SetFootAngle()
        {
            int mirror = m_moveFoot == (int)EnumMoveFoot.RightFoot ? -1 : 1;
            m_footObjList[m_moveFoot].transform.localEulerAngles = new Vector3(0f, 0f, m_value * mirror);
        }

        /// <summary>
        /// 現在の足の状態ステータスを取得
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// 足をもみほぐす
        /// </summary>
        /// <returns></returns>
        public int LoosenFoot()
        {
            SleeperStatusData sleeperStatus = GetCurrentStatus();
            int point = sleeperStatus.point + (int)Mathf.Ceil(m_value);
            AudioSource.PlayOneShot(sleeperStatus.audio);

            // 揉むのが早いと怒る
            if (sleeperStatus.isAngry)
            {
                if(m_coroutineAngry == null)
                {
                    m_coroutineAngry = StartCoroutine(CoAngry());
                }
            }

            Init();
            return point;
        }

        private Coroutine m_coroutineAngry = null;

        /// <summary>
        /// 怒る
        /// </summary>
        /// <returns></returns>
        private IEnumerator CoAngry()
        {
            m_isAngry = true;
            yield return new WaitForSeconds(m_angryTime);
            m_isAngry = false;

            m_coroutineAngry = null;
        }
    }
}
