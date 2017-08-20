using VoxelEngine.Util;

namespace VoxelEngine.Render {

    public struct CubeComponent {

        /// <summary> Positive corner. </summary>
        public BlockPos from;
        /// <summary> Negative corner. </summary>
        public BlockPos to;
        /// <summary> Rotation of the cube. </summary>
        public ComponentRotation rotation;
        /// <summary> Offset of the cube. </summary>
        public BlockPos offset;

        /// <summary>
        /// Creates a CubeComponent with the default rotation and offset.
        /// </summary>
        public CubeComponent(int toX, int toY, int toZ, int fromX, int fromY, int fromZ) {
            this.to = new BlockPos(toX, toY, toZ);
            this.from = new BlockPos(fromX, fromY, fromZ);
            this.rotation = new ComponentRotation();
            this.offset = new BlockPos();
        }

        public CubeComponent(int toX, int toY, int toZ, int fromX, int fromY, int fromZ, int rotX, int rotY, int rotZ) {
            this.to = new BlockPos(toX, toY, toZ);
            this.from = new BlockPos(fromX, fromY, fromZ);
            this.rotation = new ComponentRotation(rotX, rotY, rotZ);
            this.offset = new BlockPos();
        }

        public CubeComponent(int toX, int toY, int toZ, int fromX, int fromY, int fromZ, int rotX, int rotY, int rotZ, int offsetX, int offsetY, int offsetZ) {
            this.to = new BlockPos(toX, toY, toZ);
            this.from = new BlockPos(fromX, fromY, fromZ);
            this.rotation = new ComponentRotation(rotX, rotY, rotZ);
            this.offset = new BlockPos(offsetX, offsetY, offsetZ);
        }

        public CubeComponent(BlockPos to, BlockPos from, ComponentRotation rotation) {
            this.to = to;
            this.from = from;
            this.rotation = rotation;
            this.offset = new BlockPos();
        }

        public CubeComponent(BlockPos to, BlockPos from, ComponentRotation rotation, BlockPos offset) {
            this.to = to;
            this.from = from;
            this.rotation = rotation;
            this.offset = offset;
        }

        public CubeComponent(BlockPos orgin, int xRadius, int yRadius, int zRadius) {
            this.from = new BlockPos(orgin.x - xRadius, orgin.y - yRadius, orgin.z - zRadius);
            this.to = new BlockPos(orgin.x + xRadius, orgin.y + yRadius, orgin.z + zRadius);
            this.rotation = new ComponentRotation();
            this.offset = new BlockPos();
        }
    }
}
