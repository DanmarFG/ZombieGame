using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class FindBurgers : Node
{
    public override NodeState Evaluate()
    {
        var burgerList = new List<Burger>();

        if(GamesManager.Instance.GetBurgerCount() > 0)
        {
            parent.parent.SetData("Burgers", GameObject.FindObjectsOfType<Burger>());

            if (GetData("Burgers") != null)
            {
                state = NodeState.SUCCESS;
                return state;
            }
        }
        state = NodeState.FALIURE;
        return state;
    }
}
