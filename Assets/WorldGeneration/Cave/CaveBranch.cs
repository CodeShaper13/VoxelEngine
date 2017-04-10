using System.Collections.Generic;
using UnityEngine;

namespace WorldGeneration.Cave {

    public class CaveBranch {

        public List<Vector3> points;

        public CaveBranch(Vector3 orgin) {
            this.points = new List<Vector3>();
            this.points.Add(orgin);
        }

        public void func() {

        }
    }
}
