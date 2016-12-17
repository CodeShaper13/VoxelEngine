using UnityEngine;
using System.Collections.Generic;

public class ChunkLoaderInfinite : ChunkLoader {

    protected override void unloadChunks(ChunkPos occupiedChunkPos) {
        //BROKEN!!!  Not update when we replaced blokc pos with chunk pos
        occupiedChunkPos.x *= 16;
        occupiedChunkPos.y *= 16;
        occupiedChunkPos.z *= 16;

        List<BlockPos> removals = new List<BlockPos>();
        foreach (Chunk c in this.world.loadedChunks.Values) {
            BlockPos p = c.pos;
            if (this.toFarOnAxis(occupiedChunkPos.x, p.x) || this.toFarOnAxis(occupiedChunkPos.y, p.y) || this.toFarOnAxis(occupiedChunkPos.z, p.z)) {
                removals.Add(c.pos);
            }
        }
        foreach (BlockPos p in removals) {
            this.world.unloadChunk(p.toChunkPos());
        }
    }

    protected override void loadChunks(ChunkPos occupiedChunkPos) {
        //BROKEN!!!  Not update when we replaced blokc pos with chunk pos
        occupiedChunkPos.x *= 16;
        occupiedChunkPos.y *= 16;
        occupiedChunkPos.z *= 16;

        //Add all the chunks close to the player to the list of chunks to generate.
        for (int i = -this.loadDistance; i < this.loadDistance + 1; i++) {
            for (int j = -this.loadDistance; j < this.loadDistance + 1; j++) {
                for (int k = -this.loadDistance; k < this.loadDistance + 1; k++) {
                    int x = i * Chunk.SIZE + occupiedChunkPos.x;
                    int y = k * Chunk.SIZE + occupiedChunkPos.y;
                    int z = j * Chunk.SIZE + occupiedChunkPos.z;

                    Chunk c = world.getChunk(x, y, z);
                    BlockPos p = new BlockPos(x, y, z);
                    //if (c == null && !this.buildList.Contains(p)) {
                    //    this.buildList.Add(p);
                    //}
                }
            }
        }
    }
}