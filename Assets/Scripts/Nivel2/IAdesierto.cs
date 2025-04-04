using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
public class IAdesierto : MonoBehaviour
{
    private string apiUrl = "http://localhost:5001/decidir_accion"; // URL de tu API Flask
    private int previousAction = -1;  // Para detectar cambios en la acción
    public UnityWebRequest request;
    
    [Header("Configuración")]
    public float updateInterval = 0.5f; // Intervalo entre consultas
    public Transform player;
    public float moveSpeed = 3f;
    public float fleeSpeed = 5f;
    
    [Header("Estado Actual")]
    [Range(0, 100)] public float enemyHealth = 100f;
    public bool playerIsAttacking = false;
    private float nextUpdateTime;

    void Update()
    {
        // Consulta al servidor en intervalos regulares
        if (Time.time >= nextUpdateTime)
        {
            nextUpdateTime = Time.time + updateInterval;
            StartCoroutine(SendCombatData());
        }
        
        // Ejecutar movimiento continuo (si aplica)
        ExecuteCurrentAction();
    }

    IEnumerator SendCombatData()
    {
        float distance = Vector3.Distance(transform.position, player.position);
    
        // Crear objeto JSON correctamente formateado
        CombatData data = new CombatData
        {
            vida_enemigo = enemyHealth,
            distancia = distance,
            jugador_atacando = playerIsAttacking ? 1 : 0
        };
    
        string json = JsonUtility.ToJson(data);
        byte[] jsonData = System.Text.Encoding.UTF8.GetBytes(json);

        request = new UnityWebRequest(apiUrl, "POST");
        request.uploadHandler = new UploadHandlerRaw(jsonData);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.timeout = 2;

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string response = request.downloadHandler.text;
            CombatResponse responseData = JsonUtility.FromJson<CombatResponse>(response);
        
            if (responseData.status == "success" && responseData.accion != previousAction)
            {
                previousAction = responseData.accion;
                Debug.Log($"Nueva acción: {responseData.accion_texto}");
            }
        }
        else
        {
            Debug.LogError($"Error: {request.error}");
            previousAction = 3; // Default: Quieto
        }
    }

    void ExecuteCurrentAction()
    {
        switch (previousAction)
        {
            case 0: // Atacar
                Attack();
                break;
                
            case 1: // Perseguir
                MoveTowards(player.position, moveSpeed);
                break;
                
            case 2: // Huir
                FleeFrom(player.position, fleeSpeed);
                break;
                
            case 3: // Quieto
                Idle();
                break;
        }
    }

    void Attack()
    {
        // Tu lógica de ataque aquí
        GetComponent<Animator>().SetTrigger("Attack");
    }

    void MoveTowards(Vector3 target, float speed)
    {
        Vector3 direction = (target - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;
        GetComponent<Animator>().SetBool("Moving", true);
    }

    void FleeFrom(Vector3 danger, float speed)
    {
        Vector3 direction = (transform.position - danger).normalized;
        transform.position += direction * speed * Time.deltaTime;
        GetComponent<Animator>().SetBool("Moving", true);
    }

    void Idle()
    {
        GetComponent<Animator>().SetBool("Moving", false);
    }

    // Llamar este método cuando el jugador ataque a este enemigo
    public void RegisterPlayerAttack()
    {
        playerIsAttacking = true;
        Invoke("ResetAttackFlag", 2f); // Reset después de 2 segundos
    }

    void ResetAttackFlag()
    {
        playerIsAttacking = false;
    }

    [System.Serializable]
    public class CombatData
    {
        public float vida_enemigo;
        public float distancia;
        public int jugador_atacando;
    }

    [System.Serializable]
    public class CombatResponse
    {
        public int accion;
        public string accion_texto;
        public string status;
        public string message;
    }
}