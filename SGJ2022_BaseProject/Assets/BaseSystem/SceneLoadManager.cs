using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace SGJ
{
    public enum SceneType
    {
        Title,
        Select,
        Main,
    }
    public class SceneLoadManager : SingletonMonoBehaviour<SceneLoadManager>
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void InitializeBeforeSceneLoad()
        {
            var obj = new GameObject();
            obj.name = "SceneLoadManager";
            obj.AddComponent<SceneLoadManager>();
        }

        private SceneData m_sceneData = null;

        private void Awake()
        {
            base.Awake();
            m_sceneData = (SceneData)Resources.Load("SceneData");
            DontDestroyOnLoad(gameObject);
        }

        public void Load(SceneType type)
        {
            switch (type)
            {
                case SceneType.Title:
                    SceneManager.LoadScene(m_sceneData.TitleScene);
                    break;
                case SceneType.Select:
                    SceneManager.LoadScene(m_sceneData.SelectScene);
                    break;
                case SceneType.Main:
                    SceneManager.LoadScene(m_sceneData.MainScene);
                    break;
            }
        }
    }
}
