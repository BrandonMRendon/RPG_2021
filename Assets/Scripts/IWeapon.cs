using UnityEngine;

public interface IItem
{
    int Damage { get; set; }

    void Action(Vector2 directionFacing);
}
