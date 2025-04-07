using UnityEngine;

public class EnemigoDesierto : MonoBehaviour
{
    public int vida = 100;

    public void TakeDamage(int damage)
    {
        vida -= damage;
        Debug.Log("Vida actual del enemigo: " + vida);

        if (vida <= 0)
        {
            die();
        }
    }

    void die()
    {
        Debug.Log("El enemigo ha muerto.");
        
        Destroy(gameObject);
    }
}