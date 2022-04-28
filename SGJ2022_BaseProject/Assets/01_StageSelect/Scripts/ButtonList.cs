using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SGJ
{
    public class ButtonList : MonoBehaviour
    {
        [Label("ボタンプレハブ"), SerializeField]
        private GameObject m_prefab = null;

        private void Start()
        {
            var max = GameDataManager.Instance.GameData.MaxStage;
            for (int i = 0; i < max; ++i)
            {
                var obj = Instantiate(m_prefab, transform);
                obj.GetComponent<StageSelectButton>().Set(i);
            }
        }
    }
}
