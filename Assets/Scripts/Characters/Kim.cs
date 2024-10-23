using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class Kim : CharacterController
{
    [SerializeField] float ContextRadius;

    private Node root;

    protected Node SetupTree()
    {
        root = new Selector(new List<Node>
        {
            new Sequence(new List<Node>
            {
                new FindBurgers(),
                new PathToBurger(this)
            }),
            new PathToExit(this)
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

    public override void UpdateCharacter()
    {
        base.UpdateCharacter();

        //Zombie closest = GetClosest(GetContextByTag("Zombie"))?.GetComponent<Zombie>();

        if (root != null)
        {
            root.Evaluate();
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
}
