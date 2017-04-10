using System.Collections.Generic;
using UnityEngine;
using VoxelEngine.Level;
using VoxelEngine.Util;

namespace WorldGeneration {

    public class RegionWall : IDebugDisplayable {
        public Vector3 planeOrgin;
        public PlaneDirection planeDirection;
        //A vector that points inside the region
        public Vector3 outside;
        //A vector that points away from the region
        public Vector3 inside;

        public List<Vector3> caveOrgins;

        public RegionWall(Vector3 pos, PlaneDirection dir, bool switchNormal) {
            this.planeOrgin = pos;
            this.planeDirection = dir;
            this.outside = switchNormal ? this.planeDirection.negative : this.planeDirection.positive;
            this.inside = switchNormal ? this.planeDirection.positive : this.planeDirection.negative;
            this.caveOrgins = new List<Vector3>();
        }

        public void generateCavePoints() {
            foreach (Vector2 v in this.getCavePoints()) {
                this.caveOrgins.Add(this.get3dVec(v) + this.planeOrgin);
            }
        }

        private Vector3 get3dVec(Vector2 vec) {
            if (this.planeDirection == PlaneDirection.x) {
                return new Vector3(0, vec.x, vec.y);
            } else if (this.planeDirection == PlaneDirection.y) {
                return new Vector3(vec.x, 0, vec.y);
            } else if (this.planeDirection == PlaneDirection.z) {
                return new Vector3(vec.x, vec.y, 0);
            }
            return Vector3.zero;
        }

        //Generates all the cave starting points on the wall, returning a list of their 2d coordinates
        private List<Vector2> getCavePoints() {
            List<Vector2> points = new List<Vector2>();
            int stepSize = 32;
            int i = (Region.SIZE * 16) / 2;
            for (int x = -i; x < i; x += stepSize) {
                for(int y = -i; y < i; y += stepSize) {
                    points.Add(new Vector2(x, y));
                }
            }
            return points;
        }

        private List<Vector2> getCavePointsNoise() {
            List<Vector2> points = new List<Vector2>();
            for (float x = -7.5f; x <= 7.5f; x += 0.5f) {
                for (float y = -7.5f; y <= 7.5f; y += 0.5f) {
                    float n = Mathf.PerlinNoise(x + 12, y + -23);
                    if(n > 0.5f) {
                        points.Add(new Vector2(x, y));
                    }
                }
            }
            return points;
        }

        public void debugDisplay() {
            float f = (Region.SIZE * Chunk.SIZE) / 2 - 0.1f;
            Debug.DrawLine(planeDirection.topLeft * f + planeOrgin, planeDirection.topRight * f + planeOrgin, Color.red);
            Debug.DrawLine(planeDirection.topRight * f + planeOrgin, planeDirection.bottomRight * f + planeOrgin, Color.red);
            Debug.DrawLine(planeDirection.bottomRight * f + planeOrgin, planeDirection.bottomLeft * f + planeOrgin, Color.red);
            Debug.DrawLine(planeDirection.bottomLeft * f + planeOrgin, planeDirection.topLeft * f + planeOrgin, Color.red);

            foreach (Vector3 v in this.caveOrgins) {
                Debug.DrawLine(v + this.outside / 8, v + this.inside / 8, Color.blue);
            }
        }
    }
}
