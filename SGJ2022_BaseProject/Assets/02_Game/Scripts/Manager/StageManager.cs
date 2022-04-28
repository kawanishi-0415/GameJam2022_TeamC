using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace SGJ
{
    public class StageManager : MonoBehaviour
    {
        [Label("ステージリスト"), SerializeField]
        private string[] m_list;

        private void Awake()
        {
            // 複数シーンが読み込まれている場合は読み込みません
            if (SceneManager.sceneCount > 1)
                return;

            var data = GameDataManager.Instance.GameData;
            if (data.MaxStage > m_list.Length)
            {
                GameDebug.LogWarning("登録ステージが不足しています");
            }
            else
            {
                GameDebug.LogWarning("登録ステージが多すぎます");
            }
            SceneManager.LoadScene(m_list[data.StageNum % m_list.Length], LoadSceneMode.Additive);
        }
    }
}
