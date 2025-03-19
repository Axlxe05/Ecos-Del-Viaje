using System;
using TMPro;
using UnityEngine;

public class NPCInteraction : MonoBehaviour
{
    public GameObject alertUI;  // La alerta "E" en el Canvas
    public GameObject alertUIImg;
    public GameObject dialogueUI;  // UI del diálogo
    public GameObject dialogueUIImg;
    public GameObject textoTroncos;  
    public GameObject numTroncos;
    private bool isPlayerCerca = false;

    void Start()
    {
        alertUI.SetActive(false);  
        dialogueUI.SetActive(false);
        alertUIImg.SetActive(false);
        dialogueUIImg.SetActive(false);
        textoTroncos.SetActive(false);
        numTroncos.SetActive(false);
    }

    void Update()
    {
        
        // Si el jugador está cerca y presiona "E"
        if (isPlayerCerca && Input.GetKey(KeyCode.E))
        {
            
            dialogueUI.SetActive(true); // Mostrar diálogo
            dialogueUIImg.SetActive(true);
            alertUI.SetActive(false);
            alertUIImg.SetActive(false);
            
            FindAnyObjectByType<EstadoInteraction>().cambiarEstado();

            FindAnyObjectByType<Jugador>().objetivo();
            
        }
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
            textoTroncos.SetActive(true);
            numTroncos.SetActive(true);
        }
    }

    

    public GameObject TextoTroncos => textoTroncos;
}
