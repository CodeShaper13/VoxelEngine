using UnityEngine;

namespace WorldGeneration {

    public class PlaneDirection {
        public static PlaneDirection x = new PlaneDirection(Vector3.right, Vector3.left, new Vector3(0, 1, 1), new Vector3(0, -1, 1), new Vector3(0, 1, -1), new Vector3(0, -1, -1));
        public static PlaneDirection y = new PlaneDirection(Vector3.up, Vector3.down, new Vector3(1, 0, 1), new Vector3(-1, 0, 1), new Vector3(1, 0, -1), new Vector3(-1, 0, -1));
        public static PlaneDirection z = new PlaneDirection(Vector3.forward, Vector3.back, new Vector3(1, 1, 0), new Vector3(-1, 1, 0), new Vector3(1, -1, 0), new Vector3(-1, -1, 0));

        public Vector3 positive;
        public Vector3 negative;
        public Vector3 topRight;
        public Vector3 topLeft;
        public Vector3 bottomRight;
        public Vector3 bottomLeft;

        public PlaneDirection(Vector3 positive, Vector3 negative, Vector3 tr, Vector3 tl, Vector3 br, Vector3 bl) {
            this.positive = positive;
            this.negative = negative;

            this.topRight = tr;
            this.topLeft = tl;
            this.bottomRight = br;
            this.bottomLeft = bl;
        }
    }
}
