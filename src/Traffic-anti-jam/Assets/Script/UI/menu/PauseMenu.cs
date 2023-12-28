using System;
using AstroPirate.DesignPatterns;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// The pause menu.
/// </summary>
public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private Button btn_Close;

    [SerializeField]
    private Button btn_Exit;

    [SerializeField]
    private Button btn_Restart;

    [Header("Main Menu")]
    [SerializeField]
    private GameObject grp_MainMenu;

    [Header("Settings")]
    [SerializeField]
    private Button btn_Settings;

    [SerializeField]
    private GameObject grp_Settings;

    private IMenuManager menuManager;

    private void Awake()
    {
        GlobalServiceContainer.Resolve(out menuManager);
    }

    private void OnEnable()
    {
        btn_Settings.onClick.AddListener(OnBtnSettingsClicked);
        btn_Restart.onClick.AddListener(OnBtnRestartClicked);
        btn_Exit.onClick.AddListener(OnBtnExitClicked);
        btn_Close.onClick.AddListener(OnBtnCloseClicked);
    }

    private void OnDisable()
    {
        btn_Settings.onClick.RemoveListener(OnBtnSettingsClicked);
        btn_Restart.onClick.RemoveListener(OnBtnRestartClicked);
        btn_Exit.onClick.RemoveListener(OnBtnExitClicked);
        btn_Close.onClick.RemoveListener(OnBtnCloseClicked);
    }

    private void OnBtnSettingsClicked()
    {
        menuManager.OpenMenu(grp_Settings, grp_MainMenu);
    }

    private void OnBtnCloseClicked()
    {
        menuManager.ExitMenu(grp_MainMenu);
    }

    private void OnBtnRestartClicked()
    {
        var activeScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(activeScene.buildIndex);
    }

    private void OnBtnExitClicked()
    {
        SceneManager.LoadScene(0);
    }
}
