using BehaviourTree;

public class PathToBurger : Node
{
    Kim kim;
    public PathToBurger(Kim kim)
    {
        this.kim = kim;
    }

    public override NodeState Evaluate()
    {
        var target = (Grid.Tile)GetData("target");

        kim.SetWalkBuffer(Astar.GetTileListFromNode(Astar.GetPath(kim.GetCurrentTile(), target)));

        state = NodeState.SUCCESS;
        return state;
    }


}
