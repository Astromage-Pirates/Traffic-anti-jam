using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The button to pause game and open <see cref="PauseMenu"/>.
/// </summary>
public class PauseButton : MonoBehaviour
{
    [SerializeField]
    private Button btn_Pause;

    [SerializeField]
    private GameObject grp_PauseMenu;

    private IMenuManager menuManager;

    private void Awake()
    {
        GlobalServiceContainer.Resolve(out menuManager);
    }

    private void OnEnable()
    {
        btn_Pause.onClick.AddListener(OnBtnPauseClicked);
    }

    private void OnDisable()
    {
        btn_Pause.onClick.RemoveListener(OnBtnPauseClicked);
    }

    private void OnBtnPauseClicked()
    {
        menuManager.OpenMenu(grp_PauseMenu);
    }
}
