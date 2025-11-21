using UnityEngine;
using UnityEngine.UI;

public class GameService : MonoBehaviour
{
    private GameplayService gameplayService;

    [SerializeField] private GameplayView gameplayView;
    [SerializeField] private TruckView RedTruck;
    [SerializeField] private TruckView YellowTruck;
    [SerializeField] private TruckView BlueTruck;

    [SerializeField] private Image Tint;

    private void Start()
    {
        CreateServices();
    }
    private void Update()
    {
        if (!RedTruck.gameObject.activeInHierarchy && !BlueTruck.gameObject.activeInHierarchy && !YellowTruck.gameObject.activeInHierarchy)
        {
            Debug.Log("You Won!!!");
            //won.gameObject.SetActive(true);
        }
    }
    private void CreateServices()
    {
        gameplayService = new GameplayService(gameplayView,RedTruck,YellowTruck,BlueTruck,Tint);
    }

}
