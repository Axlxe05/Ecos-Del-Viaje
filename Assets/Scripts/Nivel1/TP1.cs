using UnityEngine;

public class TP1 : MonoBehaviour
{

    private Vector3 teleportPosition;


    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            teleportPosition = new Vector3(23f,1f,145f);
            other.transform.position = teleportPosition;
        }
    }
    
}
