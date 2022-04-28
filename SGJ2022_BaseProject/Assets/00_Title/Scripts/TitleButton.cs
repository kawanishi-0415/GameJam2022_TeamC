using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SGJ
{
    public class TitleButton : MonoBehaviour
    {
        public void OnClick()
        {
            SceneLoadManager.Instance.Load(SceneType.Select);
        }
    }
}
