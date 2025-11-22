public class GameplayModel
{
    public int CurrentCoins { get; private set; }

    public GameplayModel()
    {
        SetCoin(0);
    }

    public void SetCoin(int coins) => this.CurrentCoins = coins;
}
