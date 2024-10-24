using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class Kim : CharacterController
{
    [SerializeField] float ContextRadius;
    public bool isWalkingToSafety = false;

    private Node root;

    protected Node SetupTree()
    {
        root = new Selector(new List<Node>
        {
            new Sequence(new List<Node>
            {
                new CheckZombieTile(this),
                new DodgeZombie(this)
            }),
            new PathToTarget(this, Grid.Instance.GetFinishTile())
        });
        return root;
    }

    public override void StartCharacter()
    {
        base.StartCharacter();

        SetupTree();
    }

    public List<Grid.Tile> GetTileListFromNode(Astar.Node node)
    {
        var Tiles = new List<Grid.Tile>();
        while (node.Previous != null)
        {
            node.Tile.isPathTile = true;
            Tiles.Add(node.Tile);
            node = node.Previous;
        }

        Tiles.Reverse();

        return Tiles;
    }

    void EvalueateBehaviourTree()
    {
        if (root != null)
        {
            root.Evaluate();
        }
    }

    void UpdateClosestZombie()
    {
        Zombie closest = GetClosest(GetContextByTag("Zombie"))?.GetComponent<Zombie>();

         if (!closest)
            return;

        Grid.Tile tile;

        for (int x = -4; x <= 4; x++)
        {
            for (int y = -4; y <= 4; y++)
            {
                if (x == 4 && y == 4 || x == -4 && y == 4 || x == -4 && y == 4 || x == -4 && y == -4)
                    continue;

                tile = new Grid.Tile
                {
                    x = closest.GetCurrentTile().x - x,
                    y = closest.GetCurrentTile().y - y
                };

                tile = Grid.Instance.TryGetTile(new Vector2Int(tile.x, tile.y));

                if (tile == null)
                    continue;

                tile.innerZombie = true;
            }
        }
    }

    float time = 0.2f;

    public override void UpdateCharacter()
    {
        base.UpdateCharacter();

        if (time <= 0)
        {
            time -= Time.deltaTime;
        }
        else
        {
            Grid.Instance.ResetGrid();
            UpdateClosestZombie();
            EvalueateBehaviourTree();
            time = 0.2f;
        }
    }

    Vector3 GetEndPoint()
    {
        return Grid.Instance.WorldPos(Grid.Instance.GetFinishTile());
    }

    public GameObject[] GetContextByTag(string aTag)
    {
        Collider[] context = Physics.OverlapSphere(transform.position, ContextRadius);
        List<GameObject> returnContext = new List<GameObject>();
        foreach (Collider c in context)
        {
            if (c.transform.CompareTag(aTag))
            {
                returnContext.Add(c.gameObject);
            }
        }
        return returnContext.ToArray();
    }

    public GameObject GetClosest(GameObject[] aContext)
    {
        float dist = float.MaxValue;
        GameObject Closest = null;
        foreach (GameObject z in aContext)
        {
            float curDist = Vector3.Distance(transform.position, z.transform.position);
            if (curDist < dist)
            {
                dist = curDist;
                Closest = z;
            }
        }
        return Closest;
    }

    public Grid.Tile GetCurrentTile()
    {
        return myCurrentTile;
    }
}
