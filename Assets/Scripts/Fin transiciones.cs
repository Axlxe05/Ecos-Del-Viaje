using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class Fintransiciones : MonoBehaviour
{
    private VideoPlayer videoPlayer;

    void Start()
    {
        // Obtener el componente VideoPlayer
        videoPlayer = GetComponent<VideoPlayer>();

        // Suscribirse al evento de finalización del video
        videoPlayer.loopPointReached += OnVideoEnd;
    }

    // Método llamado cuando el video termina
    private void OnVideoEnd(VideoPlayer vp)
    {
        // Cargar la escena "Nivel 1"
        SceneManager.LoadScene("Nivel 1");
    }

    // Opcional: Saltar el video con una tecla (ej: Espacio)
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene("Nivel 1");
        }
    }
}