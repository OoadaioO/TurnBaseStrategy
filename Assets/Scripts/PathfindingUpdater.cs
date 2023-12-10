using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingUpdater : MonoBehaviour
{

    private void Start() {
        DestructibleCrate.OnAnyDestoryed += DestructibleCrate_OnAnyDestoryed; 
    }

    private void DestructibleCrate_OnAnyDestoryed(object sender, EventArgs e) {
        DestructibleCrate destructibleCrate = sender as DestructibleCrate;
        Pathfinding.Instance.SetIsWalkableGridPosition(destructibleCrate.GetGridPosition(),true);
    }
}
