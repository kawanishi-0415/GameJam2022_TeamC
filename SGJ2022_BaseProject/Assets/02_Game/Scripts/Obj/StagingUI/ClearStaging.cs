using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SGJ
{
    public class ClearStaging : SingletonMonoBehaviour<ClearStaging>
    {
        [SerializeField]
        private CanvasGroup m_canvasGroup = null;

        private bool m_isTap = false;

        public void SetActive(bool active)
        {
            if (active)
            {
                m_canvasGroup.alpha = 1f;
                m_canvasGroup.blocksRaycasts = true;
            }
            else
            {
                m_canvasGroup.alpha = 0f;
                m_canvasGroup.blocksRaycasts = false;
            }
        }

        public void OnClick()
        {
            if (m_isTap)
                return;
            SceneLoadManager.Instance.Load(SceneType.Title);
            m_isTap = true;
        }
    }
}
