using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Animator levelSelectAnimator;
    [SerializeField] Animator settingAnimator;
    [SerializeField] Animator mainMenuAnimator;


    [SerializeField] LevelWidget levelWidgetPrefab;
    [SerializeField] GridLayoutGroup levelSelectGrid;

	private void Start()
	{
        LoadLevelSelect();
		mainMenuAnimator.SetBool("show", true);
		settingAnimator.SetBool("show", false);
		levelSelectAnimator.SetBool("show", false);
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
        mainMenuAnimator.SetBool("show", false);
		settingAnimator.SetBool("show", false);
        levelSelectAnimator.SetBool("show", true);
	}

    public void Setting()
    {
		mainMenuAnimator.SetBool("show", false);
		settingAnimator.SetBool("show", true);
		levelSelectAnimator.SetBool("show", false);
	}

    public void Back()
    {
		mainMenuAnimator.SetBool("show", true);
		settingAnimator.SetBool("show", false);
		levelSelectAnimator.SetBool("show", false);
	}

    public void Exit()
    {

    }
}
