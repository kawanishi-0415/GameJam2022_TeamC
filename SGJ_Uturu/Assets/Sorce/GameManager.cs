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
        /// �Q�[���̐i�s�󋵊Ǘ��񋓌^
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

        /// <summary>�Q�Ă���l�̐�</summary>
        public const int SLEEPER_NUM = 5;
        /// <summary>1�v���C�̐�������</summary>
        public const float STAGE_TIME = 30f;
        /// <summary>���邭�Ȃ肾������</summary>
        public const float MORNING_TIME = 10f;

        [SerializeField, Tooltip("�Q�[���I�u�W�F�N�g")] private Transform m_gameTransform = null;
        [SerializeField, Tooltip("�Q�Ă�l�i�[�G���A")] private Transform m_sleeperAreaTransform = null;
        [SerializeField, Tooltip("UI�G���A")] private Transform m_uiTransform = null;

        [SerializeField, Tooltip("�X�R�A�\���e�L�X�g")] private TextMeshProUGUI m_scoreText = null;
        [SerializeField, Tooltip("���ԕ\���e�L�X�g")] private TextMeshProUGUI m_timeText = null;

        [SerializeField, Tooltip("���p�w�i�}�X�N")] private Image m_windowMaskImage = null;
        [SerializeField, Tooltip("��ʗp�}�X�N")] private Image m_screenMaskImage = null;

        [SerializeField, Tooltip("�v���C���[�v���n�u")] private PlayerController m_playerPrefab = null;
        [SerializeField, Tooltip("�Q�Ă�l�v���n�u")] private SleeperController m_sleeperPrefab = null;
        [SerializeField, Tooltip("���f�BWindow�v���n�u")] private ReadyWindow m_readyWindowPrefab = null;
        [SerializeField, Tooltip("���U���gWindow�v���n�u")] private ResultWindow m_resultWindowPrefab = null;

        [SerializeField, Tooltip("�Q�[����BGM")] private AudioClip m_gameClip = null;
        [SerializeField, Tooltip("���U���gBGM")] private AudioClip m_resultClip = null;

        /// <summary>�v���C���[</summary>
        private PlayerController m_player = null;
        /// <summary>�Q�Ă���l</summary>
        private List<SleeperController> m_sleeperList = new List<SleeperController>();

        /// <summary>��������</summary>
        public float m_time = STAGE_TIME;
        /// <summary>�_��</summary>
        public int Score { get; set; } = 0;
        /// <summary>�_��</summary>
        private int m_playerPosIndex = 0;

        /// <summary>���f�B�p�R���[�`��</summary>
        private Coroutine m_coroutineReady = null;
        /// <summary>���U���g�p�R���[�`��</summary>
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
        /// �����Ƀv���C���[�ʒu�����߂�ׁA1F�x�点�Ă܂�
        /// </summary>
        /// <returns></returns>
        private IEnumerator CoStart()
        {
            yield return null;

            SetPlayerPosition(0);
        }

        /// <summary>
        /// ���̊O�̖��邳�ݒ�
        /// </summary>
        /// <returns></returns>
        private void SetWindowMaskImage()
        {
            Color color = m_windowMaskImage.color;
            color.a = m_time / MORNING_TIME;
            m_windowMaskImage.color = color;
        }

        /// <summary>
        /// ��ʑS�̂��Â����Ă���摜��On/Off
        /// </summary>
        /// <returns></returns>
        public void SetScreenMask(bool flag)
        {
            m_screenMaskImage.enabled = flag;
        }

        /// <summary>
        /// BGM�؂�ւ�(�Q�[���v���C��/���U���g)
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
        /// BGM�؂�ւ�(�Q�[���v���C��/���U���g)
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
        /// �v���C���[�ʒu�̐؂�ւ�
        /// X���W�͐Q�Ă���l�̈ʒu�ɍ��킹��
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
        /// �Q�Ă���l�̑����ق���
        /// </summary>
        /// <returns></returns>
        public void LoosenSleeper()
        {
            Score += m_sleeperList[m_playerPosIndex].LoosenFoot();
        }

        /// <summary>
        /// �Q�[�����X�e�[�^�X�̐؂�ւ�
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
        /// ����������
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
        /// �Q�[���J�n����
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
        /// �Q�[������
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
        /// ���U���g����
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
        /// ���U���g�̃R���[�`��
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
