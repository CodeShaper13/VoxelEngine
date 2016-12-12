using System;
using UnityEngine;

public class BlockGrass : Block {

    //Never gets called, seems to be realy slow...
    public override void onRandomTick(World world, BlockPos pos, byte meta) {
        if(world.getBlock(pos + BlockPos.up).isSolid) {
            world.setBlock(pos, Block.dirt);
        } else {
            Debug.Log("!");
            foreach (Direction d in Direction.xzPlane) {
                BlockPos pos1 = pos + d.direction;
                Debug.Log(world.getBlock(pos1));
                //if (world.getBlock(pos1) == Block.dirt) {
                world.setBlock(pos1, Block.wood);
                //}
            }
        }
    }

    public override TexturePos getTexturePos(Direction direction, byte meta) {
        TexturePos tile = new TexturePos(0, 0);
        if(direction == Direction.UP) {
            tile.x = 2;
        } else if(direction == Direction.DOWN) {
            tile.x = 1;
        } else {
            tile.x = 3;
        }
        return tile;
    }
}