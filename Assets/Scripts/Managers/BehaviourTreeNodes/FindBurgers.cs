using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class FindBurgers : Node
{
    Transform kimPosition;

    public FindBurgers(Transform transform) 
    {
        kimPosition = transform;
    }

    public override NodeState Evaluate()
    {
        object burger = GetData("Burger");
        if(burger != null)
        {
            state = NodeState.SUCCESS;
            return state;
        }

        parent.parent.SetData("Burger", GameObject.FindObjectOfType<Burger>().transform);

        if (GetData("Burger") != null)
        {
            state = NodeState.SUCCESS;
            return state;
        }
        state = NodeState.FALIURE;
        return state;
    }
}
