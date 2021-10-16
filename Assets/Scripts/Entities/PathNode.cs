using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{

    public class PathNode
    {
        public int x { get; private set; }
        public int y { get; private set; }
        public bool isWalkable { get; private set; }


        public int gCost;
        public int hCost;
        public int fCost;

        public PathNode cameFromNode;


        public PathNode(int x, int y, bool isWalkable)
        {
            this.x = x;
            this.y = y;
            this.isWalkable = isWalkable;

        }

        public void CalculateFCost()
        {
            fCost = gCost + hCost;
        }
    }
}