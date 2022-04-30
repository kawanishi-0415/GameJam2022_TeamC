using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    public void LoadingNewScene()
        {
        if (Input.GetKeyDown(KeyCode.Space))
        { SceneManager.LoadScene("Game"); }
    }
}
