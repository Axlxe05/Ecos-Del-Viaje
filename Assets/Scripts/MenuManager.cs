using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;
    
    [Header("Referencias UI")]
    public GameObject panel;
    public Slider brilloSlider;
    public Slider volumenSlider;
    public Dropdown graftcosDropdown;
    public Button continuarButton;
    public Button menuPrincipalButton;
    
    [Header("Configuración")]
    public bool pausarJuego = true;
    
    [Header("Post-Processing")]
    public PostProcessVolume postProcessVolume;
    
    private bool isPaused = false;
    private ColorGrading colorGrading;
    private PostProcessProfile profile;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializePostProcessing();
            EnsureActiveState();
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        InitializeUI();
        LoadSettings();
        panel.SetActive(false);
    }

    public void OnMenuSettings(InputValue value)
    {
        TogglePauseMenu();
    }

    private void InitializePostProcessing()
    {
        // Crear nuevo volumen si no existe
        if (postProcessVolume == null)
        {
            var volumeObj = new GameObject("PostProcess Volume");
            postProcessVolume = volumeObj.AddComponent<PostProcessVolume>();
            postProcessVolume.isGlobal = true;
            volumeObj.transform.SetParent(transform);
        }

        // Crear nuevo perfil si no existe
        if (postProcessVolume.profile == null)
        {
            profile = ScriptableObject.CreateInstance<PostProcessProfile>();
            postProcessVolume.profile = profile;
        }
        else
        {
            profile = postProcessVolume.profile;
        }

        // Obtener o crear ColorGrading
        if (!profile.TryGetSettings(out colorGrading))
        {
            colorGrading = profile.AddSettings<ColorGrading>();
            colorGrading.enabled.Override(true);
            colorGrading.gradingMode.Override(GradingMode.LowDefinitionRange);
            Debug.Log("ColorGrading añadido al perfil");
        }
    }

    private void EnsureActiveState()
    {
        if (!gameObject.activeSelf) gameObject.SetActive(true);
        var canvas = GetComponent<Canvas>();
        if (canvas != null) canvas.enabled = true;
    }

    private void InitializeUI()
    {
        continuarButton?.onClick.AddListener(TogglePauseMenu);
        menuPrincipalButton?.onClick.AddListener(ReturnToMainMenu);
        
        brilloSlider?.onValueChanged.AddListener(SetBrightness);
        volumenSlider?.onValueChanged.AddListener(SetVolume);
        graftcosDropdown?.onValueChanged.AddListener(SetGraphicsQuality);
    }

    public void TogglePauseMenu()
    {
        isPaused = !isPaused;
        
        if (panel)
        {
            panel.SetActive(isPaused);
            if (isPaused)
            {
                foreach (Transform child in panel.transform)
                {
                    child.gameObject.SetActive(true);
                }
            }
        }

        if (pausarJuego)
        {
            Time.timeScale = isPaused ? 0 : 1;
            Cursor.visible = isPaused;
            Cursor.lockState = isPaused ? CursorLockMode.None : CursorLockMode.Locked;
        }
    }

    public void SetBrightness(float value)
    {
        if (colorGrading == null)
        {
            Debug.LogError("ColorGrading no inicializado!");
            return;
        }

        // Rango más efectivo para el brillo (-2 a 2)
        float exposureValue = Mathf.Lerp(-2f, 2f, value);
        colorGrading.postExposure.Override(exposureValue);
        Screen.brightness = exposureValue;
        PlayerPrefs.SetFloat("Brillo", value);
        
        Debug.Log($"Brillo actualizado: {value} -> {exposureValue}");
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
        brilloSlider.value = PlayerPrefs.GetFloat("Brillo", 0.5f);
        volumenSlider.value = PlayerPrefs.GetFloat("Volumen", 1f);
        graftcosDropdown.value = PlayerPrefs.GetInt("CalidadGrafica", QualitySettings.GetQualityLevel());
        
        SetBrightness(brilloSlider.value);
        SetVolume(volumenSlider.value);
        SetGraphicsQuality(graftcosDropdown.value);
    }

    void OnDisable()
    {
        PlayerPrefs.Save();
    }
}