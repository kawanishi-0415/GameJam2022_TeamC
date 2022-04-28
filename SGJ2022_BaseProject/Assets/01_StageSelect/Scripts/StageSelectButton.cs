using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace SGJ
{
    public class StageSelectButton : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI m_text = null;

        private int m_num = 0;

        public void Set(int num)
        {
            m_text.text = "Stage " + (num + 1).ToString();
            m_num = num;
        }

        public void OnClick()
        {
            GameDataManager.Instance.GameData.StageNum = m_num;
            SceneLoadManager.Instance.Load(SceneType.Main);
        }
    }
}
