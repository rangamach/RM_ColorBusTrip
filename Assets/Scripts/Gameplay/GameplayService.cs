using System.Collections.Generic;
using UnityEngine;

public class GameplayService
{
    private GameplayController controller;
    public GameplayService(GameplaySO so, PlayerView Red,PlayerView Yellow, PlayerView Blue)
    {
        this.controller = new GameplayController(so.View, Red, Yellow, Blue,so.Positions);
    }
}
