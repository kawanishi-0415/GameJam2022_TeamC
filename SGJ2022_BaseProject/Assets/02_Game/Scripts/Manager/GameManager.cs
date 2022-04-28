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
        /// ゲームプレイ中フラグ
        /// </summary>
        public bool IsPlay
        {
            get
            {
                // スタートしていて、クリアやゲームオーバーじゃないならtrue
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
            GameDebug.Log("ゲームスタート");
            if(GoalObj.Instance == null)
            {
                GameDebug.LogError("ゴールがありません");
            }
            m_isStart = true;
        }

        public void GameClear()
        {
            if (m_isGameClear || m_isGameOver)
                return;
            GameDebug.Log("ゲームクリア");
            m_isGameClear = true;
            ClearStaging.Instance.SetActive(true);
        }

        public void GameOver()
        {
            if (m_isGameClear || m_isGameOver)
                return;
            GameDebug.Log("ゲームオーバー");
            m_isGameOver = true;
        }

    }
}

