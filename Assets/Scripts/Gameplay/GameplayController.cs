using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayController
{
    private GameplayView view;
    private GameplayModel model;

    public GameplayController(GameplayView view, TruckView[] trucks,Image tint)
    {
        model = new GameplayModel();

        this.view = Object.Instantiate(view);

        this.view.SetController(this);
        this.view.SetTruckArray(trucks);
        this.view.SetTint(tint);
    }
    public void SetCurrentCoin(int coin) => model.SetCoin(coin);
    public int GetCurrentCoin() => model.CurrentCoins;
}