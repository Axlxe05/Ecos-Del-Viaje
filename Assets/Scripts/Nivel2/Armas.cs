using UnityEngine;

public class Armas : MonoBehaviour
{
    public Collider armaCollider;

    private void Start()
    {
        armaCollider.enabled = false;
    }

    public void EnableCollider()
    {
        armaCollider.enabled = true;
    }

    public void DisableCollider()
    {
        armaCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        EnemigoDesierto vidaEnemigoDesierto = other.GetComponent<EnemigoDesierto>();
        if (vidaEnemigoDesierto != null)
        {
            vidaEnemigoDesierto.TakeDamage(20);
        }
    }
}
