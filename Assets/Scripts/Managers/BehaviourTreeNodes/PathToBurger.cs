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
        Transform target = (Transform)GetData("Burger");
        kim.SetWalkBuffer(kim.GetTileListFromNode(Astar.GetPath(Grid.Instance.GetClosest(kim.transform.position), Grid.Instance.GetClosest(target.position))));
        state = NodeState.RUNNING;
        return state;
    }


}
