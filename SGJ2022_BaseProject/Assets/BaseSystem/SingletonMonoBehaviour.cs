using UnityEngine;
using System;

public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    static private T m_instance = null;
    static public T Instance
    {
        get
        {
            if (m_instance == null)
            {
                Type t = typeof(T);

                m_instance = (T)FindObjectOfType(t);
                if (m_instance == null)
                {
                    Debug.LogError(t + " をアタッチしているGameObjectはありません");
                }
            }

            return m_instance;
        }
    }

    protected bool m_isReady = false;
    public bool IsReady { get { return m_isReady; } }

    virtual protected void Start()
    {
        m_isReady = true;
    }

    virtual protected void Awake()
    {
        CheckInstance();
    }

    /// <summary>
    /// 既にInstanceがある場合は破棄する
    /// </summary>
    /// <returns></returns>
    protected bool CheckInstance()
    {
        if (m_instance == null)
        {
            m_instance = this as T;
            return true;
        }
        else if (Instance == this)
        {
            return true;
        }
        Destroy(this);
        return false;
    }

    static public void Create()
    {
        var obj = new GameObject();
        obj.name = typeof(T).Name;
        obj.AddComponent<T>();
        DontDestroyOnLoad(obj);
    }
}
