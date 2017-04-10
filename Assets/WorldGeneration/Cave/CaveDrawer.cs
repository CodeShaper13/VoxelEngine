using System.Collections.Generic;
using UnityEngine;

namespace WorldGeneration.Cave {

    public class CaveDrawer {

        public const float MAX_BOUNDS = 250;
        public const float MAX_ROT = 10f;

        public Vector3 pos;
        public Vector3 direction;

        public CaveDrawer(Vector3 pos, Vector3 direction) {
            this.pos = pos;
            this.direction = direction;
        }

        // Returns true if we should stop generating this branch
        public void addNextSegment(List<CaveDrawer> drawers, List<CaveSegment> caveSegements) {
            //A chance to branch
            if(Random.Range(0, 5) == 0) { //One in five
                Vector3 dirVec = Random.rotation.eulerAngles.normalized;
                drawers.Add(new CaveDrawer(this.pos, new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f))));
            }

            Vector3 endPnt = this.pos + (this.direction * Random.Range(1.5f, 3f));
            endPnt = Quaternion.Euler(Random.Range(-MAX_ROT, MAX_ROT), Random.Range(-MAX_ROT, MAX_ROT), Random.Range(-MAX_ROT, MAX_ROT)) * endPnt;

            caveSegements.Add(new CaveSegment(this.pos, endPnt));
            this.direction = (endPnt - this.pos).normalized;
            this.pos = endPnt;
        }

        public bool outOfBounds() {
            if(this.pos.x > CaveDrawer.MAX_BOUNDS || this.pos.x < -CaveDrawer.MAX_BOUNDS ||
                this.pos.y > CaveDrawer.MAX_BOUNDS || this.pos.y < -CaveDrawer.MAX_BOUNDS||
                this.pos.z > CaveDrawer.MAX_BOUNDS || this.pos.z < -CaveDrawer.MAX_BOUNDS) {
                return true;
            }
            return false;
        }
    }
}
