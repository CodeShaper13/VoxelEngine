using VoxelEngine.Blocks;
using VoxelEngine.Level;
using VoxelEngine.Render;
using VoxelEngine.Util;

namespace Assets.VoxelEngine.Render {

    public class CachedRegion {

        //public IChunk[] cachedChunks;

        public IChunk centered;
        public IChunk up;
        public IChunk down;
        public IChunk north;
        public IChunk east;
        public IChunk west;
        public IChunk south;

        public CachedRegion(World world, Chunk chunk) {
            int x = chunk.chunkPos.x;
            int y = chunk.chunkPos.y;
            int z = chunk.chunkPos.z;

            /*
            this.cachedChunks = new IChunk[7] {
                chunk,
                world.getChunk(new ChunkPos(x + 1, y, z)),
                world.getChunk(new ChunkPos(x - 1, y, z)),
                world.getChunk(new ChunkPos(x, y + 1, z)),
                world.getChunk(new ChunkPos(x, y - 1, z)),
                world.getChunk(new ChunkPos(x, y, z + 1)),
                world.getChunk(new ChunkPos(x, y, z - 1)),
            };*/

            this.centered = chunk;
            this.east = this.func(world.getChunk(new ChunkPos(x + 1, y, z)));
            this.west = this.func(world.getChunk(new ChunkPos(x - 1, y, z)));
            this.up = this.func(world.getChunk(new ChunkPos(x, y + 1, z)));
            this.down = this.func(world.getChunk(new ChunkPos(x, y - 1, z)));
            this.north = this.func(world.getChunk(new ChunkPos(x, y, z + 1)));
            this.south = this.func(world.getChunk(new ChunkPos(x, y, z - 1)));

            /*
            // Replace null instances with an air chunk, to save expensive is null equality checks.
            for (int i = 0; i < 7; i++) {
                if(this.cachedChunks[i] == null) {
                    this.cachedChunks[i] = RenderManager.instance.airChunk;
                }
            }
            */
        }

        private IChunk func(IChunk c) {
            if(c == null) {
                return RenderManager.instance.airChunk;
            }
            return c;
        }

        public Block getBlock(int x, int y, int z) {
            /*
            if (x < 0) {
                x += Chunk.SIZE;
                return this.cachedChunks[2].getBlock(x, y, z);
            }
            else if (x >= Chunk.SIZE) {
                x -= Chunk.SIZE;
                return this.cachedChunks[1].getBlock(x, y, z);
            }
            else if (y < 0) {
                y += Chunk.SIZE;
                return this.cachedChunks[4].getBlock(x, y, z);
            }
            else if (y >= Chunk.SIZE) {
                y -= Chunk.SIZE;
                return this.cachedChunks[3].getBlock(x, y, z);
            }
            else if (z < 0) {
                z += Chunk.SIZE;
                return this.cachedChunks[6].getBlock(x, y, z);
            }
            else if (z >= Chunk.SIZE) {
                z -= Chunk.SIZE;
                return this.cachedChunks[5].getBlock(x, y, z);
            }
            else {
                return this.cachedChunks[0].getBlock(x, y, z);
            }
            */
            IChunk c = this.getChunk(x, y, z);
            x += (x < 0 ? Chunk.SIZE : x >= Chunk.SIZE ? -Chunk.SIZE : 0);
            y += (y < 0 ? Chunk.SIZE : y >= Chunk.SIZE ? -Chunk.SIZE : 0);
            z += (z < 0 ? Chunk.SIZE : z >= Chunk.SIZE ? -Chunk.SIZE : 0);
            return c.getBlock(x, y, z);
        }

        public int getLight(int x, int y, int z) {
            IChunk c = this.getChunk(x, y, z);
            x += (x < 0 ? Chunk.SIZE : x >= Chunk.SIZE ? -Chunk.SIZE : 0);
            y += (y < 0 ? Chunk.SIZE : y >= Chunk.SIZE ? -Chunk.SIZE : 0);
            z += (z < 0 ? Chunk.SIZE : z >= Chunk.SIZE ? -Chunk.SIZE : 0);
            return c.getLight(x, y, z);
        }

        public IChunk getChunk(int x, int y, int z) {
            if (x < 0) {
                x += Chunk.SIZE;
                return this.west;
            }
            else if (x >= Chunk.SIZE) {
                x -= Chunk.SIZE;
                return this.east;
            }
            else if (y < 0) {
                y += Chunk.SIZE;
                return this.down;
            }
            else if (y >= Chunk.SIZE) {
                y -= Chunk.SIZE;
                return this.up;
            }
            else if (z < 0) {
                z += Chunk.SIZE;
                return this.south;
            }
            else if (z >= Chunk.SIZE) {
                z -= Chunk.SIZE;
                return this.north;
            }
            else {
                return this.centered;
            }
            /*
            if (x < 0) {
                x += Chunk.SIZE;
                return this.cachedChunks[2];
            }
            else if (x >= Chunk.SIZE) {
                x -= Chunk.SIZE;
                return this.cachedChunks[1];
            }
            else if (y < 0) {
                y += Chunk.SIZE;
                return this.cachedChunks[4];
            }
            else if (y >= Chunk.SIZE) {
                y -= Chunk.SIZE;
                return this.cachedChunks[3];
            }
            else if (z < 0) {
                z += Chunk.SIZE;
                return this.cachedChunks[6];
            }
            else if (z >= Chunk.SIZE) {
                z -= Chunk.SIZE;
                return this.cachedChunks[5];
            }
            else {
                return this.cachedChunks[0];
            }
            */
        }
    }
}