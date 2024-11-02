using BlueRaja;
using RogueProject.Utils;

public class AStar2D
{
    public class Node(Vector2Int position)
    {
        public Vector2Int Position { get; } = position;
        public Node Previous { get; set; }
        public float Cost { get; set; }
    }

    public struct PathCost
    {
        public bool traversable;
        public float cost;
    }

    public static readonly Vector2Int[] NEIGHBORS =
    {
        new(1, 0),
        new(-1, 0),
        new(0, 1),
        new(0, -1)
    };

    /// <summary>
    /// Includes diagonals
    /// </summary>
    public static readonly Vector2Int[] ALL_NEIGHBORS =
    {
        new(1, 0),
        new(-1, 0),
        new(0, 1),
        new(0, -1),
        new(1, 1),
        new(-1, 1),
        new(1, -1),
        new(-1, -1)
    };

    private readonly Grid2D<Node> _grid;
    private SimplePriorityQueue<Node, float> _queue;
    private HashSet<Node> _closed;
    private readonly Stack<Vector2Int> _stack;

    public AStar2D(Grid2D<Node> grid)
    {
        _grid = grid;
        _queue = new SimplePriorityQueue<Node, float>();
        _closed = [];
        _stack = new Stack<Vector2Int>();

        var size = _grid.Size;
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                _grid[new Vector2Int(x, y)] = new Node(new Vector2Int(x, y));
            }
        }
    }

    private void ResetNodes()
    {
        var size = _grid.Size;
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                var node = _grid[new Vector2Int(x, y)];
                node.Previous = null;
                node.Cost = float.PositiveInfinity;
            }
        }
    }

    public List<Vector2Int> FindPath(Vector2Int start, Vector2Int end, Func<Node, Node, PathCost> costFunction)
    {
        ResetNodes();
        _queue.Clear();
        _closed.Clear();

        _queue = new SimplePriorityQueue<Node, float>();
        _closed = new HashSet<Node>();

        _grid[start].Cost = 0;
        _queue.Enqueue(_grid[start], 0);

        while (_queue.Count > 0)
        {
            var node = _queue.Dequeue();
            _closed.Add(node);

            if (node.Position == end) return ReconstructPath(node);

            foreach (var offset in NEIGHBORS)
            {
                if (!_grid.InBounds(node.Position + offset)) continue;

                var neighbor = _grid[node.Position + offset];

                if (_closed.Contains(neighbor)) continue;

                var pathCost = costFunction(node, neighbor);

                if (!pathCost.traversable) continue;

                float newCost = node.Cost + pathCost.cost;

                if (newCost < neighbor.Cost)
                {
                    neighbor.Previous = node;
                    neighbor.Cost = newCost;

                    if (_queue.TryGetPriority(node, out float _))
                        _queue.UpdatePriority(node, newCost);
                    else
                        _queue.Enqueue(neighbor, neighbor.Cost);
                }
            }
        }

        return null;
    }

    private List<Vector2Int> ReconstructPath(Node node)
    {
        var result = new List<Vector2Int>();

        while (node != null)
        {
            _stack.Push(node.Position);
            node = node.Previous;
        }

        while (_stack.Count > 0) result.Add(_stack.Pop());

        return result;
    }
}
