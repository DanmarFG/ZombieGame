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
        var target = (Burger)GetData("Burger");

        if (!target || Grid.Instance.GetClosest(target.transform.position).innerZombie)
        {
            kim.SetWalkBuffer(new List<Grid.Tile>());
            state = NodeState.SUCCESS;
            return state;
        }

        kim.SetWalkBuffer(kim.GetTileListFromNode(Astar.GetPath(Grid.Instance.GetClosest(kim.transform.position), Grid.Instance.GetClosest(target.transform.position))));

        state = NodeState.SUCCESS;
        return state;
    }


}
