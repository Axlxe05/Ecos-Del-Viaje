using UnityEngine;
using UnityEngine.InputSystem;

public class Jugador : MonoBehaviour
{
    private Rigidbody rb;
    public float speed = 5f;
    public float sprintSpeed = 15f;
    private float currentSpeed;
    public Vector2 velocity;
    
    // variables para la rotación de la cámara
    public float mouseSensitivity = 25f;
    public Transform cameraTransform; // Referencia al transform de la cámara
    private float xRotation = 0f;
    
    // variables salto
    public float jumpForce = 5f; // Fuerza del salto
    private bool isGrounded; // Verifica si el personaje está en el suelo

    public GameObject madera;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentSpeed = speed;
        Cursor.lockState = CursorLockMode.Locked;
    }


    void FixedUpdate()
    {
        Vector3 movement = new Vector3(velocity.x * currentSpeed, rb.linearVelocity.y, velocity.y * currentSpeed);
        rb.linearVelocity = transform.TransformDirection(movement);
        checkGrounded();
    }
 
    public void OnMove(InputValue value)
    {
        velocity = value.Get<Vector2>();
    }
    
    // Método para capturar el movimiento del ratón
    public void OnLook(InputValue value)
    {
        Vector2 mouseDelta = value.Get<Vector2>();

        float mouseX = mouseDelta.x * mouseSensitivity * Time.deltaTime;
        float mouseY = mouseDelta.y * mouseSensitivity * Time.deltaTime;

        // Rotación vertical (arriba/abajo)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Aplicar la rotación a la cámara
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Rotación horizontal (izquierda/derecha)
        transform.Rotate(Vector3.up * mouseX);
    }
    
    // Método para verificar si el personaje está en el suelo
    private void checkGrounded()
    {
        // Lanzar un rayo hacia abajo para detectar el suelo
        float rayDistance = 1.1f; // Distancia del rayo (ajusta según el tamaño del personaje)
        isGrounded = Physics.Raycast(transform.position, Vector3.down, rayDistance);
    }

    
    
    // Método para el salto
    public void OnJump(InputValue value)
    {
        if (isGrounded) // Solo saltar si está en el suelo
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
    
    // Activar y desactivar sprint
    public void OnSprint(InputValue value)
    {
        if (value.isPressed)
        {
            currentSpeed = sprintSpeed;
        }
        else
        {
            currentSpeed = speed;
        }
    }

    public void objetivo()
    {

        if (FindAnyObjectByType<IA>().request.downloadHandler.text.Contains("\"resultado\": \"Hay 3 troncos recogidos\""))
        {
            Destroy(madera);
        }
        
        
    }
    
}
