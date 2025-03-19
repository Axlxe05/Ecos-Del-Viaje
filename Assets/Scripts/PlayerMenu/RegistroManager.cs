using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Text;

public class RegistroManager : MonoBehaviour
{
    public TMP_InputField nombreInput;   // Campo de nombre
    public TMP_InputField emailInput;    // Campo de email
    public TMP_InputField passwordInput; // Campo de contraseña
    public TMP_InputField imagenInput;   // Campo de imagen (si es una URL o ruta de imagen)
    public TMP_Text mensajeError;        // Texto donde se muestra el error
    public Button botonRegistrar;        // Botón para registrar

    private string urlRegistrar = "http://localhost/APIRestPlayerUnity/crud/insertar.php"; // Cambia esto por la URL real

    void Start()
    {
        // Asignar la función de registro al botón
        botonRegistrar.onClick.AddListener(RegistrarUsuario);
    }

    // Función que se llama al presionar el botón de "Registrar"
    public void RegistrarUsuario()
    {
        StartCoroutine(EnviarDatosRegistro());
    }

    // Enviar los datos del registro al servidor
    IEnumerator EnviarDatosRegistro()
    {
        // Crear un objeto con los datos del usuario
        Usuario usuario = new Usuario()
        {
            nombre = nombreInput.text,
            email = emailInput.text,
            password = passwordInput.text,
            imagen = imagenInput.text  // La URL o ruta de la imagen
        };

        // Convertir el objeto a formato JSON
        string jsonData = JsonUtility.ToJson(usuario);

        // Crear la petición HTTP
        using (UnityWebRequest www = new UnityWebRequest(urlRegistrar, "POST"))
        {
            byte[] jsonToSend = new UTF8Encoding().GetBytes(jsonData);
            www.uploadHandler = new UploadHandlerRaw(jsonToSend);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");  // Indicamos que estamos enviando JSON

            // Esperar la respuesta del servidor
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                // Obtener la respuesta JSON
                string json = www.downloadHandler.text.Trim();

                // Verificar el estado de la respuesta
                if (json.Contains("\"info\" => \"Jugador/Jugadora Creado!\""))
                {
                    Debug.Log("Registro exitoso");

                    // Aquí podrías guardar información adicional si lo necesitas (por ejemplo, el ID del usuario)
                    // Para el ejemplo solo redirigimos a LoginPlayerMenu
                    SceneManager.LoadScene("LoginPlayerMenu"); // Cambia el nombre de la escena si es necesario
                }
                else
                {
                    mensajeError.text = "Error al registrar: " + json;  // Mostrar el mensaje de error
                }
            }
            else
            {
                mensajeError.text = "Error de conexión con el servidor";
            }
        }
    }
}
