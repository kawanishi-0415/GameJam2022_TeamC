using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    public void Update()
    {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene("Game");
            }
#if UNITY_STANDALONE
            if (Input.GetKey(KeyCode.Escape))
                UnityEngine.Application.Quit();
#endif
    }
}
