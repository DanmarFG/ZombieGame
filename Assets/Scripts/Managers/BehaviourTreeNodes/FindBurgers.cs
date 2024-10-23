using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class FindBurgers : Node
{
    public override NodeState Evaluate()
    {
        var burgerList = new List<Burger>();
        burgerList.AddRange(Object.FindObjectsOfType<Burger>());

        if(burgerList.Count > 0)
        {
            parent.parent.SetData("Burger", burgerList[0]);

            if (GetData("Burger") != null)
            {
                state = NodeState.SUCCESS;
                return state;
            }
        }
        state = NodeState.FALIURE;
        return state;
    }
}
