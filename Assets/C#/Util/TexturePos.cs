using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public struct TexturePos {
    public const float BLOCK_SIZE = 0.25f; //size of the texture sheet
    public const float ITEM_SIZE = 0.25f;

    public int x;
    public int y;

    public TexturePos(int x, int y) {
        this.x = x;
        this.y = y;
    }
}

