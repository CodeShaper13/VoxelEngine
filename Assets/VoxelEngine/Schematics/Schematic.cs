using fNbt;
using VoxelEngine.Blocks;
using VoxelEngine.Level;
using VoxelEngine.Util;

namespace VoxelEngine.Schematics {

    public class Schematic {
        public string schematicName;
        public Block[] blocks;
        public byte[] metaData;
        public int sizeX;
        public int sizeY;
        public int sizeZ;

        public Schematic(Chunk chunk) {
            this.blocks = new Block[Chunk.BLOCK_COUNT];
            this.metaData = new byte[Chunk.BLOCK_COUNT];
            this.sizeX = Chunk.SIZE;
            this.sizeY = Chunk.SIZE;
            this.sizeZ = Chunk.SIZE;
            for (int i = 0; i < Chunk.BLOCK_COUNT; i++) {
                this.blocks[i] = chunk.blocks[i];
                this.metaData[i] = chunk.metaData[i];
            }
            this.schematicName = chunk.chunkPos.ToString();
        }

        public Schematic(World world, BlockPos orgin, int sizeX, int sizeY, int sizeZ) {
            this.sizeX = sizeX;
            this.sizeY = sizeY;
            this.sizeZ = sizeZ;
            int i = this.sizeX * this.sizeY * this.sizeZ;
            this.blocks = new Block[i];
            this.metaData = new byte[i];
            i = 0;
            for (int x = orgin.x; x < this.sizeX; x++) {
                for (int y = orgin.y; y < this.sizeY; y++) {
                    for (int z = orgin.z; z < this.sizeZ; z++) {
                        this.blocks[i] = world.getBlock(x, y, z);
                        this.metaData[i] = world.getMeta(x, y, z);
                        i++;
                    }
                }
            }
        }

        public Block getBlock(int x, int y, int z) {
            return this.blocks[x + this.sizeX * (z + sizeZ * y)];
        }

        public byte getMeta(int x, int y, int z) {
            return this.metaData[x + this.sizeX * (z + sizeZ * y)];
        }

        public NbtCompound writeToNbt(NbtCompound tag) {
            tag.Add(new NbtString("name", this.schematicName));
            tag.Add(new NbtInt("sizeX", this.sizeX));
            tag.Add(new NbtInt("sizeY", this.sizeY));
            tag.Add(new NbtInt("sizeZ", this.sizeZ));
            byte[] blockBytes = new byte[Chunk.BLOCK_COUNT];
            for (int i = 0; i < Chunk.BLOCK_COUNT; i++) {
                blockBytes[i] = this.blocks[i].id;
            }
            tag.Add(new NbtByteArray("blocks", blockBytes));
            tag.Add(new NbtByteArray("meta", this.metaData));
            return tag;
        }

        public void readFromNbt(NbtCompound tag) {
            this.schematicName = tag.Get<NbtString>("name").StringValue;
            this.sizeX = tag.Get<NbtInt>("sizeX").IntValue;
            this.sizeY = tag.Get<NbtInt>("sizeY").IntValue;
            this.sizeZ = tag.Get<NbtInt>("sizeZ").IntValue;
            byte[] blockBytes = tag.Get<NbtByteArray>("blocks").ByteArrayValue;
            for (int i = 0; i < Chunk.BLOCK_COUNT; i++) {
                this.blocks[i] = Block.getBlock(blockBytes[i]);
            }
            this.metaData = tag.Get<NbtByteArray>("meta").ByteArrayValue;
        }
    }
}
