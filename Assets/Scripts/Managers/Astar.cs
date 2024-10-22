using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class Astar : MonoBehaviour
{
    public static Node GetPath(Grid.Tile startTile, Grid.Tile endTile)
    {
        Grid _grid = Grid.Instance;
        var openList = new List<Node> {new (startTile)};
        var closedList = new List<Node>();

        var current = new Node(startTile);

        while (openList.Any())
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
                        Node node = new Node(tile);

                        if(x == -1 && y == -1 || x == 1 && y == -1 || x == -1 && y == 1 || x == 1 && y == 1)
                            node.isDiagonal = true;

                        neighbors.Add(node);
                        break;
                    }
                }
            }

            foreach (var child in neighbors)
            {
                child.Previous = current;

                if (closedList.Any(node => _grid.IsSameTile(node.Tile, child.Tile)) || !_grid.isReachable(current.Tile, child.Tile))
                    goto Repeat;

                if (child.Tile.occupied)
                    child.SetG(current.G + float.MaxValue);
                else
                    child.SetG(current.G + 1f);

                child.SetH(Mathf.Pow(child.Tile.x - endTile.x, 2) + Mathf.Pow(child.Tile.y - endTile.y,2));

                foreach(Node node in openList)
                {
                    if(_grid.IsSameTile(child.Tile, node.Tile))
                    {
                        if (child.G > node.G)
                        {
                            goto Repeat;
                        }
                    }
                }
                             

                openList.Add(child);

                Repeat: continue;
            }


        }

        return current;
    }

    public record Node
    {
        public Node(Grid.Tile tile)
        {
            this.Tile = tile;

            G = 0;
            H = 0;
        }

        public bool isDiagonal = false;

        public Node Previous;

        public Grid.Tile Tile;

        public float G { get; private set; }
        public float H { get; private set; }
        public float F => G + H;
        public void SetG(float g) => G = g;
        public void SetH(float h) => H = h;
    }

}


