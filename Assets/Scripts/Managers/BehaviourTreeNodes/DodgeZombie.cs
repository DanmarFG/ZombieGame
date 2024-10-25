using BehaviourTree;
public class DodgeZombie : Node
{
    Kim kim;

    public DodgeZombie(Kim kim)
    {
        this.kim = kim;
    }

    public override NodeState Evaluate()
    {
        kim.SetWalkBuffer(Astar.GetTileListFromNode(Astar.GetPath(kim.GetCurrentTile(), SearchForSafe())));

        state = NodeState.SUCCESS;
        return state;
    }

    Grid.Tile SearchForSafe()
    {
        Grid.Tile tile = new Grid.Tile { x = 0, y = 0 };

        return tile;
    }
}
