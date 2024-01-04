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

    [SerializeField]
    private Image img_bg;

    private LevelData data;

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
        SceneManager.LoadScene(data.name);
    }

    public void Init(LevelData data)
    {
        this.data = data;
        int stars = PlayerPrefs.GetInt($"{data.name}.highScore",0);
        for (int i = 0; i < stars; i++)
        {
            this.stars[i].isOn = true;
        }
        for (int i = stars; i < this.stars.Count; i++)
        {
            this.stars[i].isOn = false;
        }
        img_bg.sprite = data.image;
    }
}
