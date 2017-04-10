using fNbt;
using UnityEngine;
using VoxelEngine.Util;

namespace WorldGeneration.Cave {

    public class CaveSegment : IDebugDisplayable {

        public Vector3 point1;
        public Vector3 point2;
        public float radius;

        private Color lineColor;

        public CaveSegment(NbtCompound tag) {
            this.point1 = NbtHelper.readDirectVector3(tag, "start");
            this.point2 = NbtHelper.readDirectVector3(tag, "end");
            this.radius = tag.Get<NbtFloat>("r").FloatValue;
        }

        public CaveSegment(Vector3 p1, Vector3 p2) {
            this.point1 = p1;
            this.point2 = p2;
            this.lineColor = new Color(0, Random.Range(0.25f, 1), 0); // Random.ColorHSV();
        }

        public void debugDisplay() {
            Debug.DrawLine(this.point1, this.point2, this.lineColor);
        }

        public void writeToNbt(NbtCompound tag) {
            NbtHelper.writeDirectVector3(tag, this.point1, "start");
            NbtHelper.writeDirectVector3(tag, this.point2, "end");
            tag.Add(new NbtFloat("r", this.radius));
        }
    }
}
