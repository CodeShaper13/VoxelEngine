using System;
using VoxelEngine.Blocks;
using VoxelEngine.Level;

namespace VoxelEngine.Schematics {

    [Serializable]
    public class Schematic {
        public string name;
        public Block[] blocks;
        public byte[] metaData;

        public Schematic(Chunk chunk) {
            this.blocks = new Block[Chunk.BLOCK_COUNT];
            this.metaData = new byte[Chunk.BLOCK_COUNT];
            for (int i = 0; i < Chunk.BLOCK_COUNT; i++) {
                this.blocks[i] = chunk.blocks[i];
                this.metaData[i] = chunk.metaData[i];
            }
            this.name = chunk.chunkPos.ToString();
        }
    }
}
