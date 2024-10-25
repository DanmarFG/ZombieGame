using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Astar : MonoBehaviour
{
    public static Node GetPath(Grid.Tile startTile, Grid.Tile endTile)
    {
        var _grid = Grid.Instance;
        var openList = new List<Node> {new (startTile)};
        var closedList = new List<Node>();

        var current = new Node(startTile);

        int iterations = 0;

        while (openList.Any() || iterations < 100)
        {
             current = openList[0];

            foreach (var node in openList.Where(node => node.F  <= current.F))
            {
                current = node;
            }

            openList.Remove(current);
            closedList.Add(current);

            if (_grid.IsSameTile(current.Tile, endTile))
            {
                return current;
            }

            var neighbors = new List<Node>();

            for (var x = -1; x <= 1; x++)
            {
                for (var y = -1; y <= 1; y++)
                {
                    if (x == 0 && y == 0)
                        continue;

                    foreach (var tile in _grid.GetTiles().Where(tile => tile.x == current.Tile.x - x && tile.y == current.Tile.y - y))
                    {
                        var node = new Node(tile);

                        neighbors.Add(node);
                        break;
                    }
                }
            }

            foreach (var child in neighbors)
            {
                child.Previous = current;

                if (closedList.Any(node => _grid.IsSameTile(node.Tile, child.Tile)) || child.Tile.occupied)
                    goto Repeat;
                
                if(child.Tile.innerZombie)
                    child.SetG(current.G + 99f);
                else
                    child.SetG(current.G + 1f);

                child.SetH(Mathf.Sqrt(Mathf.Pow(child.Tile.x - endTile.x, 2) + Mathf.Pow(child.Tile.y - endTile.y,2)));

                foreach (var node in openList.Where(node => _grid.IsSameTile(child.Tile, node.Tile)).Where(node => child.G > node.G))
                {
                    goto Repeat;
                }
                             

                openList.Add(child);

                Repeat: continue;
            }

            iterations++;
        }

        return null;
    }

    public static List<Grid.Tile> GetTileListFromNode(Astar.Node node)
    {
        var tiles = new List<Grid.Tile>();
        while (node.Previous != null)
        {
            node.Tile.isPathTile = true;
            tiles.Add(node.Tile);
            node = node.Previous;
        }

        tiles.Reverse();

        return tiles;
    }

    public record Node
    {
        public Node(Grid.Tile tile)
        {
            this.Tile = tile;

            G = 0;
            H = 0;
        }

        public Node Previous;

        public Grid.Tile Tile;

        public float G { get; private set; }
        public float H { get; private set; }
        public float F => G + H;
        public void SetG(float g) => G = g;
        public void SetH(float h) => H = h;
    }

}


