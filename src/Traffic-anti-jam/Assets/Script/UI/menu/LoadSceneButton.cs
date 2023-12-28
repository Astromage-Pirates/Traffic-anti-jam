using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// The <see cref="Button"/> when click will load specified <see cref="Scene"/>.
/// </summary>
public class LoadSceneButton : MonoBehaviour
{
    [SerializeField]
    private Button btn_LoadScene;

    [SerializeField]
    private string sceneName;

    private void OnEnable()
    {
        btn_LoadScene.onClick.AddListener(OnBtnLoadSceneClicked);
    }

    private void OnDisable()
    {
        btn_LoadScene.onClick.RemoveListener(OnBtnLoadSceneClicked);
    }

    private void OnBtnLoadSceneClicked()
    {
        SceneManager.LoadScene(sceneName);
    }
}
