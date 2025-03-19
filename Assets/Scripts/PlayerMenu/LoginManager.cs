using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;
using UnityEngine.UI;

public class LoginManager : MonoBehaviour
{
    public Button botonLogin;
    public Button botonRegistar;
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;
    public TMP_Text mensajeError;
    private string urlLogin = "http://localhost/APIRestPlayerUnity/crud/leer.php"; // URL de tu PHP

    void Start()
    {
        botonLogin.onClick.AddListener(IniciarSesion);
        
        botonRegistar.onClick.AddListener(registrar);
    }

    public void IniciarSesion()
    {
        StartCoroutine(EnviarDatosLogin());
    }

    IEnumerator EnviarDatosLogin()
    {
        // Enviar los datos del formulario (email y password)
        WWWForm form = new WWWForm();
        form.AddField("email", emailInput.text);
        form.AddField("password", passwordInput.text);

        using (UnityWebRequest www = UnityWebRequest.Post(urlLogin, form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                // Obtener la respuesta del servidor y deserializarla
                string json = www.downloadHandler.text.Trim();

                // Deserializar el JSON en una lista de usuarios
                ListaUsuarios listaUsuarios = JsonUtility.FromJson<ListaUsuarios>("{\"usuarios\":" + json + "}");

                bool usuarioValido = false;

                // Buscar si el email y la contraseña coinciden con algún usuario
                foreach (Usuario usuario in listaUsuarios.usuarios)
                {
                    if (usuario.email == emailInput.text && usuario.password == passwordInput.text)
                    {
                        usuarioValido = true;
                        break;
                    }
                }

                // Si el usuario es válido, se pasa a la siguiente escena
                if (usuarioValido)
                {
                    Debug.Log("Login correcto");
                    SceneManager.LoadScene("Nivel 1"); // Cambia "Nivel 1" por tu escena
                }
                else
                {
                    // Si no es válido, muestra un mensaje de error
                    mensajeError.text = "Correo o contraseña incorrectos";
                }
            }
            else
            {
                // Si hubo un error en la conexión con el servidor
                mensajeError.text = "Error de conexión con el servidor";
            }
        }
    }

    public void registrar()
    {

        Debug.Log("Hacia registrar");
        SceneManager.LoadScene("RegisterPlayerMenu"); 
        
    }
    
    
}

[System.Serializable]
public class Usuario
{
    public int id;
    public string nombre;
    public string email;
    public string password;
    public string imagen;
}

[System.Serializable]
public class ListaUsuarios
{
    public Usuario[] usuarios; // Esto será un array de objetos de tipo 'Usuario'
}