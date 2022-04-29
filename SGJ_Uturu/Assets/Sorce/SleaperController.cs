using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Uturu
{
    public class SleaperController : MonoBehaviour
    {
        [System.Serializable]
        public class SleaperStatusClass
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
        [SerializeField, Tooltip("右足オブジェクト")] private GameObject m_rfootObj = null;
        [SerializeField, Tooltip("左足オブジェクト")] private GameObject m_lfootObj = null;

        [SerializeField, Tooltip("1秒に増える角度")] private float m_mag = 5.0f;

        [SerializeField, Tooltip("各ステータス管理")] public List<SleaperStatusClass> m_statusList = new List<SleaperStatusClass>();

        private float m_value = 0f; // 足のつり具合

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
            m_rfootObj.transform.localEulerAngles = new Vector3(0f, 0f, m_value * mirror);

            // 顔変更
            SleaperStatusClass sleaperStatus = m_statusList[0];
            for(int i = 0; i < m_statusList.Count; i++)
            {
                if(m_value < m_statusList[i].angle)
                {
                    break;
                }
                sleaperStatus = m_statusList[i];
            }
            m_faceImage.sprite = sleaperStatus.face;
        }

        public void ResetValue()
        {
            m_value = 0f;
        }
    }
}
