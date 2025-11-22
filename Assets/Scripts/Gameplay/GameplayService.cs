using UnityEngine.UI;

public class GameplayService
{
    private GameplayController controller;
    public GameplayService(GameplayView view, TruckView[] trucks,Image Tint)
    {
        this.controller = new GameplayController(view, trucks,Tint);
    }

    public int GetCurrentCoin() => controller.GetCurrentCoin();
    public void SetCurrentCoin(int coin) => controller.SetCurrentCoin(coin);
}
