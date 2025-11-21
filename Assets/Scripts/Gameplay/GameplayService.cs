using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayService
{
    private GameplayController controller;
    public GameplayService(GameplayView view, TruckView Red,TruckView Yellow, TruckView Blue,Image Tint)
    {
        this.controller = new GameplayController(view, Red, Yellow, Blue,Tint);
    }
}
