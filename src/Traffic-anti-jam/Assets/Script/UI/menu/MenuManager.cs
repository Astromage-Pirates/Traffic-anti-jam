using UnityEngine;

public class MenuManager : IMenuManager
{
    /// <inheritdoc/>
    public void OpenMenu(GameObject toMenu, GameObject fromMenu)
    {
        fromMenu?.gameObject.SetActive(false);
        toMenu?.gameObject.SetActive(true);
    }

    /// <inheritdoc/>
    public void OpenMenu(GameObject toMenu)
    {
        OpenMenu(toMenu, null);
    }

    /// <inheritdoc/>
    public void ExitMenu(GameObject fromMenu)
    {
        OpenMenu(null, fromMenu);
    }
}
