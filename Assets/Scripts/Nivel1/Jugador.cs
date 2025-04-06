using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Jugador : MonoBehaviour
{
    private static readonly int Velocity = Animator.StringToHash("velocity");
    private static readonly int Attack = Animator.StringToHash("attack");
    private static readonly int Jump = Animator.StringToHash("jump");
    
    private Rigidbody rb;
    [Header("Velocidad")]
    public float speed = 2f;
    public float sprintSpeed = 7f;
    private float currentSpeed;
    public Vector2 velocity;

    [Header("Rotacion Camera")]
    public float mouseSensitivity = 25f;
    public Transform cameraTransform; // Referencia al transform de la cámara
    private float xRotation = 0f; // Rotación vertical de la cámara

    [Header("Variables Salto")]
    public float jumpForce = 5f; // Fuerza del salto
    private bool isGrounded; // Verifica si el personaje está en el suelo

    public GameObject madera;

    // Variables para el cambio de cámara
    private bool isFirstPerson = true; // Estado actual de la cámara
    private Vector3 firstPersonOffset = new(0, 1.5f, 0); // Offset para primera persona
    private Vector3 thirdPersonOffset = new(0, 2.0f, -5.0f); // Offset para tercera persona
    public float smoothSpeed = 5.0f; // Velocidad de transición

    // Añade estas variables al inicio de tu clase
    private Animator animator;
    private bool isWalking;
    
    private bool attacking;


    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentSpeed = speed;
        Cursor.lockState = CursorLockMode.Locked;

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

        //Assign animator velocity variable with rigidbody velocity
        animator.SetFloat(Velocity, rb.linearVelocity.magnitude);

        animator.SetFloat(Jump, rb.linearVelocity.y);

        checkGrounded();

        // Rotación de la cámara y el personaje
        RotateCameraAndPlayer();
    }

    void MoveCamera(Vector3 targetOffset)
    {
        // Calcular la posición deseada de la cámara
        Vector3 desiredPosition = transform.position + transform.TransformDirection(targetOffset);
        cameraTransform.position = desiredPosition;

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

    public void OnMove(InputValue value)
    {
        velocity = value.Get<Vector2>();
        Debug.Log("Movimiento detectado: " + velocity);
    }

    public void OnChangeCamera(InputValue value)
    {
        // Mover la cámara al modo correspondiente
        if (!isFirstPerson)
        {
            xRotation = 0f; // Reiniciar la rotación vertical
            MoveCamera(firstPersonOffset);
            print("Entering first person mode");
            isFirstPerson = true;
        }
        else
        {
            MoveCamera(thirdPersonOffset);
            print("Entering third person mode");
            isFirstPerson = false;
        }
    }

// Método para verificar si el personaje está en el suelo
    private void checkGrounded()
    {
        // Lanzar un rayo hacia abajo para detectar el suelo
        float rayDistance = 1.1f; // Distancia del rayo (ajusta según el tamaño del personaje)
        isGrounded = Physics.Raycast(transform.position, Vector3.down, rayDistance);

        if (isGrounded)
        {
            animator.SetBool(Jump, false);
        }
    }

    // Método para el salto
    public void OnJump(InputValue value)
    {
        if (isGrounded) // Solo saltar si está en el suelo
        {
            animator.SetBool(Jump, true);
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


    public void OnMenuSettings(InputValue value)
    {
        MenuManager menuManager = FindObjectOfType<MenuManager>(true);

        if (menuManager != null)
        {
            menuManager.TogglePauseMenu();
        }
        else
        {
            Debug.LogError("MenuManager no encontrado en la escena");
        }
    }

    private IEnumerator Atacar()
    {
        attacking = true;
        animator.SetBool(Attack, true);
        yield return new WaitForSeconds(1.3f);
        animator.SetBool(Attack, false);
        attacking = false;
        
    }

    public void OnAttack(InputValue value)
    {
        if (attacking) return;
        StartCoroutine(Atacar());
    } 

public void objetivo()
    {
        if (FindAnyObjectByType<IA>().request.downloadHandler.text.Contains("\"resultado\": \"Hay 3 troncos recogidos\""))
        {
            Destroy(madera);
        }
    }
}