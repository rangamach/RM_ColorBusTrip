using System.Collections.Generic;
using UnityEngine;

public class GameplayController
{
    private GameplayView view;
    private List<Vector3> Positions;

    private PlayerView Red;
    private PlayerView Yellow;
    private PlayerView Blue;

    public GameplayController(GameplayView view, PlayerView red, PlayerView yellow, PlayerView blue, List<Vector3> positions)
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
