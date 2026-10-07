using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance { get; private set; }

    // Default Values
    private const float DEFAULT_SENSITIVITY = 2f;
    private const bool DEFAULT_INVERT_X = false;
    private const bool DEFAULT_INVERT_Y = false;
    private const KeyCode DEFAULT_INTERACT_KEY = KeyCode.E;

    [Header("Mouse Controls")]
    [SerializeField] private Slider sensitivitySlider;
    [SerializeField] private Toggle invertXToggle;
    [SerializeField] private Toggle invertYToggle;

    [Header("Keybinding UI")]
    [SerializeField] private TMP_Text interactKeyText;
    private KeyCode interactKey = DEFAULT_INTERACT_KEY;
    private bool isRebindingKey = false;

    // Public properties to read from player movement / camera scripts
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
        LoadSettings();
    }

    // MOUSE SENSITIVITY
    public void SetSensitivity(float value)
    {
        Sensitivity = value;
        PlayerPrefs.SetFloat("MouseSensitivity", Sensitivity);
    }

    // AXIS INVERSION
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

    // KEYBINDING
    public void StartRebindingInteractKey()
    {
        if (!isRebindingKey)
        {
            StartCoroutine(RebindKeyRoutine());
        }
    }

    private IEnumerator RebindKeyRoutine()
    {
        isRebindingKey = true;
        if (interactKeyText != null) interactKeyText.text = "Press Any Key...";

        yield return null; // Wait 1 frame to prevent immediate trigger

        while (!Input.anyKeyDown)
        {
            yield return null;
        }

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

    public KeyCode GetInteractKey()
    {
        return interactKey;
    }

    // RESET SETTINGS TO DEFAULT
    public void ResetToDefaults()
    {
        // Cancel active rebinding if the player clicks reset during keybind prompt
        StopAllCoroutines();
        isRebindingKey = false;

        // Reset backing values and saved preferences
        SetSensitivity(DEFAULT_SENSITIVITY);
        SetInvertX(DEFAULT_INVERT_X);
        SetInvertY(DEFAULT_INVERT_Y);

        interactKey = DEFAULT_INTERACT_KEY;
        PlayerPrefs.SetString("InteractKey", DEFAULT_INTERACT_KEY.ToString());

        // Refresh UI elements
        if (sensitivitySlider != null) sensitivitySlider.value = DEFAULT_SENSITIVITY;
        if (invertXToggle != null) invertXToggle.isOn = DEFAULT_INVERT_X;
        if (invertYToggle != null) invertYToggle.isOn = DEFAULT_INVERT_Y;
        if (interactKeyText != null) interactKeyText.text = DEFAULT_INTERACT_KEY.ToString();

        PlayerPrefs.Save();
    }

    // LOAD SAVED SETTINGS
    private void LoadSettings()
    {
        Sensitivity = PlayerPrefs.GetFloat("MouseSensitivity", DEFAULT_SENSITIVITY);
        InvertX = PlayerPrefs.GetInt("InvertX", DEFAULT_INVERT_X ? 1 : 0) == 1;
        InvertY = PlayerPrefs.GetInt("InvertY", DEFAULT_INVERT_Y ? 1 : 0) == 1;

        string savedKey = PlayerPrefs.GetString("InteractKey", DEFAULT_INTERACT_KEY.ToString());
        Enum.TryParse(savedKey, out interactKey);

        // Apply loaded values to UI controls
        if (sensitivitySlider != null) sensitivitySlider.value = Sensitivity;
        if (invertXToggle != null) invertXToggle.isOn = InvertX;
        if (invertYToggle != null) invertYToggle.isOn = InvertY;
        if (interactKeyText != null) interactKeyText.text = interactKey.ToString();
    }
}