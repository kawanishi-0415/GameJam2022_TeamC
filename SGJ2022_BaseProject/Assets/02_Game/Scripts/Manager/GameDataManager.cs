using UnityEngine;
using System.Collections;

namespace SGJ
{
    public class GameDataManager : SingletonMonoBehaviour<GameDataManager>
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void InitializeBeforeSceneLoad()
        {
            var obj = new GameObject();
            obj.name = "GameDataManager";
            obj.AddComponent<GameDataManager>();
        }

        private GameData m_gameData = null;

        public GameData GameData { get => m_gameData; }

        private void Awake()
        {
            base.Awake();
            m_gameData = (GameData)Resources.Load("GameData");
            DontDestroyOnLoad(gameObject);
        }
    }
}

