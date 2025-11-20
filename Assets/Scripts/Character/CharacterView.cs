using UnityEngine;

public class CharacterView : MonoBehaviour
{
    [SerializeField] private Colors Color;
    public Colors GetColor() => this.Color;
}
