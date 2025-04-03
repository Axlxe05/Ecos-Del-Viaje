using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;
    
    [Header("Referencias UI")]
    public GameObject panel;  // Panel principal
    public Slider brilloSlider;
    public Slider volumenSlider;
    public Dropdown graftcosDropdown;
    public Button continuarButton;
    public Button menuPrincipalButton;
    
    [Header("Configuración")]
    public bool pausarJuego = true; // Si debe pausar el juego al mostrar menú
    
    private bool isPaused = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            EnsureActiveState(); // Garantizar estado correcto
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        InitializeUI();
        LoadSettings();
        panel.SetActive(false); // Asegurar que empiece oculto
    }

    public void OnJump(InputValue value)
    {
        TogglePauseMenu();
    }

    private void EnsureActiveState()
    {
        // Asegurar que el GameObject principal está activo
        if (!gameObject.activeSelf) gameObject.SetActive(true);
        
        // Asegurar que el Canvas está activo
        var canvas = GetComponent<Canvas>();
        if (canvas != null) canvas.enabled = true;
    }

    private void InitializeUI()
    {
        // Configurar listeners
        continuarButton?.onClick.AddListener(TogglePauseMenu);
        menuPrincipalButton?.onClick.AddListener(ReturnToMainMenu);
        
        if (brilloSlider != null) brilloSlider.onValueChanged.AddListener(SetBrightness);
        if (volumenSlider != null) volumenSlider.onValueChanged.AddListener(SetVolume);
        if (graftcosDropdown != null) graftcosDropdown.onValueChanged.AddListener(SetGraphicsQuality);
    }

    public void TogglePauseMenu()
    {
        isPaused = !isPaused;
        
        // Activar/desactivar panel y sus hijos
        if (panel)
        {
            panel.SetActive(isPaused);
            
            // Activar todos los hijos por si alguno estaba desactivado individualmente
            if (isPaused)
            {
                foreach (Transform child in panel.transform)
                {
                    child.gameObject.SetActive(true);
                }
            }
        }

        // Manejar pausa del juego
        if (pausarJuego)
        {
            Time.timeScale = isPaused ? 0 : 1;
            Cursor.visible = isPaused;
            Cursor.lockState = isPaused ? CursorLockMode.None : CursorLockMode.Locked;
        }
    }

    public void SetBrightness(float value)
    {
        // Implementar ajuste de brillo
        // Esto es un ejemplo básico - considera usar Post-Processing para mejor efecto
        RenderSettings.ambientLight = new Color(value, value, value);
        PlayerPrefs.SetFloat("Brillo", value);
    }

    public void SetVolume(float value)
    {
        AudioListener.volume = value;
        PlayerPrefs.SetFloat("Volumen", value);
    }

    public void SetGraphicsQuality(int level)
    {
        QualitySettings.SetQualityLevel(level);
        PlayerPrefs.SetInt("CalidadGrafica", level);
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }

    private void LoadSettings()
    {
        // Cargar valores guardados o usar defaults
        brilloSlider.value = PlayerPrefs.GetFloat("Brillo", 0.8f);
        volumenSlider.value = PlayerPrefs.GetFloat("Volumen", 1f);
        graftcosDropdown.value = PlayerPrefs.GetInt("CalidadGrafica", QualitySettings.GetQualityLevel());
        
        // Aplicar los valores cargados
        SetBrightness(brilloSlider.value);
        SetVolume(volumenSlider.value);
        SetGraphicsQuality(graftcosDropdown.value);
    }
}
