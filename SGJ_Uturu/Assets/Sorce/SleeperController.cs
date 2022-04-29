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

        [SerializeField, Tooltip("1秒に増える角度")] private float m_mag = 5.0f;

        [SerializeField, Tooltip("各ステータス管理")] public List<SleeperStatusClass> m_statusList = new List<SleeperStatusClass>();

        private float m_value = 0f; // 足のつり具合

        public RectTransform RectTransform { get; private set; } = null;
        private void Awake()
        {
            RectTransform = GetComponent<RectTransform>();
        }

        // Start is called before the first frame update
        void Start()
        {
            m_moveFoot = Random.Range(0, (int)EnumMoveFoot.Count);
        }

        // Update is called once per frame
        void Update()
        {
            int mirror = m_moveFoot == (int)EnumMoveFoot.RightFoot ? -1 : 1;

            m_value += m_mag * Time.deltaTime;
            m_footObjList[m_moveFoot].transform.localEulerAngles = new Vector3(0f, 0f, m_value * mirror);

            // 顔変更
            SleeperStatusClass sleeperStatus = m_statusList[0];
            for(int i = 0; i < m_statusList.Count; i++)
            {
                if(m_value < m_statusList[i].angle)
                {
                    break;
                }
                sleeperStatus = m_statusList[i];
            }
            m_faceImage.sprite = sleeperStatus.face;
        }

        public void ResetValue()
        {
            m_value = 0f;
        }
    }
}
