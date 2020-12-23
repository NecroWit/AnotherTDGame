using UnityEngine;

public class SingletonBehaviour<T> : MonoBehaviour where T: SingletonBehaviour<T>
{
    public static T Instance { get; private set; }
 
    void Awake()
    {
        Init();
    }

    public void Init()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            throw new System.Exception("An instance of this singleton already exists.");
        }
        Instance = (T)this;
    }
}
