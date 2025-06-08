using UnityEngine;

public class NPcCiudad : MonoBehaviour
{
    public GameObject uiPromptE;      // Icono de "Presiona E"
    public GameObject uiDialogue;     // Panel de diálogo
    public GameObject uiPromptImg;
    public GameObject uiDialogueImg;

    private bool jugadorCerca = false;

    void Start()
    {
        uiPromptE.SetActive(false);
        uiDialogue.SetActive(false);
        uiPromptImg.SetActive(false);
        uiDialogueImg.SetActive(false);
    }

    void FixedUpdate()
    {
        if (jugadorCerca && Input.GetKey(KeyCode.E))
        {
            MostrarDialogo();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = true;
            uiPromptE.SetActive(true);
            uiPromptImg.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = false;
            OcultarTodo();
        }
    }

    private void MostrarDialogo()
    {
        uiPromptE.SetActive(false);
        uiPromptImg.SetActive(false);
        uiDialogue.SetActive(true);
        uiDialogueImg.SetActive(true);
    }

    private void OcultarTodo()
    {
        uiPromptE.SetActive(false);
        uiPromptImg.SetActive(false);
        uiDialogue.SetActive(false);
        uiDialogueImg.SetActive(false);
    }
}
