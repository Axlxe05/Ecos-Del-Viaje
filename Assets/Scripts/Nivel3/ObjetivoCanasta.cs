using UnityEngine;

public class ObjetivoCanasta : MonoBehaviour
{
    public string pelotas = "Movible"; 
    public int numCanasta; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(pelotas))
        {
            CanastaManager.Instance.MarkGoalReached(numCanasta, true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(pelotas))
        {
            CanastaManager.Instance.MarkGoalReached(numCanasta, false);
        }
    }
}
