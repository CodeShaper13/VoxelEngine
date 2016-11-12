using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class Save
{
    public Dictionary<BlockPos, Block> blocks = new Dictionary<BlockPos, Block>();

    public Save(Chunk chunk) {
        for (int x = 0; x < Chunk.SIZE; x++) {
            for (int y = 0; y < Chunk.SIZE; y++) {
                for (int z = 0; z < Chunk.SIZE; z++) {
                    //if (!chunk.blocks[x, y, z].changed)
                    //    continue;
                    BlockPos pos = new BlockPos(x, y, z);
                    blocks.Add(pos, chunk.getBlock(x, y, z)); // .blocks[x, y, z]);
                }
            }
        }
    }
}