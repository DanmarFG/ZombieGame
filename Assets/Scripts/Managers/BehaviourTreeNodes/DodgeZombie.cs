using BehaviourTree;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
public class DodgeZombie : Node
{
    Kim kim;

    public DodgeZombie(Kim kim)
    {
        this.kim = kim;
    }

    public override NodeState Evaluate()
    {
        kim.SetWalkBuffer(kim.GetTileListFromNode(Astar.GetPath(Grid.Instance.GetClosest(kim.transform.position), SearchForSafe())));

        state = NodeState.SUCCESS;
        return state;
    }

    Grid.Tile SearchForSafe()
    {
        Grid.Tile tile = new Grid.Tile { x = 0, y = 0 };

        //for (int x = 3; x >= -3; x--)
        //{
        //    for (int y = -3; y <= 3; y++)
        //    {
        //        tile = new Grid.Tile
        //        {
        //            x = kim.GetCurrentTile().x - x,
        //            y = kim.GetCurrentTile().y - y,
        //        };

        //        tile = Grid.Instance.TryGetTile(new Vector2Int(tile.x, tile.y));

        //        if (tile == null)
        //            continue;

        //        if(!tile.occupied || !tile.innerZombie)
        //        {
        //            return tile;
        //        }
        //    }
        //}

        return tile;
    }
}
