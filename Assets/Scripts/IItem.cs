using UnityEngine;

public interface IItem
{
    int Damage { get; set; }
    Sprite sprite { get; set; }

    int usesMana { get; set; }

    int stack { get; set; }
    int stackHolding { get; set; }
    string itemName { get; set; }

    bool isActivelyUsed { get; set; }

    void Action(Vector2 directionFacing);
}
