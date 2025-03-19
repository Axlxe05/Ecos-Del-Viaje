using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class IA : MonoBehaviour
{
    private string apiUrl = "http://34.227.155.94:5000/predecir"; // URL de la API Flask
    private int previousCollectiblesNumber = -1;  // Variable para almacenar el número de troncos anterior
    public UnityWebRequest request;
    
    // Update is called once per frame
    void Update()
    {
        // Obtener el número actual de troncos recogidos desde GameManager
        int currentCollectiblesNumber = FindAnyObjectByType<GameManager>().collectiblesNumber;

        // Verificar si el número de troncos ha cambiado
        if (currentCollectiblesNumber != previousCollectiblesNumber)
        {
            // Actualizar el número de troncos anterior
            previousCollectiblesNumber = currentCollectiblesNumber;

            // Enviar los datos al servidor solo si el número de troncos ha cambiado
            StartCoroutine(enviarDatos(currentCollectiblesNumber));
        }
    }

    // Método para enviar los datos de la cantidad de troncos recogidos al servidor
    public IEnumerator enviarDatos(int cantidadTroncos)
    {
        // Crear el JSON con la cantidad de troncos que has recogido
        string json = "{\"cantidad_troncos\": " + cantidadTroncos + "}";

        // Convertir el JSON a bytes
        byte[] jsonData = System.Text.Encoding.UTF8.GetBytes(json);

        // Crear la solicitud POST
        request = new UnityWebRequest(apiUrl, "POST");
        request.uploadHandler = new UploadHandlerRaw(jsonData);  // Usamos UploadHandlerRaw para enviar los datos en bruto
        request.downloadHandler = new DownloadHandlerBuffer();   // Usamos DownloadHandlerBuffer para recibir la respuesta
        request.SetRequestHeader("Content-Type", "application/json");  // Definir que el contenido es JSON
        
        
        // Enviar la solicitud
        yield return request.SendWebRequest();

        // Comprobar si hay un error
        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log("Error: " + request.error);
        }
        else
        {
            // Mostrar la respuesta del servidor
            Debug.Log("Respuesta del servidor: " + request.downloadHandler.text);

            // Puedes agregar una lógica adicional aquí para manejar la respuesta, como mostrarla en la interfaz de usuario
            if (request.downloadHandler.text.Contains("Hay 3 troncos recogidos"))
            {
                Debug.Log("¡Felicidades! Has recogido 3 troncos.");
            }
            else
            {
                Debug.Log("Aún no has recogido 3 troncos.");
            }
        }
    }
    
}

