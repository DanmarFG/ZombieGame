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
        var burgerList = (Burger[])GetData("Burgers");

        if(burgerList.Length <= 0)
        {
            state = NodeState.FALIURE;
            return state;
        }

        var targetBurger = burgerList[0];

        foreach(var burger in burgerList)
        {
            if(Vector3.Distance(kim.transform.position, targetBurger.transform.position) > Vector3.Distance(kim.transform.position, burger.transform.position))
            {
                targetBurger = burger;
            }
        }

        var target = targetBurger.transform;

        kim.SetWalkBuffer(kim.GetTileListFromNode(Astar.GetPath(Grid.Instance.GetClosest(kim.transform.position), Grid.Instance.GetClosest(target.position))));

        state = NodeState.RUNNING;
        return state;
    }


}
