using UnityEngine;
using UnityEngine.SceneManagement;

public class CanastaManager : MonoBehaviour
{
    public static CanastaManager Instance;

    private bool[] goalsReached = new bool[3]; // Para las 3 canastas

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void MarkGoalReached(int index, bool reached)
    {
        goalsReached[index] = reached;

        // Verifica si las 3 están completas
        if (goalsReached[0] && goalsReached[1] && goalsReached[2])
        {
            Debug.Log("¡Nivel completado!");
            SceneManager.LoadScene("Final");
        }
    }
}
