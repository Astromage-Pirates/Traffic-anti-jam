using System;
using UnityEngine;
using UnityEngine.UI;

public class SettingMenu : MonoBehaviour
{
    [SerializeField]
    private Button btn_Close;

    [SerializeField]
    private GameObject grp_MainMenu;

    private IMenuManager menuManager;

    private void Awake()
    {
        GlobalServiceContainer.Resolve(out menuManager);
    }

    private void OnEnable()
    {
        btn_Close.onClick.AddListener(OnBtnCloseClicked);
    }

    private void OnDisable()
    {
        btn_Close.onClick.RemoveListener(OnBtnCloseClicked);
    }

    private void OnBtnCloseClicked()
    {
        menuManager.OpenMenu(grp_MainMenu, gameObject);
    }
}
