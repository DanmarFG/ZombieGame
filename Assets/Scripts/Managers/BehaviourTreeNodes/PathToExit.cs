using BehaviourTree;
using System;
using System.Diagnostics;

public class PathToExit : Node
{
    Kim kim;
    public PathToExit(Kim kim)
    {
        this.kim = kim;
    }

    public override NodeState Evaluate()
    {
        Console.WriteLine("pathing to exit");
        kim.SetWalkBuffer(kim.GetTileListFromNode(Astar.GetPath(Grid.Instance.GetClosest(kim.transform.position), Grid.Instance.GetFinishTile())));
        state = NodeState.RUNNING;
        return state;
    }
}
