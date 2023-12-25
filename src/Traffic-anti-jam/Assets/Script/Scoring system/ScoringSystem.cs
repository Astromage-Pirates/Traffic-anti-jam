using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using AstroPirate.DesignPatterns;
using Cysharp.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;

public class ScoringSystem : MonoBehaviour
{
    [SerializeField]
    private EcoSystem ecoSystem;

    [SerializeField]
    private TrafficEfficiency trafficEfficiency;

    [SerializeField]
    private LevelData levelData;

    private IEventBus eventBus;

    private void Awake()
    {
        GlobalServiceContainer.Resolve<IEventBus>(out eventBus);
        eventBus.Register<PlayStageEnded>(OnPlayStageEnded);
    }

    private void OnPlayStageEnded(PlayStageEnded playStageEnded)
    {
        ScoreCaluculated();
    }

    private int ScoreCaluculated()
    {
        int score = levelData.currScore = 0;

        if (trafficEfficiency.EfficiencyStatus != EfficiencyStatus.Bad)
        {
            if (trafficEfficiency.EfficiencyStatus == EfficiencyStatus.Medium)
            {
                score += 1;
            }
            else if (trafficEfficiency.EfficiencyStatus == EfficiencyStatus.Good)
            {
                score += 2;
            }

            if (ecoSystem.CheckUseBudgetWinning() == true)
            {
                score += 1;
            }
        }

        levelData.highestScore = Mathf.Max(levelData.highestScore, score);

        return levelData.currScore = score;
    }

    private void OnDestroy()
    {
        eventBus.UnRegister<PlayStageEnded>(OnPlayStageEnded);
    }
}
