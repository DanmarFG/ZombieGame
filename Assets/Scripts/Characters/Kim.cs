using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kim : CharacterController
{
    [SerializeField] float ContextRadius;

    private Grid _grid;

    public override void StartCharacter()
    {
        base.StartCharacter();

        _grid = GameObject.Find("Grid").GetComponent<Grid>();

        SetWalkBuffer(GetTileListFromNode(Astar.GetPath(_grid, _grid.GetClosest(transform.position), _grid.GetFinishTile())));
    }

    public List<Grid.Tile> GetTileListFromNode(Astar.Node node)
    {
        var Tiles = new List<Grid.Tile>();
        while (node.Previous != null)
        {
            Tiles.Add(node.Tile);
            node = node.Previous;
        }

        Tiles.Reverse();

        return Tiles;
    }

    public override void UpdateCharacter()
    {
        base.UpdateCharacter();

        Zombie closest = GetClosest(GetContextByTag("Zombie"))?.GetComponent<Zombie>();
    }

    Vector3 GetEndPoint()
    {
        return Grid.Instance.WorldPos(Grid.Instance.GetFinishTile());
    }

    GameObject[] GetContextByTag(string aTag)
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

    GameObject GetClosest(GameObject[] aContext)
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
