using UnityEngine;
using VoxelEngine.Level;

namespace VoxelEngine.Util {

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

        public BlockPos(int i) {
            this.x = i;
            this.y = i;
            this.z = i;
        }

        public BlockPos(int x, int y, int z) {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public BlockPos(Vector3 vector) {
            this.x = Mathf.RoundToInt(vector.x);
            this.y = Mathf.RoundToInt(vector.y);
            this.z = Mathf.RoundToInt(vector.z);
        }

        /// <summary>
        /// Converts the block pos to a chunk pos, dividing everything by Chunk.SIZE.
        /// </summary>
        public ChunkPos toChunkPos() {
            return new ChunkPos(this.x / Chunk.SIZE, this.y / Chunk.SIZE, this.z / Chunk.SIZE);
        }

        /// <summary>
        /// Returns a new block pos moved in the passes direction.
        /// </summary>
        public BlockPos move(Direction direction) {
            return new BlockPos(this.x + direction.blockPos.x, this.y + direction.blockPos.y, this.z + direction.blockPos.z);
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
                hash = hash * 227 + x;
                hash = hash * 227 + y;
                hash = hash * 227 + z;
                return hash;
            }
        }

        /// <summary>
        /// Adds the passed values to the block pos.
        /// </summary>
        public BlockPos add(int x, int y, int z) {
            return new BlockPos(this.x + x, this.y + y, this.z + z);
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
        /// Adds 1 to all of the axes that are not 0.
        /// </summary>
        public static BlockPos operator ++(BlockPos p) {
            return new BlockPos(
                p.x + (p.x != 0 ? 1 : 0),
                p.y + (p.y != 0 ? 1 : 0),
                p.z + (p.z != 0 ? 1 : 0));
        }

        /// <summary>
        /// Subtracts 1 from all of the axes that are not 0.
        /// </summary>
        public static BlockPos operator --(BlockPos p) {
            return new BlockPos(
                p.x - (p.x != 0 ? 1 : 0),
                p.y - (p.y != 0 ? 1 : 0),
                p.z - (p.z != 0 ? 1 : 0));
        }

        /// <summary>
        /// Coverts the BlockPos to a Vector3
        /// </summary>
        public Vector3 toVector() {
            return new Vector3(this.x, this.y, this.z);
        }

        /// <summary>
        /// Rotates a BlockPos around a pivot and returns it.
        /// </summary>
        public BlockPos rotateAround(BlockPos pivot, Quaternion angle) { // TODO optimize!
            return new BlockPos(MathHelper.rotateVecAround(this.toVector(), pivot.toVector(), angle));
        }

        public static BlockPos fromRaycastHit(RaycastHit hit) {
            Vector3 vec = hit.point + ((hit.normal * -1f) / 100);
            return new BlockPos(vec);
            /*
            float f = adjacent ? 1f : -1f;
            BlockPos pos = BlockPos.fromVec(new Vector3(
                BlockPos.moveWithinBlock(hit.point.x + (hit.normal.x / 4) * f, hit.normal.x, adjacent),
                BlockPos.moveWithinBlock(hit.point.y + (hit.normal.y / 4) * f, hit.normal.y, adjacent),
                BlockPos.moveWithinBlock(hit.point.z + (hit.normal.z / 4) * f, hit.normal.z, adjacent)));
            return pos;
            */
        }

        /// <summary>
        /// Returns a random position within a chunk.
        /// </summary>
        public static BlockPos rndInChunk(System.Random rnd) {
            return new BlockPos(rnd.Next(0, Chunk.SIZE), rnd.Next(0, Chunk.SIZE), rnd.Next(0, Chunk.SIZE));
        }

        public bool isZero() {
            return this.x == 0 && this.y == 0 && this.z == 0;
        }
    }
}