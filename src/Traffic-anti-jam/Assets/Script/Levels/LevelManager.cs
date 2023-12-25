using System;
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

    [SerializeField]
    private GameObject overCanvas;

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
        overCanvas.SetActive(true);
        Debug.Log("aaa");
    }

    private void OnDestroy()
    {
        eventBus.UnRegister<PlayStageEnded>(OnShowOverCanvas);
    }
}
