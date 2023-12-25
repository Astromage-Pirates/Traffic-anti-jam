using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Animator animator;


    [SerializeField] LevelWidget levelWidgetPrefab;
    [SerializeField] GridLayoutGroup levelSelectGrid;

	private void Start()
	{
        LoadLevelSelect();
		animator.SetTrigger("MainMenu");
	}
	public void LoadLevelSelect()
    {
        for(int i =0; i < 6; i++)
        {
            var go = Instantiate<LevelWidget>(levelWidgetPrefab,levelSelectGrid.transform);
            go.Init(Random.Range(0,3),true);
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
