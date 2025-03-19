using UnityEngine;

public class MusicManager : MonoBehaviour
{

    private static MusicManager instance;
    
    void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    
}
