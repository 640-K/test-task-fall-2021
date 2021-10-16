using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    public class Grid
    {
        public int width { get; private set; }
        public int height { get; private set; }
        private float size;
        private Vector3 startPosition;
        private PathNode[,] gridArray;

        public Grid(float size, Vector3 start, Vector3 end, LayerMask walkableMask)
        {
            this.width = Mathf.CeilToInt(Mathf.Abs((start.x - end.x) / size));
            this.height = Mathf.CeilToInt(Mathf.Abs((start.y - end.y) / size));
            this.size = size;
            startPosition = start;
            gridArray = new PathNode[this.width, this.height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++) {
                    bool walkable = Physics2D.OverlapBoxAll(GetWorldPositionGizmos(x, y), Vector2.one*size, 0, walkableMask).Length == 0;
                    gridArray[x, y] = new PathNode(x, y, walkable);
                }
            }

        }

        private Vector3 GetWorldPosition(int x, int y)
        {
            return new Vector3(x, y) * size + startPosition;
        }

        public Vector3 GetWorldPositionGizmos(int x, int y)
        {
            return GetWorldPosition(x, y) + new Vector3(size / 2, size / 2, 0);
        }

        public PathNode GetPathNode(int x, int y)
        {
            if (x<width && y<height)
                return gridArray[x, y];
            return null;
        }

        public PathNode getNodeFromWorldPosition(Vector3 position)
        {
            int x = Mathf.CeilToInt(Mathf.Abs(startPosition.x - position.x) / size)-1;
            int y = Mathf.CeilToInt(Mathf.Abs(startPosition.y - position.y) / size)-1;
            return GetPathNode(x, y);
        }
    }
}