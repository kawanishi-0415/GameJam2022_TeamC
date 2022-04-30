using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleaperController : MonoBehaviour
{
    [SerializeField] GameObject m_rfootObj = null;
    [SerializeField] float m_mag = 2f;

    public const float MIN_CLEAR_VALUE = 0f;
    public const float GAMEOVER_VALUE = 180f;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    private float m_time = 0f;
    // Update is called once per frame
    void Update()
    {
        m_time += Time.deltaTime * m_mag;
        m_rfootObj.transform.localEulerAngles = new Vector3(0f, 0f, -m_time);
        if (m_time > GAMEOVER_VALUE)
        {
            // GameOver

        }
    }
}
