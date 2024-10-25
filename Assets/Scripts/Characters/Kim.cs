using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using Unity.VisualScripting;
using Sequence = BehaviourTree.Sequence;
using static UnityEngine.GraphicsBuffer;

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
                new CheckZombieTile(this),
                new DodgeZombie(this),
            }),
            new Sequence(new List<Node>
            {
                new FindBurgers(this),
                new PathToBurger(this),
            }),
            new PathToTarget(this, Grid.Instance.GetFinishTile())
        });
        return root;
    }

    public override void StartCharacter()
    {
        base.StartCharacter();

        StartCoroutine(UpdateClosestZombie());
        StartCoroutine(EvalueateBehaviourTree());
    }

    IEnumerator EvalueateBehaviourTree()
    {
        SetupTree();

        yield return new WaitForSeconds(0.2f);

        while (root != null)
        {
            //root.Evaluate();
            SetWalkBuffer(Astar.GetTileListFromNode(Astar.GetPath(GetCurrentTile(), new Grid.Tile{x = 30,y = 5})));
            yield return new WaitForSeconds(0.2f);
        }
    }

    private const int TotalTilesToCheck = 3;

    void OnDisable()
    {
        StopAllCoroutines();
    }

    IEnumerator UpdateClosestZombie()
    {

        while (true)
        {
            var closest = GetClosest(GetContextByTag("Zombie"))?.GetComponent<Zombie>();

            if (closest == null)
            {
                yield return new WaitForSeconds(0.2f);
                continue;
            }

            for (var x = -TotalTilesToCheck; x <= TotalTilesToCheck; x++)
            {
                for (var y = -TotalTilesToCheck; y <= TotalTilesToCheck; y++)
                {
                    if (x == TotalTilesToCheck && y == TotalTilesToCheck || x == -TotalTilesToCheck && y == TotalTilesToCheck || x == TotalTilesToCheck && y == -TotalTilesToCheck || x == -TotalTilesToCheck && y == -TotalTilesToCheck)
                        continue;

                    Grid.Tile tile = Grid.Instance.TryGetTile(new Vector2Int(closest.GetCurrentTile().x - x, closest.GetCurrentTile().y - y));

                    if (tile == null || tile.finishTile || tile.occupied)
                        continue;

                    tile.innerZombie = true;
                }
            }

            yield return new WaitForSeconds(0.2f);
            Grid.Instance.ResetGrid();
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

    public Grid.Tile GetCurrentTile()
    {
        return Grid.Instance.TryGetTile(new Vector2Int(myCurrentTile.x, myCurrentTile.y));
    }
}
