using UnityEngine;

public interface IItem
{
    int Damage { get; set; }
    Sprite sprite { get; set; }

    int usesMana { get; set; }

    string itemName { get; set; }

    void Action(Vector2 directionFacing);
}
