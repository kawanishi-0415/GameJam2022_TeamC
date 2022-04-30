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

        /// <summary>���E�̑��Ǘ��񋓌^</summary>
        public enum EnumMoveFoot
        {
            RightFoot,
            LeftFoot,

            Count,
        }
        /// <summary>�ړ����Ă��鑫</summary>
        private int m_moveFoot = 0;

        [SerializeField, Tooltip("��C���[�W")] private Image m_faceImage = null;
        [SerializeField, Tooltip("���I�u�W�F�N�g")] private List<GameObject> m_footObjList = new List<GameObject>();

        [SerializeField, Tooltip("1�b�ɑ�����p�x(��l)")] private float m_baseAngle = 5.0f;
        [SerializeField, Tooltip("1�b�ɑ�����p�x(�덷)")] private float m_diffAngle = 2.0f;

        [SerializeField, Tooltip("�J�n���̊p�x(�ŏ�)")] private float m_initValueMin = 0f;
        [SerializeField, Tooltip("�J�n���̊p�x(�ő�)")] private float m_initValueMax = 30f;

        /// <summary>���݂̑��̊p�x</summary>
        private float m_angle = 0f;

        [SerializeField, Tooltip("�e�X�e�[�^�X�Ǘ�")] public List<SleeperStatusData> m_statusList = new List<SleeperStatusData>();
        [SerializeField, Tooltip("�{������̃X�v���C�g")] private Sprite m_angryFace = null;
        [SerializeField, Tooltip("�{������̎���")] private float m_angryTime = 1.0f;

        /// <summary>�{���Ă��邩�H</summary>
        private bool m_isAngry = false;

        /// <summary>���̂�</summary>
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
        /// ���̏�ԏ���������
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
        /// ���̊p�x��������
        /// </summary>
        private void SetInitAngle()
        {
            m_angle = Random.Range(m_baseAngle - m_diffAngle / 2, m_baseAngle + m_diffAngle / 2);
        }

        /// <summary>
        /// ���������Đݒ�
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

                // ��ύX
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
        /// ��摜�̕ύX
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
        /// �����o���̕\������
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
        /// ���̕\���p�x��ݒ�
        /// </summary>
        /// <returns></returns>
        private void SetFootAngle()
        {
            int mirror = m_moveFoot == (int)EnumMoveFoot.RightFoot ? -1 : 1;
            m_footObjList[m_moveFoot].transform.localEulerAngles = new Vector3(0f, 0f, m_value * mirror);
        }

        /// <summary>
        /// ���݂̑��̏�ԃX�e�[�^�X���擾
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
        /// �������݂ق���
        /// </summary>
        /// <returns></returns>
        public int LoosenFoot()
        {
            SleeperStatusData sleeperStatus = GetCurrentStatus();
            int point = sleeperStatus.point + (int)Mathf.Ceil(m_value);
            AudioSource.PlayOneShot(sleeperStatus.audio);

            // ���ނ̂������Ɠ{��
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
        /// �{��
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
