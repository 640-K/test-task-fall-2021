using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{

    public class Pathfinding
    {
        public Grid grid { get; private set; }
        private List<PathNode> openList;
        private List<PathNode> closedList;

        public Pathfinding(float size, Transform start, Transform end, LayerMask noWalkableMask)
        {
            grid = new Grid(size, start.position, end.position, noWalkableMask);
        }

        public List<PathNode> FindPath(int startX, int startY, int endX, int endY) 
        {
            PathNode startNode = grid.GetPathNode(startX, startY);
            PathNode endNode = grid.GetPathNode(endX, endY);

            openList = new List<PathNode> { startNode };
            closedList = new List<PathNode>();

            for(int x=0; x< grid.width; x++)
            {
                for (int y = 0; y < grid.height; y++)
                {
                    PathNode pathNode = grid.GetPathNode(x, y);
                    pathNode.gCost = int.MaxValue;
                    pathNode.CalculateFCost();
                    pathNode.cameFromNode = null;
                }
            }

            startNode.gCost = 0;
            startNode.hCost = CalculateDistaceCost(startNode, endNode);
            startNode.CalculateFCost();

            while(openList.Count > 0)
            {
                PathNode currentNode = GetLowestCostNode(openList);
                if(currentNode == endNode)
                {
                    return CalculatePath(endNode);
                }

                openList.Remove(currentNode);
                closedList.Add(currentNode);

                foreach(PathNode neibourt in GetNeibourtList(currentNode)){
                    if (closedList.Contains(neibourt)) continue;

                    int gCost = currentNode.gCost + CalculateDistaceCost(currentNode, neibourt);
                    if (gCost < neibourt.gCost)
                    {
                        neibourt.cameFromNode = currentNode;
                        neibourt.gCost = gCost;
                        neibourt.hCost = CalculateDistaceCost(neibourt, endNode);
                        neibourt.CalculateFCost();

                        if (!openList.Contains(neibourt))
                        {
                            openList.Add(neibourt);
                        }
                    }
                }
            }
            return null;

        }

        private List<PathNode> GetNeibourtList(PathNode currentNode)
        {
            List<PathNode> neibourtList = new List<PathNode>();

            if(currentNode.x -1 >= 0)
            {

                if (grid.GetPathNode(currentNode.x - 1, currentNode.y).isWalkable) neibourtList.Add(grid.GetPathNode(currentNode.x - 1, currentNode.y));
                if (currentNode.y -1 >= 0 && grid.GetPathNode(currentNode.x - 1, currentNode.y-1).isWalkable) neibourtList.Add(grid.GetPathNode(currentNode.x - 1, currentNode.y-1));
                if (currentNode.y + 1 <= grid.height && grid.GetPathNode(currentNode.x - 1, currentNode.y+1).isWalkable) neibourtList.Add(grid.GetPathNode(currentNode.x - 1, currentNode.y + 1));

            }
            if (currentNode.x + 1 <= grid.width)
            {

                if (grid.GetPathNode(currentNode.x + 1, currentNode.y).isWalkable) neibourtList.Add(grid.GetPathNode(currentNode.x + 1, currentNode.y));
                if (currentNode.y - 1 >= 0 && grid.GetPathNode(currentNode.x + 1, currentNode.y - 1).isWalkable) neibourtList.Add(grid.GetPathNode(currentNode.x + 1, currentNode.y - 1));
                if (currentNode.y + 1 <= grid.height && grid.GetPathNode(currentNode.x + 1, currentNode.y + 1).isWalkable) neibourtList.Add(grid.GetPathNode(currentNode.x + 1, currentNode.y + 1));

            }

            if (currentNode.y - 1 >= 0 && grid.GetPathNode(currentNode.x, currentNode.y - 1).isWalkable) neibourtList.Add(grid.GetPathNode(currentNode.x, currentNode.y - 1));
            if (currentNode.y + 1 <= grid.height && grid.GetPathNode(currentNode.x, currentNode.y + 1).isWalkable) neibourtList.Add(grid.GetPathNode(currentNode.x, currentNode.y + 1));


            return neibourtList;
        }

        private List<PathNode> CalculatePath(PathNode endNode)
        {
            List<PathNode> path = new List<PathNode>();
            path.Add(endNode);
            PathNode currentNode = endNode;
            while(currentNode.cameFromNode != null)
            {
                path.Add(currentNode);
                currentNode = currentNode.cameFromNode;
            }
            path.Reverse();
            return path;
        }

        private int CalculateDistaceCost(PathNode a, PathNode b)
        {
            int xDistance = Mathf.Abs(a.x - b.x);
            int yDistance = Mathf.Abs(a.y - b.y);
            int remaining = Mathf.Abs(xDistance - yDistance);
            return 14 * Mathf.Min(xDistance, yDistance) + 10 * remaining;
        }

        private PathNode GetLowestCostNode(List<PathNode> pathNodes)
        {
            PathNode lowestCostPathNode = pathNodes[0];
            for(int i=1;i<pathNodes.Count; i++)
            {
                if(pathNodes[i].fCost < lowestCostPathNode.fCost)
                {
                    lowestCostPathNode = pathNodes[i];
                }
            }
            return lowestCostPathNode;
        }
    }
}