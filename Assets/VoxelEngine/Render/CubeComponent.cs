using UnityEngine;
using VoxelEngine.Util;

namespace VoxelEngine.Render {

    /// <summary>
    /// NOTE:  CubeComponent's position's are not 0 based. <- whats this mesh, clarify
    /// </summary>
    public struct CubeComponent {

        /// <summary> Positive corner. </summary>
        public Vector3 pos;
        /// <summary> Negative corner. </summary>
        public Vector3 neg;
        /// <summary> Rotation of the cube. </summary>
        public ComponentRotation rotation;
        /// <summary> Offset of the cube. </summary>
        public Vector3 offset;

        public int index;

        /// <summary>
        /// Creates a CubeComponent with the default rotation and offset.
        /// </summary>
        public CubeComponent(int toX, int toY, int toZ, int fromX, int fromY, int fromZ) {
            this.neg = new Vector3(toX, toY, toZ);
            this.pos = new Vector3(fromX, fromY, fromZ);
            this.rotation = new ComponentRotation();
            this.offset = new Vector3();
            this.index = 0;
        }

        /// <summary>
        /// Creates a CubeComponent with the default rotation and offset.
        /// </summary>
        public CubeComponent(int toX, int toY, int toZ, int fromX, int fromY, int fromZ, int index) {
            this.neg = new Vector3(toX, toY, toZ);
            this.pos = new Vector3(fromX, fromY, fromZ);
            this.rotation = new ComponentRotation();
            this.offset = new Vector3();
            this.index = index;
        }

        public CubeComponent(int toX, int toY, int toZ, int fromX, int fromY, int fromZ, int rotX, int rotY, int rotZ) {
            this.neg = new Vector3(toX, toY, toZ);
            this.pos = new Vector3(fromX, fromY, fromZ);
            this.rotation = new ComponentRotation(rotX, rotY, rotZ);
            this.offset = new Vector3();
            this.index = 0;
        }

        public CubeComponent(int toX, int toY, int toZ, int fromX, int fromY, int fromZ, int rotX, int rotY, int rotZ, int index) {
            this.neg = new Vector3(toX, toY, toZ);
            this.pos = new Vector3(fromX, fromY, fromZ);
            this.rotation = new ComponentRotation(rotX, rotY, rotZ);
            this.offset = new Vector3();
            this.index = index;
        }

        public CubeComponent(int toX, int toY, int toZ, int fromX, int fromY, int fromZ, int rotX, int rotY, int rotZ, int offsetX, int offsetY, int offsetZ) {
            this.neg = new Vector3(toX, toY, toZ);
            this.pos = new Vector3(fromX, fromY, fromZ);
            this.rotation = new ComponentRotation(rotX, rotY, rotZ);
            this.offset = new Vector3(offsetX, offsetY, offsetZ);
            this.index = 0;
        }

        public CubeComponent(BlockPos to, BlockPos from, ComponentRotation rotation, Vector3 offset) {
            this.neg = to.toVector();
            this.pos = from.toVector();
            this.rotation = rotation;
            this.offset = offset;
            this.index = 0;
        }

        // Used by slabs, maybe just move this over to the slab and have it call a normal ctor?
        public CubeComponent(BlockPos orgin, int xRadius, int yRadius, int zRadius, int index) {
            this.neg = new Vector3(orgin.x - xRadius, orgin.y - yRadius, orgin.z - zRadius);
            this.pos = new Vector3(orgin.x + xRadius, orgin.y + yRadius, orgin.z + zRadius);
            this.rotation = new ComponentRotation();
            this.offset = new Vector3();
            this.index = index;
        }

        public CubeComponent setIndex(int index) {
            this.index = index;
            return this;
        }
    }
}
