using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class PathToBurger : Node
{
    Kim kim;
    public PathToBurger(Kim kim)
    {
        this.kim = kim;
    }

    public override NodeState Evaluate()
    {
        var burger = (Burger)GetData("Burger");
        var target = burger.transform;
        if (Grid.Instance.GetClosest(burger.transform.position).innerZombie)
        {
            kim.SetWalkBuffer(new List<Grid.Tile>());
        }
        else
        {
            kim.SetWalkBuffer(kim.GetTileListFromNode(Astar.GetPath(Grid.Instance.GetClosest(kim.transform.position), Grid.Instance.GetClosest(target.position))));
        }

        state = NodeState.SUCCESS;
        return state;
    }


}
