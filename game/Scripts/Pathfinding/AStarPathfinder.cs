using System.Collections.Generic;
using Godot;

namespace Maze.Scripts.Pathfinding;

public class AStarPathfinder : GridBasedPathfinder
{
    public AStarPathfinder(Grid grid) : base(grid)
    {
    }
    
    public override IEnumerable<Vector2> FindPath(Vector2 start, Vector2 destination, PathOptions? options = default)
    {
        options ??= new PathOptions();
        Waypoints.Clear();
        var startNode = Grid.GetNodeFromWorldPosition(start);
        var destinationNode = Grid.GetNodeFromWorldPosition(destination);
        
        var openSet = new HashSet<Node>();
        var closedSet = new HashSet<Node>();
        var cameFrom = new Dictionary<Node, Node>();
        var gScore = new Dictionary<Node, float>();
        var fScore = new Dictionary<Node, float>();

        foreach (var node in Grid.Nodes)
        {
            gScore[node] = Mathf.Inf;
            fScore[node] = Mathf.Inf;
        }

        gScore[startNode] = 0;
        fScore[startNode] = Heuristic(startNode, destinationNode);
        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            var currentNode = GetLowestFScoreNode(openSet, fScore);

            if (currentNode is null)
            {
                break;
            }
            
            if (currentNode.Equals(destinationNode))
            {
                var paths = ConstructPath(cameFrom, destinationNode, options);
                return paths;
            }
            
            openSet.Remove(currentNode);
            closedSet.Add(currentNode);
            
            // Explore neighbors
            foreach (var neighbor in Grid.GetNeighbors(currentNode))
            {
                // Skip unwalkable nodes
                if (neighbor.Cost == 0 || closedSet.Contains(neighbor))
                {
                    continue;
                }

                var tentativeGScore = gScore[currentNode] + neighbor.Cost;
                
                if (tentativeGScore < gScore[neighbor])
                {
                    cameFrom[neighbor] = currentNode;
                    gScore[neighbor] = tentativeGScore;
                    fScore[neighbor] = gScore[neighbor] + Heuristic(neighbor, destinationNode);
                    openSet.Add(neighbor);
                }
            }
        }

        return new List<Vector2>();
    }
    
    private static Node? GetLowestFScoreNode(HashSet<Node> openSet, Dictionary<Node, float> fScore)
    {
        var lowestFScore = Mathf.Inf;
        Node? lowestFScoreNode = null;

        foreach (var node in openSet)
        {
            if (fScore[node] < lowestFScore)
            {
                lowestFScore = fScore[node];
                lowestFScoreNode = node;
            }
        }

        return lowestFScoreNode;
    }
    
    private static float Heuristic(Node a, Node b)
    {
        return a.Position.DistanceTo(b.Position);
    }
}
