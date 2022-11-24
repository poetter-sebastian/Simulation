using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GUIMainMenu : MonoBehaviour
{
    public Canvas[] windows;
    public TMP_Dropdown resSel;
    public Slider fpsSlider;
    public TMP_Text fpsText;
    public TMP_Dropdown modeSel;
    public TMP_Dropdown graphSel;
    public TMP_Text loadingText;
    public AudioSource click;

    /// <summary>
    /// Current selected resolution index
    /// </summary>
    /// <see cref="resolutions"/>
    private int selResIndex = 2;

    /// <summary>
    /// Current refresh rate 
    /// </summary>
    private int fps = 60;
    
    /// <summary>
    /// Current selected window mode
    /// </summary>
    private int selModeIndex = 0;

    /// <summary>
    /// Possible window modes
    /// https://docs.unity3d.com/2021.3/Documentation/ScriptReference/FullScreenMode.html
    /// </summary>
    private readonly FullScreenMode[] windowMode = {
        FullScreenMode.FullScreenWindow,
        FullScreenMode.MaximizedWindow,
        FullScreenMode.Windowed,
    };

    /// <summary>
    /// Constant array of possible resolutions
    /// </summary>
    private readonly (int, int)[] resolutions = {
        (3840,2160),
        (2560,1440),
        (1920,1080),
        (1366,768),
        (1280,720),
        (3840,2400),
        (2560,1600),
        (1920,1200),
        (1680,1050),
        (1440,900),
        (1280,800),
        (1920,1200),
    };
    
    /// <summary>
    /// Window count for the active window in UI
    /// </summary>
    private int wc;

    public static void Exit()
    {
        Application.Quit();
    }
    
    public void SwitchToGerman()
    {
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[1];
    }
    
    public void SwitchToEnglish()
    {
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[0];
    }

    public void NextWindow()
    {
        windows[wc % windows.Length].gameObject.SetActive(false);
        wc++;
        windows[wc % windows.Length].gameObject.SetActive(true);
    }

    public void PreviousWindows()
    {
        windows[wc % windows.Length].gameObject.SetActive(false);
        wc--;
        windows[wc % windows.Length].gameObject.SetActive(true);
    }

    public void ChangeWindowMode()
    {
        selModeIndex = modeSel.value;
        SetWindow();
    }

    public void ChangeFPS()
    {
        fps = (int)fpsSlider.value;
        fpsText.text = fps.ToString();
        SetWindow();
    }
    
    public void ChangeResolution()
    {
        selResIndex = resSel.value;
        SetWindow();
    }

    public void ChangeQualityPreset()
    {
        QualitySettings.SetQualityLevel(graphSel.value, true);
    }

    public void StartLoading()
    {
        StartCoroutine(LoadSimulationAsync());
    }
    
    public void PlayClick()
    {
        click.Play();
    }
    
    private IEnumerator LoadSimulationAsync()
    {
        var asyncLoad = SceneManager.LoadSceneAsync("Simulation");
        
        while (!asyncLoad.isDone)
        {
            loadingText.text = asyncLoad.progress * 100 + "%";
            yield return null;
        }
    }
    
    private void SetWindow()
    {
        Screen.SetResolution(resolutions[selResIndex].Item1, resolutions[selResIndex].Item2,  windowMode[selModeIndex], fps);
    }
}
