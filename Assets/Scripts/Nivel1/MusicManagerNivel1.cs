using UnityEngine;
using UnityEngine.SceneManagement;

namespace Nivel1
{
    public class MusicManagerNivel1 : MonoBehaviour
    {
        private static MusicManagerNivel1 _instance;
        private AudioSource audioSource;

        void Awake()
        {

            if (_instance == null)
            {
                _instance = this;
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
            if (scene.name == "Nivel 2" && audioSource != null)
            {
                audioSource.Stop();
                
            }
        }

        void OnDestroy()
        {
            // Importante: Desuscribirse del evento al destruir el objeto
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }
}