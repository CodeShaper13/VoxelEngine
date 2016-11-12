using UnityEngine;
using System.Collections;
using System;

public class BlockGrass : Block {
    public override TexturePos getTexturePos(Direction direction) {
        TexturePos tile = new TexturePos();
        switch (direction) {
            case Direction.up:
                tile.x = 2;
                tile.y = 0;
                return tile;
            case Direction.down:
                tile.x = 1;
                tile.y = 0;
                return tile;
        }
        tile.x = 3;
        tile.y = 0;

        return tile;
    }
}