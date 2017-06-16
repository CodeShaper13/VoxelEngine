using UnityEngine;
using VoxelEngine.Level;
using VoxelEngine.Util;

namespace VoxelEngine.ChunkLoaders {
    
    public struct NewChunkInstructions {

        public ChunkPos chunkPos;
        public bool isReadOnly;

        public NewChunkInstructions(int x, int y, int z, bool isReadOnly) {
            this.chunkPos = new ChunkPos(x, y, z);
            this.isReadOnly = isReadOnly;
        }

        /// <summary>
        /// Returns the position for the new chunk in world space.
        /// </summary>
        public Vector3 toChunkVector() {
            return new Vector3(this.chunkPos.x * Chunk.SIZE, this.chunkPos.y * Chunk.SIZE, this.chunkPos.z * Chunk.SIZE);
        }

        public override bool Equals(object obj) {
            return this.chunkPos.Equals(obj);
        }

        public override int GetHashCode() {
            return this.chunkPos.GetHashCode();
        }
    }
}
