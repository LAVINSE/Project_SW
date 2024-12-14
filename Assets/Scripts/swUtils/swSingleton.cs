using UnityEngine;

public class SwSingleton<T> : MonoBehaviour where T : Component
{
    #region 변수
    private static T instance;
    #endregion // 변수

    #region 프로퍼티
    public static T Instance
    {
        get
        {
            if(instance == null)
            {
                instance = (T)FindAnyObjectByType(typeof(T));

                if(instance == null)
                {
                    SetupInstance();
                }
            }

            return instance;
        }
    }
    #endregion // 프로퍼티

    #region 함수
    private static void SetupInstance()
    {
        instance = (T)FindAnyObjectByType(typeof(T));

        if (instance == null)
        {
            GameObject obj = new GameObject();
            obj.name = typeof(T).Name;
            instance = obj.AddComponent<T>();
            DontDestroyOnLoad(obj);
        }
    }

    public virtual void Awake()
    {
        RemoveDuplicates();
    }

    private void RemoveDuplicates()
    {
        if(instance == null)
        {
            instance = this as T;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    #endregion // 함수
}
