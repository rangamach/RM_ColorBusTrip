using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayController
{
    private GameplayView view;

    //private TruckView Red;
    //private TruckView Yellow;
    //private TruckView Blue;

   // public GameplayController(GameplayView view, TruckView red, TruckView yellow, TruckView blue,Image tint)
    public GameplayController(GameplayView view, TruckView[] trucks,Image tint)
    {
        //this.Red = red;
        //this.Yellow = yellow;
        //this.Blue = blue;

        this.view = Object.Instantiate(view);

        this.view.SetController(this);
        this.view.SetTruckArray(trucks);
        this.view.SetTint(tint);
    }
}