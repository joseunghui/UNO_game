using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T instance;
    public static void Load(GameObject SingletonPrefab)
    {
        if (instance == null)
        {
            GameObject NewGameObject = Instantiate(SingletonPrefab);
            instance = NewGameObject.GetComponent<T>();
            DontDestroyOnLoad(NewGameObject);
        }
    }
}
