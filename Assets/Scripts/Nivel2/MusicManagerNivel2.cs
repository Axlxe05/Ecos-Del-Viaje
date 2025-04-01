using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManagerNivel2 : MonoBehaviour
{
    private static MusicManagerNivel2 instance;
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
        // Detener la música si la escena cargada es "Nivel 3"
        if (scene.name == "Nivel 3" && audioSource != null)
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