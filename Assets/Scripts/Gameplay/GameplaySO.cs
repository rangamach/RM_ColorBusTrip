using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameplaySO", menuName = "Scriptable Objects/GameplaySO")]
public class GameplaySO : ScriptableObject
{
    public GameplayView View;
    public List<Vector3> Positions = new List<Vector3>();
}
