using UnityEngine;
using System.Collections;

public static class EditTerrain {

    public static bool SetBlock(RaycastHit hit, Block block, bool adjacent = false) {
        Chunk chunk = hit.collider.GetComponent<Chunk>();
        if (chunk == null) {
            return false;
        }

        BlockPos pos = BlockPos.fromRaycast(hit, adjacent);

        chunk.world.setBlock(pos.x, pos.y, pos.z, block);

        return true;
    }

    public static Block GetBlock(RaycastHit hit, bool adjacent = false)
    {
        Chunk chunk = hit.collider.GetComponent<Chunk>();
        if (chunk == null)
            return null;

        BlockPos pos = BlockPos.fromRaycast(hit, adjacent);

        Block block = chunk.world.getBlock(pos.x, pos.y, pos.z);

        return block;
    }
}