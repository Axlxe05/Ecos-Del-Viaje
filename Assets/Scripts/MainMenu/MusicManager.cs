using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    private static MusicManager instance;
    private AudioSource audioSource;

    void Awake()
    {
        // Implementación del patrón Singleton
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource = GetComponent<AudioSource>();
            
            // Suscribirse al evento de cambio de escena
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Detener la música si la escena cargada es "Nivel 1"
        if (scene.name == "Nivel 1" && audioSource != null)
        {
            audioSource.Stop();
            
            // Opcional: Destruir el objeto de música si ya no se necesita
            // Destroy(gameObject);
        }
    }

    void OnDestroy()
    {
        // Importante: Desuscribirse del evento al destruir el objeto
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}