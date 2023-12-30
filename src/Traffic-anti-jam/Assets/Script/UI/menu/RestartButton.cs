using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// The button to restart the current <see cref="Scene"/> .
/// </summary>
public class RestartButton : MonoBehaviour
{
    [SerializeField]
    private Button btn_Restart;

    private void OnEnable()
    {
        btn_Restart.onClick.AddListener(OnBtnRestartClicked);
    }

    private void OnDisable()
    {
        btn_Restart.onClick.RemoveListener(OnBtnRestartClicked);
    }

    private void OnBtnRestartClicked()
    {
        var activeScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(activeScene.buildIndex);
    }
}
