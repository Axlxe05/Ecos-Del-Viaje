using System;
using UnityEngine;

public class Armas : MonoBehaviour
{
    public Collider armaCollider;
    public Animator jugadorAnimator;

    private void Start()
    {
        armaCollider.enabled = false;
    }

    private void Update()
    {
        // Verifica si la animación "Attack" está activa
        if (jugadorAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            EnableCollider();
        }
        else
        {
            DisableCollider();
        }
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
