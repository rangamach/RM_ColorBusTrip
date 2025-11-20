using System.Collections.Generic;
using UnityEngine;

public class GameplayService
{
    private GameplayController controller;
    public GameplayService(GameplaySO so, TruckView Red,TruckView Yellow, TruckView Blue)
    {
        this.controller = new GameplayController(so.View, Red, Yellow, Blue,so.Positions);
    }
}
