using BehaviourTree;

public class PathToExit : Node
{
    Kim kim;
    public PathToExit(Kim kim)
    {
        this.kim = kim;
    }

    public override NodeState Evaluate()
    {
        kim.SetWalkBuffer(kim.GetTileListFromNode(Astar.GetPath(Grid.Instance.GetClosest(kim.transform.position), Grid.Instance.GetFinishTile())));
        state = NodeState.RUNNING;
        return state;
    }
}
