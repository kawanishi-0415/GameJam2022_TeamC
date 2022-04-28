using UnityEngine;
using UnityEditor;

namespace SGJ
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "GameData", menuName = "Create/GameData")]
    public class GameData : ScriptableObject
    {
        [Label("ステージナンバー"), SerializeField]
        private int m_stageNum = 0;

        [Label("最大ステージ数"), SerializeField]
        private int m_maxStage = 1;

        public int StageNum { get => m_stageNum; set => m_stageNum = value; }
        public int MaxStage { get => m_maxStage; }
    }
}