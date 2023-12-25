using AstroPirate.DesignPatterns;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The level manager.
/// </summary>
public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private Button btn_Play;

    private IEventBus eventBus;

    private void Awake()
    {
        GlobalServiceContainer.Resolve(out eventBus);
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
        eventBus.Send(new LevelStateChanged { IsPlay = true });
    }
}
