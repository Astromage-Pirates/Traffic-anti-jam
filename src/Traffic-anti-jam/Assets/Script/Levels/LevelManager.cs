using System;
using System.Collections.Generic;
using AstroPirate.DesignPatterns;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The level manager.
/// </summary>
public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private Button btn_Play;

    [SerializeField]
    private ScrollRect scrollView_Toolbar;

    [SerializeField]
    private AudioSource ambientSoundAudioSource;

    private IEventBus eventBus;

    [SerializeField]
    private LevelData levelData;

    public LevelData LevelData => levelData;

    [SerializeField]
    private GameObject overCanvas;
    
    [SerializeField]
    private List<Toggle> stars;

    private void Awake()
    {
        overCanvas.SetActive(false);
        GlobalServiceContainer.Resolve(out eventBus);
        eventBus.Register<PlayStageEnded>(OnShowOverCanvas);
    }

    private void OnEnable()
    {
        btn_Play.onClick.AddListener(OnBtnPlayPressed);
    }

    private void OnDisable()
    {
        btn_Play.onClick.RemoveListener(OnBtnPlayPressed);
    }

    private void OnBtnPlayPressed()
    {
        ambientSoundAudioSource.Play();
        btn_Play.interactable = false;
        scrollView_Toolbar.gameObject.SetActive(false);
        eventBus.Send(new LevelStateChanged { IsPlay = true });
        StartPlayStageEnded().Forget();
    }

    private async UniTaskVoid StartPlayStageEnded()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(levelData.runTimeStage));
        eventBus.Send(new PlayStageEnded());
    }

    private void OnShowOverCanvas(PlayStageEnded playStageEnded)
    {
		for (int i = 0; i < levelData.currScore; i++)
		{
			this.stars[i].isOn = true;
		}
		for (int i = levelData.currScore; i < this.stars.Count; i++)
		{
			this.stars[i].isOn = false;
		}
		overCanvas.SetActive(true);
    }

    private void OnDestroy()
    {
        eventBus.UnRegister<PlayStageEnded>(OnShowOverCanvas);
    }
}
