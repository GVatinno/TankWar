using UnityEngine;

public abstract class SceneSingleton<T> : MonoBehaviour where T : SceneSingleton<T>
{

    private static T mInstance;


    public static T Instance
    {
        get
        {
            if (mInstance == null)
            {

                mInstance = FindObjectOfType(typeof(T)) as T;
            }
            return mInstance;
        }
    }

    protected void Awake()
    {
        if (mInstance == null)
        {
            mInstance = this as T;
            Init();
        }
        else if (mInstance != this)
        {
            gameObject.SetActive(false);
        }
    }

    protected virtual void OnEnable()
    {
        if (mInstance != this)
        {
            gameObject.SetActive(false);
        }
    }

    protected void OnApplicationQuit()
    {
        Destroy(gameObject);
        mInstance = null;
    }

    protected virtual void OnDestroy()
    {
        mInstance = null;
    }

    protected abstract void Init();
}