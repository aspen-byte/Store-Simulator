using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance { get; private set; }

    private const float DEFAULT_SENSITIVITY = 2f;
    private const bool DEFAULT_INVERT_X = false;
    private const bool DEFAULT_INVERT_Y = false;
    private const KeyCode DEFAULT_INTERACT_KEY = KeyCode.E;
    private int defaultGraphicsIndex;

    [Header("Graphics Settings")]
    [SerializeField] private TMP_Dropdown graphicsDropdown;

    [Header("Mouse Controls")]
    [SerializeField] private Slider sensitivitySlider;
    [SerializeField] private Toggle invertXToggle;
    [SerializeField] private Toggle invertYToggle;

    [Header("Keybinding UI")]
    [SerializeField] private TMP_Text interactKeyText;
    private KeyCode interactKey = DEFAULT_INTERACT_KEY;
    private bool isRebindingKey = false;

    public float Sensitivity { get; private set; } = DEFAULT_SENSITIVITY;
    public bool InvertX { get; private set; } = DEFAULT_INVERT_X;
    public bool InvertY { get; private set; } = DEFAULT_INVERT_Y;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        if (graphicsDropdown != null)
        {
            graphicsDropdown.ClearOptions();
            List<string> options = new List<string>(QualitySettings.names);
            graphicsDropdown.AddOptions(options);
        }

        defaultGraphicsIndex = QualitySettings.names.Length - 1;

        LoadSettings();
    }

    // Graphics Quality
    public void SetGraphicsQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        PlayerPrefs.SetInt("GraphicsQuality", qualityIndex);
    }

    // Mouse Sensitivity
    public void SetSensitivity(float value)
    {
        Sensitivity = value;
        PlayerPrefs.SetFloat("MouseSensitivity", Sensitivity);
    }

    // Axis Inversion
    public void SetInvertX(bool isInverted)
    {
        InvertX = isInverted;
        PlayerPrefs.SetInt("InvertX", InvertX ? 1 : 0);
    }

    public void SetInvertY(bool isInverted)
    {
        InvertY = isInverted;
        PlayerPrefs.SetInt("InvertY", InvertY ? 1 : 0);
    }

    // Keybinding
    public void StartRebindingInteractKey()
    {
        if (!isRebindingKey) StartCoroutine(RebindKeyRoutine());
    }

    private IEnumerator RebindKeyRoutine()
    {
        isRebindingKey = true;
        if (interactKeyText != null) interactKeyText.text = "Press Any Key...";

        yield return null;

        while (!Input.anyKeyDown) yield return null;

        foreach (KeyCode key in Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(key))
            {
                interactKey = key;
                PlayerPrefs.SetString("InteractKey", key.ToString());
                if (interactKeyText != null) interactKeyText.text = key.ToString();
                break;
            }
        }
        isRebindingKey = false;
    }

    public KeyCode GetInteractKey() { return interactKey; }

    // Reset to Defaults
    public void ResetToDefaults()
    {
        StopAllCoroutines();
        isRebindingKey = false;

        SetSensitivity(DEFAULT_SENSITIVITY);
        SetInvertX(DEFAULT_INVERT_X);
        SetInvertY(DEFAULT_INVERT_Y);
        SetGraphicsQuality(defaultGraphicsIndex);

        interactKey = DEFAULT_INTERACT_KEY;
        PlayerPrefs.SetString("InteractKey", DEFAULT_INTERACT_KEY.ToString());

        if (sensitivitySlider != null) sensitivitySlider.value = DEFAULT_SENSITIVITY;
        if (invertXToggle != null) invertXToggle.isOn = DEFAULT_INVERT_X;
        if (invertYToggle != null) invertYToggle.isOn = DEFAULT_INVERT_Y;
        if (interactKeyText != null) interactKeyText.text = DEFAULT_INTERACT_KEY.ToString();

        if (graphicsDropdown != null)
        {
            graphicsDropdown.value = defaultGraphicsIndex;
            graphicsDropdown.RefreshShownValue();
        }

        PlayerPrefs.Save();
    }

    // Load Settings
    private void LoadSettings()
    {
        Sensitivity = PlayerPrefs.GetFloat("MouseSensitivity", DEFAULT_SENSITIVITY);
        InvertX = PlayerPrefs.GetInt("InvertX", DEFAULT_INVERT_X ? 1 : 0) == 1;
        InvertY = PlayerPrefs.GetInt("InvertY", DEFAULT_INVERT_Y ? 1 : 0) == 1;

        int savedQuality = PlayerPrefs.GetInt("GraphicsQuality", defaultGraphicsIndex);
        QualitySettings.SetQualityLevel(savedQuality);

        string savedKey = PlayerPrefs.GetString("InteractKey", DEFAULT_INTERACT_KEY.ToString());
        Enum.TryParse(savedKey, out interactKey);

        if (sensitivitySlider != null) sensitivitySlider.value = Sensitivity;
        if (invertXToggle != null) invertXToggle.isOn = InvertX;
        if (invertYToggle != null) invertYToggle.isOn = InvertY;
        if (interactKeyText != null) interactKeyText.text = interactKey.ToString();

        if (graphicsDropdown != null)
        {
            graphicsDropdown.value = savedQuality;
            graphicsDropdown.RefreshShownValue();
        }
    }
}