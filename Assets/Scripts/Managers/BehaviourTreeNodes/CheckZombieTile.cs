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

        if (Grid.Instance.TryGetTile(new Vector2Int(kim.GetCurrentTile().x, kim.GetCurrentTile().y)).innerZombie)
        {
            state = NodeState.SUCCESS;
            return state;
        }
        state = NodeState.FALIURE;
        return state;

    }
}
