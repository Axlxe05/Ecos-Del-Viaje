using UnityEngine;
using UnityEngine.SceneManagement;

public class TPDesert : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        // Verifica si el objeto que entra en contacto es el jugador (usando tags)
        if (other.CompareTag("Player"))
        {
            // Carga la escena "Nivel 2"
            SceneManager.LoadScene("Nivel 2");
        }
    }

    // Opcional: Dibuja un gizmo en el editor para visualizar el plano invisible
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 1, 0, 0.5f); // Verde semitransparente
        Gizmos.DrawCube(transform.position, transform.localScale);
    }
    
}
