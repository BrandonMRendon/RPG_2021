using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerContainer : MonoBehaviour
{
    protected static GameObject ColliderTilesLevelOneOnly, ColliderTilesLevelTwoOnly;
    public const string LAYER_One = "Player";
    public const string LAYER_Two = "PlayerUpper";
    private void Start()
    {
        ColliderTilesLevelOneOnly = GameObject.FindGameObjectWithTag("LayerFloorOneOnly");
        ColliderTilesLevelTwoOnly = GameObject.FindGameObjectWithTag("ColliderProjectile");
    }
}
