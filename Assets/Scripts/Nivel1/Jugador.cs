using UnityEngine;
using UnityEngine.InputSystem;

public class Jugador : MonoBehaviour
{
    private Rigidbody rb;
    public float speed = 5f;
    public float sprintSpeed = 15f;
    private float currentSpeed;
    public Vector2 velocity;

    // Variables para la rotación de la cámara
    public float mouseSensitivity = 25f;
    public Transform cameraTransform; // Referencia al transform de la cámara
    private float xRotation = 0f; // Rotación vertical de la cámara

    // Variables para el salto
    public float jumpForce = 5f; // Fuerza del salto
    private bool isGrounded; // Verifica si el personaje está en el suelo

    public GameObject madera;

    // Variables para el cambio de cámara
    private bool isFirstPerson = true; // Estado actual de la cámara
    private Vector3 firstPersonOffset = new Vector3(0, 1.5f, 0); // Offset para primera persona
    private Vector3 thirdPersonOffset = new Vector3(0, 2.0f, -5.0f); // Offset para tercera persona
    public float smoothSpeed = 5.0f; // Velocidad de transición

    // Animator
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentSpeed = speed;
        Cursor.lockState = CursorLockMode.Locked;

        // Obtener el componente Animator del hijo DummyModel_Male
        animator = GetComponentInChildren<Animator>();

        // Configura el Rigidbody
        rb.interpolation = RigidbodyInterpolation.Interpolate; // Suaviza el movimiento
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic; // Mejora la detección de colisiones
        rb.freezeRotation = true; // Congela la rotación en X y Z
    }

    void FixedUpdate()
    {
        // Movimiento del jugador
        Vector3 movement = new Vector3(velocity.x * currentSpeed, rb.linearVelocity.y, velocity.y * currentSpeed);
        rb.linearVelocity = transform.TransformDirection(movement);

        // Limitar la velocidad horizontal
        Vector3 horizontalVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        if (horizontalVelocity.magnitude > currentSpeed)
        {
            horizontalVelocity = horizontalVelocity.normalized * currentSpeed;
            rb.linearVelocity = new Vector3(horizontalVelocity.x, rb.linearVelocity.y, horizontalVelocity.z);
        }

        checkGrounded();
        
        // Cambiar entre primera y tercera persona al presionar F5
        if (Input.GetKeyDown(KeyCode.F5))
        {
            isFirstPerson = !isFirstPerson; // Alternar entre modos

            // Reiniciar la rotación vertical de la cámara al cambiar a primera persona
            if (isFirstPerson)
            {
                xRotation = 0f; // Reiniciar la rotación vertical
            }
        }

        // Mover la cámara al modo correspondiente
        if (isFirstPerson)
        {
            MoveCamera(firstPersonOffset);
        }
        else
        {
            MoveCamera(thirdPersonOffset);
        }

        // Rotación de la cámara y el personaje
        RotateCameraAndPlayer();

        // Controlar la animación de caminar
        ControlarAnimacionCaminar();
    }

    void MoveCamera(Vector3 targetOffset)
    {
        // Calcular la posición deseada de la cámara
        Vector3 desiredPosition = transform.position + transform.TransformDirection(targetOffset);

        // Suavizar el movimiento de la cámara
        Vector3 smoothedPosition = Vector3.Lerp(cameraTransform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        cameraTransform.position = smoothedPosition;

        // En tercera persona, la cámara siempre mira hacia el jugador
        if (!isFirstPerson)
        {
            cameraTransform.LookAt(transform.position + Vector3.up * firstPersonOffset.y);
        }
    }

    void RotateCameraAndPlayer()
    {
        // Capturar el movimiento del ratón
        Vector2 mouseDelta = Mouse.current.delta.ReadValue();

        float mouseX = mouseDelta.x * mouseSensitivity * Time.deltaTime;
        float mouseY = mouseDelta.y * mouseSensitivity * Time.deltaTime;

        if (isFirstPerson)
        {
            // En primera persona, la rotación vertical afecta solo a la cámara
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);
            cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

            // Rotación horizontal afecta al personaje
            transform.Rotate(Vector3.up * mouseX);
        }
        else
        {
            // En tercera persona:
            // Rotación horizontal afecta solo al personaje
            transform.Rotate(Vector3.up * mouseX);

            // Rotación vertical afecta solo a la cámara
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -30f, 60f); // Limitar la rotación vertical

            // Calcular la rotación vertical de la cámara
            Quaternion verticalRotation = Quaternion.Euler(xRotation, transform.eulerAngles.y, 0f);

            // Calcular la posición de la cámara en tercera persona
            Vector3 offset = verticalRotation * thirdPersonOffset;
            cameraTransform.position = transform.position + offset;

            // Hacer que la cámara mire hacia el jugador
            cameraTransform.LookAt(transform.position + Vector3.up * firstPersonOffset.y);
        }
    }

    void ControlarAnimacionCaminar()
    {
        // Activar o desactivar la animación de caminar según si el jugador se está moviendo
        if (velocity.magnitude > 0.1f) // Si el jugador se está moviendo
        {
            animator.SetBool("isWalking", true);
        }
        else // Si el jugador está quieto
        {
            animator.SetBool("isWalking", false);
        }
    }

    public void OnMove(InputValue value)
    {
        velocity = value.Get<Vector2>();
        Debug.Log("Movimiento detectado: " + velocity);
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