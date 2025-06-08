using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class ApiProgresoDesierto : MonoBehaviour
{
    private string urlGuardarProgreso = "http://13.219.131.239/APIRestProgresoUnity/crud/actualizar.php";

    public int jugadorId; // Este debe venir del login (usuario que ha iniciado sesión)
    public string mapaActual = "Desierto"; // Nombre del nivel o mapa
    public int muertes = 0;
    public int tiempoJugado = 0; // Puedes actualizarlo luego si quieres contar tiempo real

    
    
    
    public void GuardarProgreso()
    {
        StartCoroutine(EnviarProgreso());
        
    }

    IEnumerator EnviarProgreso()
    {
        WWWForm form = new WWWForm();
        form.AddField("jugador_id", LoginManager.getIdJugador());
        form.AddField("mapa_actual", mapaActual);
        form.AddField("muertes", muertes);
        form.AddField("tiempo_jugado", tiempoJugado);

        using (UnityWebRequest www = UnityWebRequest.Post(urlGuardarProgreso, form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Progreso guardado correctamente: " + www.downloadHandler.text);
            }
            else
            {
                Debug.LogError("Error al guardar progreso: " + www.error);
            }
        }
    }
}