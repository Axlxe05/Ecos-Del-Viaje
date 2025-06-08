using UnityEngine;
using UnityEngine.SceneManagement;

public class NPcDesierto : MonoBehaviour
{
    public GameObject alertUI;  // La alerta "E" en el Canvas
    public GameObject alertUIImg;
    public GameObject dialogueUI;  // UI del diálogo
    public GameObject dialogueUIImg;
    private bool isPlayerCerca = false;
    
    public GameObject armaPrefab; // Prefab del arma (Firebrand Sword)
    public Transform manoDerecha; // Transform de B-hand.R
    private bool armaDada = false;

    void Start()
    {
        alertUI.SetActive(false);  
        dialogueUI.SetActive(false);
        alertUIImg.SetActive(false);
        dialogueUIImg.SetActive(false);
    }

    void FixedUpdate()
    {
        
        // Si el jugador está cerca y presiona "E"
        if (isPlayerCerca && Input.GetKey(KeyCode.E))
        {
            
            dialogueUI.SetActive(true); // Mostrar diálogo
            dialogueUIImg.SetActive(true);
            alertUI.SetActive(false);
            alertUIImg.SetActive(false);
            
            if (!armaDada)
            {
                DarArma();
                armaDada = true;
            }

        }

        if (isPlayerCerca && EnemigoDesierto.muertes == 3 && Input.GetKey(KeyCode.E))
        {
            alertUI.SetActive(false);  
            dialogueUI.SetActive(false);
            alertUIImg.SetActive(false);
            dialogueUIImg.SetActive(false);
            SceneManager.LoadScene("Nivel 3");
        }
    }

    private void DarArma()
    {
        GameObject arma = Instantiate(armaPrefab, manoDerecha);
        arma.transform.localPosition = Vector3.zero;
        arma.transform.localRotation = Quaternion.identity;

        // Ajustes finos si tu arma no encaja bien:
        arma.transform.localPosition = new Vector3(0f, 0f, 0f);  // Ajusta según tu modelo
        arma.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
    }
    
    // Detectar cuando el jugador entra en el rango
    private void OnTriggerEnter(Collider other)
    {
        
        
        if (other.CompareTag("Player"))  // Asegurar que es el jugador
        {
            alertUI.SetActive(true);  // Mostrar la alerta "E"
            alertUIImg.SetActive(true);
            isPlayerCerca = true;
        }
    }

    // Detectar cuando el jugador sale del rango
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            alertUI.SetActive(false); // Ocultar la alerta "E"
            alertUIImg.SetActive(false);
            dialogueUI.SetActive(false); // Cerrar diálogo al salir
            dialogueUIImg.SetActive(false);
            isPlayerCerca = false;
        }
    }
    
}