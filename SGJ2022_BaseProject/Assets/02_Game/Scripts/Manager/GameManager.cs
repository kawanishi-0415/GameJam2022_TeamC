using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SGJ
{
    public class GameManager : SingletonMonoBehaviour<GameManager>
    {
        private bool m_isStart = false;
        private bool m_isGameClear = false;
        private bool m_isGameOver = false;

        /// <summary>
        /// �Q�[���v���C���t���O
        /// </summary>
        public bool IsPlay
        {
            get
            {
                // �X�^�[�g���Ă��āA�N���A��Q�[���I�[�o�[����Ȃ��Ȃ�true
                if (m_isStart && m_isGameClear == false && m_isGameOver == false)
                {
                    return true;
                }
                return false;
            }
        }

        public bool IsGameClear { get => m_isGameClear; }
        public bool IsGameOver { get => m_isGameOver; }

        public void GameStart()
        {
            if (m_isStart)
                return;
            GameDebug.Log("�Q�[���X�^�[�g");
            if(GoalObj.Instance == null)
            {
                GameDebug.LogError("�S�[��������܂���");
            }
            m_isStart = true;
        }

        public void GameClear()
        {
            if (m_isGameClear || m_isGameOver)
                return;
            GameDebug.Log("�Q�[���N���A");
            m_isGameClear = true;
            ClearStaging.Instance.SetActive(true);
        }

        public void GameOver()
        {
            if (m_isGameClear || m_isGameOver)
                return;
            GameDebug.Log("�Q�[���I�[�o�[");
            m_isGameOver = true;
        }

    }
}

