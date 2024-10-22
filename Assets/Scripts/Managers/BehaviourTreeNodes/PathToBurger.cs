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
        Burger burger = (Burger)GetData("Burger");
        Transform target = burger.transform;
        kim.SetWalkBuffer(kim.GetTileListFromNode(Astar.GetPath(Grid.Instance.GetClosest(kim.transform.position), Grid.Instance.GetClosest(target.position))));
        state = NodeState.RUNNING;
        return state;
    }


}
