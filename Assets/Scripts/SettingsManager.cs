using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance { get; private set; }

    [Header("Mouse Controls")]
    [SerializeField] private Slider sensitivitySlider;
    [SerializeField] private Toggle invertXToggle;
    [SerializeField] private Toggle invertYToggle;

    [Header("Keybinding UI")]
    [SerializeField] private TMP_Text interactKeyText;
    private KeyCode interactKey = KeyCode.E;
    private bool isRebindingKey = false;

    // Public properties to read from player movement / camera scripts
    public float Sensitivity { get; private set; } = 2f;
    public bool InvertX { get; private set; } = false;
    public bool InvertY { get; private set; } = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        LoadSettings();
    }

    // --- MOUSE SENSITIVITY ---
    public void SetSensitivity(float value)
    {
        Sensitivity = value;
        PlayerPrefs.SetFloat("MouseSensitivity", Sensitivity);
    }

    // --- AXIS INVERSION ---
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

    // --- KEYBINDING ---
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

    // --- LOAD SAVED SETTINGS ---
    private void LoadSettings()
    {
        Sensitivity = PlayerPrefs.GetFloat("MouseSensitivity", 2f);
        InvertX = PlayerPrefs.GetInt("InvertX", 0) == 1;
        InvertY = PlayerPrefs.GetInt("InvertY", 0) == 1;

        string savedKey = PlayerPrefs.GetString("InteractKey", "E");
        Enum.TryParse(savedKey, out interactKey);

        // Apply loaded values to UI controls
        if (sensitivitySlider != null) sensitivitySlider.value = Sensitivity;
        if (invertXToggle != null) invertXToggle.isOn = InvertX;
        if (invertYToggle != null) invertYToggle.isOn = InvertY;
        if (interactKeyText != null) interactKeyText.text = interactKey.ToString();
    }
}