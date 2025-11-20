using System.Collections.Generic;
using UnityEngine;

public class GameplayController
{
    private GameplayView view;
    private List<Vector3> Positions;

    private TruckView Red;
    private TruckView Yellow;
    private TruckView Blue;

    public GameplayController(GameplayView view, TruckView red, TruckView yellow, TruckView blue, List<Vector3> positions)
    {
        this.Red = red;
        this.Yellow = yellow;
        this.Blue = blue;
        this.Positions = positions;

        this.view = Object.Instantiate(view);

        this.view.SetController(this);
    }
    public Vector3 GetDestination(int index)
    {
        return Positions[index % Positions.Count];
    }
}
