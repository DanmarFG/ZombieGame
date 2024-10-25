using System.Collections.Generic;
using BehaviourTree;
using UnityEngine;

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

        List<Grid.Tile> tileList = new List<Grid.Tile>();

        for (int x = -3; x <= 3; x++)
        {
            for (int y = -3; y <= 3; y++)
            {
                tile = Grid.Instance.TryGetTile(new Vector2Int(kim.GetCurrentTile().x - x, kim.GetCurrentTile().x - y));

                if(tile == null)
                    continue;

                if(!tile.occupied && !tile.innerZombie)
                    tileList.Add(tile);
            }
        }

        return tileList[(int)Random.Range(0, tileList.Count-1)];
    }
}
