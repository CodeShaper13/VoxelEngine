using UnityEngine;
using System;
using VoxelEngine.Level;

namespace VoxelEngine.Util {

    [Serializable]
    public struct BlockPos {
        /// <summary> Short for 0, 0, 0 </summary>
        public static BlockPos zero = new BlockPos(0, 0, 0);
        /// <summary> Short for 1, 1, 1 </summary>
        public static BlockPos one = new BlockPos(1, 1, 1);
        /// <summary> Short for 0, 0, 1 </summary>
        public static BlockPos north = new BlockPos(0, 0, 1);
        /// <summary> Short for 1, 0, 0 </summary>
        public static BlockPos east = new BlockPos(1, 0, 0);
        /// <summary> Short for 0, 0, -1 </summary>
        public static BlockPos south = new BlockPos(0, 0, -1);
        /// <summary> Short for -1, 0, 0 </summary>
        public static BlockPos west = new BlockPos(-1, 0, 0);
        /// <summary> Short for 0, 1, 0 </summary>
        public static BlockPos up = new BlockPos(0, 1, 0);
        /// <summary> Short for 0, -1, 0 </summary>
        public static BlockPos down = new BlockPos(0, -1, 0);

        public int x;
        public int y;
        public int z;

        public BlockPos(int x, int y, int z) {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public ChunkPos toChunkPos() {
            return new ChunkPos(this.x / Chunk.SIZE, this.y / Chunk.SIZE, this.z / Chunk.SIZE);
        }

        public BlockPos move(Direction dir) {
            return new BlockPos(this.x + dir.direction.x, this.y + dir.direction.y, this.z + dir.direction.z);
        }

        public override string ToString() {
            return "(" + this.x + ", " + this.y + ", " + this.z + ")";
        }

        public override bool Equals(object obj) {
            return this.GetHashCode() == obj.GetHashCode();
        }

        public override int GetHashCode() {
            unchecked {
                int hash = 47;
                hash = hash * 227 + x.GetHashCode();
                hash = hash * 227 + y.GetHashCode();
                hash = hash * 227 + z.GetHashCode();
                return hash;
            }
        }

        public static BlockPos operator +(BlockPos b, BlockPos b1) {
            return new BlockPos(b.x + b1.x, b.y + b1.y, b.z + b1.z);
        }

        public static BlockPos operator -(BlockPos b, BlockPos b1) {
            return new BlockPos(b.x - b1.x, b.y - b1.y, b.z - b1.z);
        }

        public static BlockPos operator *(BlockPos b, int i) {
            return new BlockPos(b.x * i, b.y * i, b.z * i);
        }

        public static BlockPos operator /(BlockPos b, int i) {
            return new BlockPos(b.x / i, b.y / i, b.z / i);
        }

        /// <summary>
        /// Adds 1 to all of the axes that are not 0
        /// </summary>
        public static BlockPos operator ++(BlockPos p) {
            return new BlockPos(
                p.x + (p.x != 0 ? 1 : 0),
                p.y + (p.y != 0 ? 1 : 0),
                p.z + (p.z != 0 ? 1 : 0));
        }

        /// <summary>
        /// Subtracts 1 from all of the axes that are not 0
        /// </summary>
        public static BlockPos operator --(BlockPos p) {
            return new BlockPos(
                p.x - (p.x != 0 ? 1 : 0),
                p.y - (p.y != 0 ? 1 : 0),
                p.z - (p.z != 0 ? 1 : 0));
        }

        public Vector3 toVector() {
            return new Vector3(this.x, this.y, this.z);
        }

        public static BlockPos fromVec(Vector3 vector) {
            return new BlockPos(Mathf.RoundToInt(vector.x), Mathf.RoundToInt(vector.y), Mathf.RoundToInt(vector.z));
        }

        public static BlockPos fromRaycastHit(RaycastHit hit) {
            // Debug.DrawLine(Camera.main.transform.position, hit.point, Color.red);

            Vector3 vec = hit.point + ((hit.normal * -1f) / 100);
            int x = Mathf.RoundToInt(vec.x);
            int y = Mathf.RoundToInt(vec.y);
            int z = Mathf.RoundToInt(vec.z);

            return new BlockPos(x, y, z);

            /*
            float f = adjacent ? 1f : -1f;
            BlockPos pos = BlockPos.fromVec(new Vector3(
                BlockPos.moveWithinBlock(hit.point.x + (hit.normal.x / 4) * f, hit.normal.x, adjacent),
                BlockPos.moveWithinBlock(hit.point.y + (hit.normal.y / 4) * f, hit.normal.y, adjacent),
                BlockPos.moveWithinBlock(hit.point.z + (hit.normal.z / 4) * f, hit.normal.z, adjacent)));
            return pos;
            */
        }

        // Unused Is it broken?
        private static float moveWithinBlock(float point, float normal, bool adjacent = false) {
            //Debug.Log(point + ", " + normal + " VALUE: " + (point - (int)point));
            if ((point - (int)point) == 0.5f || (point - (int)point) == -0.5f) {
                if (adjacent) {
                    point += (normal / 2);
                }
                else {
                    point -= (normal / 2);
                }
            }
            return point;
        }
    }
}