using System.Collections.Generic;
using UnityEngine;

public class GameplayService
{
    private GameplayController controller;
    public GameplayService(GameplayView view, TruckView Red,TruckView Yellow, TruckView Blue)
    {
        this.controller = new GameplayController(view, Red, Yellow, Blue);
    }
}
