using UnityEngine;
using System.Collections;

namespace SGJ
{
    public class GoalObj : SingletonMonoBehaviour<GoalObj>
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                GameManager.Instance.GameClear();
            }
        }
    }
}
