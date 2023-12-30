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
    private LevelManager levelManager;

    private void OnEnable()
    {
        btn_LoadScene.onClick.AddListener(OnBtnLoadSceneClicked);
        if(levelManager.LevelData.currScore == 0)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        btn_LoadScene.onClick.RemoveListener(OnBtnLoadSceneClicked);
    }

    private void OnBtnLoadSceneClicked()
    {
        if(levelManager == null)
        {
			SceneManager.LoadScene("MainMenu");
		}
        if (levelManager.NextLevelData == null) return;
        SceneManager.LoadScene(levelManager.NextLevelData.name);
    }
}
