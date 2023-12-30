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
        btn_Close.onClick.AddListener(OnBtnCloseClicked);
    }

    private void OnDisable()
    {
        btn_Settings.onClick.RemoveListener(OnBtnSettingsClicked);
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
}
