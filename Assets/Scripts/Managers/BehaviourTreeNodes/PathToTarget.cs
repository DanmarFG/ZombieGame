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
        kim.SetWalkBuffer(Astar.GetTileListFromNode(Astar.GetPath(kim.GetCurrentTile(), target)));

        state = NodeState.SUCCESS;
        return state;
    }
}
