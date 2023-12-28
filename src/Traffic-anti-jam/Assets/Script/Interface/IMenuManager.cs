using UnityEngine;

interface IMenuManager
{
    /// <summary>
    /// Change menu UI to specified menu and disable the opened one.
    /// </summary>
    /// <param name="toMenu">The menu to be opened.</param>
    /// <param name="fromMenu">The menu that is called from.</param>
    public void OpenMenu(GameObject toMenu, GameObject fromMenu);

    /// <summary>
    /// Change menu UI to specified menu.
    /// </summary>
    /// <param name="toMenu">The menu to be opened.</param>
    public void OpenMenu(GameObject toMenu);

    /// <summary>
    /// Exit specified menu.
    /// </summary>
    /// <param name="toMenu">The menu to be opened.</param>
    public void ExitMenu(GameObject fromMenu);
}
