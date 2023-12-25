using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelWidget : MonoBehaviour
{
    [SerializeField]
    List<Toggle> stars;

    [SerializeField]
    private Button btn_Play;

    private void OnEnable()
    {
        btn_Play.onClick.AddListener(OnBtnPlayClicked);
    }

    private void OnDisable()
    {
        btn_Play.onClick.RemoveListener(OnBtnPlayClicked);
    }

    private void OnBtnPlayClicked()
    {
        SceneManager.LoadScene("VehicelTestScene");
    }

    public void Init(int stars, bool unlocked)
    {
        for (int i = 0; i < stars; i++)
        {
            this.stars[i].isOn = true;
        }
        for (int i = stars; i < this.stars.Count; i++)
        {
            this.stars[i].isOn = false;
        }
    }
}
