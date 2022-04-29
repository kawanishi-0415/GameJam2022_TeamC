using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Uturu
{
    public class SleeperController : MonoBehaviour
    {
        [System.Serializable]
        public class SleeperStatusClass
        {
            public Sprite face = null;
            public float angle = 0f;
            public AudioClip audio = null;
            public int point = 0;
            public bool isGameOver = false;
        }

        public enum EnumMoveFoot
        {
            RightFoot,
            LeftFoot,

            Count,
        }
        private int m_moveFoot = 0;

        public const float MIN_CLEAR_VALUE = 0f;
        public const float GAMEOVER_VALUE = 180f;

        [SerializeField, Tooltip("顔イメージ")] private Image m_faceImage = null;
        [SerializeField, Tooltip("足オブジェクト")] private List<GameObject> m_footObjList = new List<GameObject>();

        [SerializeField, Tooltip("1秒に増える角度(基準値)")] private float m_baseAngle = 5.0f;
        [SerializeField, Tooltip("1秒に増える角度(誤差)")] private float m_diffAngle = 2.0f;

        [SerializeField, Tooltip("開始時の角度(最小)")] private float m_initValueMin = 0f;
        [SerializeField, Tooltip("開始時の角度(最大)")] private float m_initValueMax = 30f;

        private float m_angle = 0f;

        [SerializeField, Tooltip("各ステータス管理")] public List<SleeperStatusClass> m_statusList = new List<SleeperStatusClass>();

        private float m_value = 0f; // 足のつり具合

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

        public void Init()
        {
            m_value = Random.Range(m_initValueMin, m_initValueMax);
            SetFootValue();
            SetAngle();
            ChangeFoot();
        }

        private void SetAngle()
        {
            m_angle = Random.Range(m_baseAngle - m_diffAngle / 2, m_baseAngle + m_diffAngle / 2);
        }

        private void ChangeFoot()
        {
            m_moveFoot = Random.Range(0, (int)EnumMoveFoot.Count);
        }

        // Update is called once per frame
        private void Update()
        {
            if (GameManager.Instance.GameStatus == GameManager.EnumGameStatus.Play)
            {
                m_value += m_angle * Time.deltaTime;
                SetFootValue();

                // 顔変更
                SleeperStatusClass sleeperStatus = GetCurrentStatus();
                m_faceImage.sprite = sleeperStatus.face;
                if (sleeperStatus.isGameOver)
                {
                    AudioSource.PlayOneShot(sleeperStatus.audio);
                    GameManager.Instance.SetScreenMask(false);
                    GameManager.Instance.ChangeGameState(GameManager.EnumGameStatus.Result);
                }
            }
        }

        private void SetFootValue()
        {
            int mirror = m_moveFoot == (int)EnumMoveFoot.RightFoot ? -1 : 1;
            m_footObjList[m_moveFoot].transform.localEulerAngles = new Vector3(0f, 0f, m_value * mirror);
        }

        private SleeperStatusClass GetCurrentStatus()
        {
            SleeperStatusClass sleeperStatus = m_statusList[0];
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
            SleeperStatusClass sleeperStatus = GetCurrentStatus();
            int point = sleeperStatus.point;
            AudioSource.PlayOneShot(sleeperStatus.audio);

            Init();
            return point;
        }
    }
}
