using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class GameService : MonoBehaviour
{
    private GameplayService gameplayService;

    [SerializeField] private PlayerView RedTruck;
    [SerializeField] private PlayerView YellowTruck;
    [SerializeField] private PlayerView BlueTruck;
    [SerializeField] private GameplayView View;

    [SerializeField] private GameplaySO SO;

    private void Start()
    {
        CreateServices();
    }
    private void CreateServices()
    {
        gameplayService = new GameplayService(SO,RedTruck,YellowTruck,BlueTruck);
    }

}
