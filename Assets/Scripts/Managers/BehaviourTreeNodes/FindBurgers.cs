using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class FindBurgers : Node
{
    Kim kim;

    public FindBurgers(Kim kim) 
    {
        this.kim = kim;
    }

    public override NodeState Evaluate()
    {
        List<Burger> burgerList = new List<Burger>();
        burgerList.AddRange(GameObject.FindObjectsOfType<Burger>());

        if(burgerList.Count > 0)
        {
            parent.parent.SetData("Burger", burgerList[0]);

            if (GetData("Burger") != null)
            {
                state = NodeState.SUCCESS;
                return state;
            }

            state = NodeState.FALIURE;
            return state;
        }
        state = NodeState.FALIURE;
        return state;
    }
}
