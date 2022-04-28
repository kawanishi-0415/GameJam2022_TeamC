using UnityEngine;
using UnityEditor;

namespace SGJ
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "SceneData", menuName = "Create/SceneData")]
    public class SceneData : ScriptableObject
    {
        [Label("タイトルシーン"), SerializeField]
        private string m_titleScene = "";

        [Label("選択シーン"), SerializeField]
        private string m_selectScene = "";

        [Label("ゲームシーン"), SerializeField]
        private string m_mainScene = "";

        public string TitleScene { get => m_titleScene; }
        public string SelectScene { get => m_selectScene; }
        public string MainScene { get => m_mainScene; }
    }
}