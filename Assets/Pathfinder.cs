using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    const float boxSize = 2.0f;
    static readonly Vector2Int start = new Vector2Int(-85, -85);
    static readonly Vector2Int end = new Vector2Int(85, 85);

    public Dictionary<Vector2Int, bool> map;

    static Pathfinder _instance;
    public static Pathfinder Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = FindObjectOfType<Pathfinder>();
            }
            if (!_instance)
            {
                _instance = new GameObject("Pathfinder").AddComponent<Pathfinder>();
            }
            return _instance;
        }
    }

    private void Start()
    {
        RegenerateMap();
    }

    [ContextMenu("Regenerate Map")]
    private void RegenerateMap()
    {
        LayerMask mask = LayerMask.GetMask("Default");
        map = new Dictionary<Vector2Int, bool>();

        int xw = (end.x - start.x) / (int)boxSize;
        int yw = (end.y - start.y) / (int)boxSize;

        for (int x = 0; x < xw; x++)
        {
            for (int y = 0; y < yw; y++)
            {
                Vector2Int key = new Vector2Int(x, y);
                Vector2 worldPos = start + (Vector2)key * boxSize;

                map.Add(key, Physics2D.OverlapBox(worldPos, Vector2.one * boxSize, 0.0f, mask));
            }
        }
    }

    public void GetPath(Vector2 startWorld, Vector2 endWorld, ref List<Vector2> path)
    {
        path.Clear();

        if (!Physics2D.CircleCast(startWorld, boxSize, endWorld - startWorld, (endWorld - startWorld).magnitude))
        {
            path.Add(endWorld);
            return;
        }

        List<PathfindingNode> openNodes = new List<PathfindingNode>();
        Dictionary<Vector2Int, PathfindingNode> closedNodes = new Dictionary<Vector2Int, PathfindingNode>();

        Vector2Int start = Vector2Int.RoundToInt((startWorld - Pathfinder.start) / boxSize);
        Vector2Int end = Vector2Int.RoundToInt((endWorld - Pathfinder.start) / boxSize);
        PathfindingNode head = null;

        openNodes.Add(new PathfindingNode(start, start, end, null));

        while (openNodes.Count > 0)
        {
            PathfindingNode node = openNodes[0];
            int index = 0;
            for (int i = 0; i < openNodes.Count; i++)
            {
                if (openNodes[i].fCost < node.fCost)
                {
                    node = openNodes[i];
                    index = i;
                }
            }

            openNodes.RemoveAt(index);
            closedNodes.Add(node.key, node);

            if (node.key == end)
            {
                head = node;
                break;
            }

            foreach (var offset in Neighbours)
            {
                if (!closedNodes.ContainsKey(node.key + offset))
                {
                    openNodes.Add(new PathfindingNode(node.key + offset, start, end, node));
                }
                else 
                {
                    PathfindingNode previous = closedNodes[node.key + offset].previous;

                    if (previous != null ? closedNodes[node.key + offset].previous.fCost > node.fCost : true)
                    {
                        closedNodes[node.key + offset].previous = node;
                    }
                }
            }
        }

        head = head.previous;
        path.Add(endWorld);

        while (head?.previous != null)
        {
            path.Add((Vector2)head.key * boxSize + start);
            head = head.previous;
        }

        path.Add(startWorld);

        path.Reverse();
    }

    private void OnDrawGizmosSelected()
    {
        if (map == null) RegenerateMap();

        foreach (var pair in map)
        {
            Gizmos.color = pair.Value ? Color.green : Color.red;

            Vector2 worldPos = start + (Vector2)pair.Key * boxSize;
            Gizmos.DrawWireCube(worldPos, Vector2.one * boxSize * 0.9f);
        }

        Gizmos.color = Color.white;
    }

    public class PathfindingNode
    {
        public Vector2Int key;
        public float gCost;
        public float hCost;
        public float fCost => gCost + hCost;
        public PathfindingNode previous;

        public PathfindingNode(Vector2Int key, Vector2Int start, Vector2Int end, PathfindingNode previous)
        {
            this.key = key;
            gCost = KindOfDistance(start - key);
            hCost = KindOfDistance(end - key);
            this.previous = previous;
        }
    }

    public static int KindOfDistance(Vector2Int v)
    {
        v.x = Mathf.Abs(v.x);
        v.y = Mathf.Abs(v.y);

        int min = Mathf.Min(v.x, v.y);
        int max = Mathf.Max(v.x, v.y);

        return (max - min) * 10 + min * 14;
    }

    readonly Vector2Int[] Neighbours = new Vector2Int[]
    {
        new Vector2Int( 1,  1),
        new Vector2Int(-1,  1),
        new Vector2Int( 1, -1),
        new Vector2Int(-1, -1),
    };
}
