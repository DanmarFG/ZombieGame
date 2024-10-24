using BehaviourTree;

public class PathToTarget : Node
{
    Grid.Tile target;
    Kim kim;
    public PathToTarget(Kim kim, Grid.Tile target)
    {
        this.kim = kim;
        this.target = target;
    }

    public override NodeState Evaluate()
    {
        kim.SetWalkBuffer(kim.GetTileListFromNode(Astar.GetPath(Grid.Instance.GetClosest(kim.transform.position), target)));

        state = NodeState.SUCCESS;
        return state;
    }
}
