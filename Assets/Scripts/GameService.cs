using UnityEngine;

public class GameService : MonoBehaviour
{
    private GameplayService gameplayService;

    [SerializeField] private TruckView RedTruck;
    [SerializeField] private TruckView YellowTruck;
    [SerializeField] private TruckView BlueTruck;

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
