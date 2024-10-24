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

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    public override void StartCharacter()
    {
        base.StartCharacter();

        SetupTree();

        StartCoroutine(UpdateClosestZombie());
        StartCoroutine(EvalueateBehaviourTree());
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

    IEnumerator EvalueateBehaviourTree()
    {
        while (true)
        {
            if (root != null)
            {
                root.Evaluate();
            }
            yield return new WaitForSeconds(0.2f);
        }
    }

    IEnumerator UpdateClosestZombie()
    {
        while (true)
        {
            Grid.Instance.ResetGrid();

            Zombie closest = GetClosest(GetContextByTag("Zombie"))?.GetComponent<Zombie>();

            //I have found out that goto might be a bad statement to use, but it fits so well here,
            //so thats why i am deciding to keep it in here :3 but after this ill be considering wether or not its worth using in specific cases: https://stackoverflow.com/questions/3517726/what-is-wrong-with-using-goto
            if (!closest)
                goto UpdateTree;

            Grid.Tile tile;

            for (int x = -4; x <= 4; x++)
            {
                for (int y = -4; y <= 4; y++)
                {

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

        UpdateTree:

            yield return new WaitForSeconds(0.2f);
        }
    }

    public override void UpdateCharacter()
    {
        base.UpdateCharacter();        
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
