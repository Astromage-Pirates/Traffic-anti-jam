using System.Collections;
using System.Collections.Generic;
using AstroPirate.DesignPatterns;
using TMPro;
using UnityEngine;

public class EcoSystem : MonoBehaviour
{
    private int totalBudget;

    [SerializeField]
    private int useBudget = 0;

    [SerializeField]
    private TextMeshProUGUI txt_totalBudget;

    [SerializeField]
    private TextMeshProUGUI txt_useBudget;

    [SerializeField]
    private AudioClip cashClip;

    private IEventBus eventBus;

    [SerializeField]
    private LevelData levelData;

    private void Awake()
    {
        totalBudget = levelData.totalBudget;
        GlobalServiceContainer.Resolve<IEventBus>(out eventBus);
        eventBus.Register<BudgetCost>(OnBudgetCost);
        txt_totalBudget.text = totalBudget + "$";
        txt_useBudget.text = useBudget + "$";
    }

    private void OnBudgetCost(BudgetCost budgetCost)
    {
        useBudget += budgetCost.intSign * budgetCost.trafficTool.cost;
        txt_useBudget.text = useBudget + "$";
        eventBus.Send(new AudioPlayed() { audioClip = cashClip });

        if (useBudget > totalBudget)
        {
            txt_useBudget.color = Color.red;
        }
        else if (useBudget < totalBudget)
        {
            txt_useBudget.color = Color.white;
        }
    }

    public bool CheckUseBudgetWinning()
    {
        if (useBudget <= totalBudget)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void OnDestroy()
    {
        eventBus.UnRegister<BudgetCost>(OnBudgetCost);
    }
}
