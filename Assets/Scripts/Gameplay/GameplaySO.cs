using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameplaySO", menuName = "Scriptable Objects/GameplaySO")]
public class GameplaySO : ScriptableObject
{
    public List<Vector3> Positions = new List<Vector3>();
    public Vector3 FinalPosition;
}
