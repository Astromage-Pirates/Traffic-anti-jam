using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
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
    private LevelData nextlevelData;

    public LevelData LevelData => levelData;
    public LevelData NextLevelData => nextlevelData;

    [SerializeField]
    private GameObject overCanvas;

    [SerializeField]
    private GameObject victoryOverCanvas;

    [SerializeField]
    private GameObject trafficJamOverCanvas;

    [SerializeField]
    private List<Toggle> stars;

    [SerializeField]
    private List<GameObject> medals;
    private CancellationTokenSource cts;

    /// <summary>
    /// List of <see cref="PathSystem"/>s of this level.
    /// </summary>
    [field: SerializeField]
    public PathSystem[] PathSystems { get; private set; }

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
        if (!PathSystems.Any(p => p.AvailablePaths.IsEmpty()))
        {
            ambientSoundAudioSource.Play();
            btn_Play.interactable = false;
            scrollView_Toolbar.gameObject.SetActive(false);
            eventBus.Send(new LevelStateChanged { IsPlay = true });
            StartPlayStageEnded().Forget();
        }
    }

    private async UniTaskVoid StartPlayStageEnded()
    {
        cts = new();
        await UniTask.Delay(
            TimeSpan.FromSeconds(levelData.runTimeStage),
            cancellationToken: cts.Token
        );
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

        for (int i = 0; i < medals.Count; i++)
        {
            medals[i].SetActive(i == levelData.currScore - 1);
        }

        victoryOverCanvas.SetActive(levelData.currScore > 0);
        trafficJamOverCanvas.SetActive(levelData.currScore == 0);
        overCanvas.SetActive(true);
    }

    private void OnDestroy()
    {
        eventBus.UnRegister<PlayStageEnded>(OnShowOverCanvas);
        if (cts is not null)
        {
            cts.Cancel();
        }
    }
}
