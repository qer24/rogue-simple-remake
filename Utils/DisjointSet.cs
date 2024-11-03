namespace RogueProject.Utils;

/// <summary>
/// Disjoint set data structure
/// </summary>
public class DisjointSet(int size)
{
    private readonly int[] _parent = Enumerable.Range(0, size).ToArray();
    private readonly int[] _rank = new int[size];

    public int Find(int x)
    {
        if (_parent[x] != x)
            _parent[x] = Find(_parent[x]);
        return _parent[x];
    }

    public void Union(int x, int y)
    {
        int rootX = Find(x);
        int rootY = Find(y);

        if (rootX == rootY) return;

        if (_rank[rootX] < _rank[rootY])
        {
            _parent[rootX] = rootY;
        }
        else if (_rank[rootX] > _rank[rootY])
        {
            _parent[rootY] = rootX;
        }
        else
        {
            _parent[rootY] = rootX;
            _rank[rootX]++;
        }
    }
}
