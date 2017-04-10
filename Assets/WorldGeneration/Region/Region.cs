using System.Collections.Generic;
using UnityEngine;
using VoxelEngine.Util;
using WorldGeneration.Cave;

namespace WorldGeneration {

    //A region in an area of chunks
    public class Region : IDebugDisplayable {
        public const int SIZE = 16; //16 * 16 * 16 chunks in size

        public Vector3 center;

        public RegionWall[] regionWallList;
        public List<CaveSegment> caveSegments;

        public Region(Vector3 center) {
            this.center = center;
            this.regionWallList = new RegionWall[6];
            this.regionWallList[0] = this.getRegionWall(PlaneDirection.z, true);  //North
            this.regionWallList[1] = this.getRegionWall(PlaneDirection.x, true);  //East
            this.regionWallList[2] = this.getRegionWall(PlaneDirection.z, false); //South
            this.regionWallList[3] = this.getRegionWall(PlaneDirection.x, false); //West
            this.regionWallList[4] = this.getRegionWall(PlaneDirection.y, true);  //Up
            this.regionWallList[5] = this.getRegionWall(PlaneDirection.y, false); //Down

            this.caveSegments = new List<CaveSegment>();

            this.calculateCaves();
        }

        private void calculateCaves() {
            //get a list of every cave orgin we will be using
            List<Vector3> points = new List<Vector3>();
            foreach (RegionWall wall in this.regionWallList) {
                points.AddRange(wall.caveOrgins);
            }

            List<Vector3> p;
            //p = this.generateCavePass(points);
            //p = this.generateCavePass(p);
            //p = this.generateCavePass(p);
            //p = this.generateCavePass(p);
            //p = this.generateCavePass(p);

            //this.mergeClosePoints(p);
        }

        private List<Vector3> generateCavePass(List<Vector3> pointsToChild) {
            List<Vector3> newPoints = new List<Vector3>();
            foreach (Vector3 caveStart in pointsToChild) {
                //for every cave point, add another point close to the center
                float f = 5f;
                Quaternion q = Quaternion.Euler(Random.Range(-f, f), Random.Range(-f, f), Random.Range(-f, f));

                Vector3 v = Vector3.MoveTowards(caveStart, this.center, Random.Range(16f, 32f));
                v = q * v;
                newPoints.Add(v);
                this.caveSegments.Add(new CaveSegment(caveStart, v));
            }
            return newPoints;
        }

        private void mergeClosePoints(List<Vector3> allPoints) {
            for(int i1 = 0; i1 < 1; i1++) {

                for(int i = allPoints.Count - 1; i >= 0; i--) {
                    Vector3 v = allPoints[i];

                    //For every point, look at every other to see if there are any close one
                    for (int j = allPoints.Count - 1; j >= 0; j--) {
                        if(i != j) { //Dont compare the same points
                            Vector3 v1 = allPoints[j];
                            if(Vector3.Distance(v, v1) > 2) {
                                Debug.Log("Merging points");
                                Vector3 newVec = (v + v1) / 2;
                                allPoints.Remove(v);
                                allPoints.Remove(v1);
                                allPoints.Add(newVec);
                            }
                        }
                    }
                }
            }
        }

        private RegionWall getRegionWall(PlaneDirection dir, bool flag) {
            RegionWall wall = new RegionWall((flag ? dir.positive : dir.negative) * ((Region.SIZE / 2) * 16), dir, !flag);
            wall.generateCavePoints();
            return wall;
        }

        public void debugDisplay() {
            foreach (RegionWall wall in this.regionWallList) {
                wall.debugDisplay();
            }
            foreach(CaveSegment s in this.caveSegments) {
                s.debugDisplay();
            }
        }
    }
}
