using TMPro;
using UnityEngine;

public class EstadoInteraction : MonoBehaviour
{

    public TMP_Text textoDialogo;
    
    void Start()
    {
        textoDialogo.text = "Necesito que me ayudes a encontrar 3 troncos repartidos en el bosque de atras";
    }


    public void cambiarEstado()
    {
        
        int numTroncos = FindAnyObjectByType<GameManager>().collectiblesNumber;
        
        if (numTroncos > 0 && numTroncos < 3)
        {
            textoDialogo.text = "Termina la misión, por favor. Aún necesitas más troncos.";
        }
        else if (numTroncos == 3)
        {
            textoDialogo.text = "¡Gracias por terminar la misión!";
        }
        
    }

    
}
