using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Animator animator;
    [SerializeField] List<LevelData> levelData;


    [SerializeField] LevelWidget levelWidgetPrefab;
    [SerializeField] GridLayoutGroup levelSelectGrid;

	private void Start()
	{
        LoadLevelSelect();
		animator.SetTrigger("MainMenu");
	}
	public void LoadLevelSelect()
    {
        foreach(var data in levelData)
        {
            var go = Instantiate<LevelWidget>(levelWidgetPrefab,levelSelectGrid.transform);

            go.Init(data);
        }
    }

    public void Play()
    {
        animator.SetTrigger("MainMenu");
        animator.SetTrigger("LevelSelect");
	}

    public void Setting()
    {
		animator.SetTrigger("MainMenu");
		animator.SetTrigger("Setting");
	}

    public void BackFromPlay()
    {
        animator.SetTrigger("MainMenu");
        animator.SetTrigger("LevelSelect");
    }
    public void BackFromSetting()
    {
        animator.SetTrigger("MainMenu");
        animator.SetTrigger("Setting");
    }

    public void Exit()
    {

    }
}
