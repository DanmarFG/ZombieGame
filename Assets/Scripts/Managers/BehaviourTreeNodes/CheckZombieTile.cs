using BehaviourTree;
using UnityEngine;

public class CheckZombieTile : Node
{
    Kim kim;
    public CheckZombieTile(Kim kim)
    {
        this.kim = kim;
    }

    public override NodeState Evaluate()
    {

        if (Grid.Instance.GetClosest(kim.GetCurrentTile()).innerZombie)
        {
            state = NodeState.SUCCESS;
            return state;
        }
        else
        {
            parent.parent.SetData("RunningToSafety", false);
            state = NodeState.FALIURE;
            return state;
        }
        
    }
}
