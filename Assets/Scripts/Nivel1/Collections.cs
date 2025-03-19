using UnityEngine;

public class Collections : MonoBehaviour
{
    //RECOGER POR ASI DECIRLO
    

    private void OnCollisionEnter(Collision collision)
    {
        if (FindAnyObjectByType<NPCInteraction>().textoTroncos.activeInHierarchy)
        {
            if (collision.transform.CompareTag("Player"))
            {
                FindAnyObjectByType<GameManager>().addCollectible();
                Destroy(transform.gameObject);
                
            }
        }
        
        

    }
  
}
