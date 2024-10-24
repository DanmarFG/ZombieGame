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
        var burgerList = new List<Burger>();

        if(GamesManager.Instance.GetBurgerCount() > 0)
        {
            //parent.parent.SetData("Burger", );

            burgerList.AddRange(GameObject.FindObjectsOfType<Burger>());

            if (burgerList.Count <= 0)
            {
                state = NodeState.FALIURE;
                return state;
            }

            var targetBurger = burgerList[0];

            foreach (var burger in burgerList)
            {
                if (Vector3.Distance(kim.transform.position, targetBurger.transform.position) > Vector3.Distance(kim.transform.position, burger.transform.position))
                {
                    targetBurger = burger;
                }
            }

            parent.parent.SetData("Burger", targetBurger);


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
