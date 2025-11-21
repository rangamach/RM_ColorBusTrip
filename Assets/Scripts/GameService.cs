using UnityEngine;
using UnityEngine.UI;

public class GameService : GenericMonoSingleton<GameService>
{
    public GameplayService GameplayService { get; private set; }
    public EventService EventService { get; private set; }
    [SerializeField] private UI ui;
    public UI UI => this.ui;
    

    [SerializeField] private GameplayView gameplayView;
        
    [SerializeField] private TruckView[] trucks;

    [SerializeField] private Image Tint;

    private void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        CreateServices();
    }
    private void CreateServices()
    {
        EventService = new EventService();
        GameplayService = new GameplayService(gameplayView, trucks,Tint);
    }

}
