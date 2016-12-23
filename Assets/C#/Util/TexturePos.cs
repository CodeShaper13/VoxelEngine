//Class for the position of a texture on a sprite sheet.  Used for blocks and items
public struct TexturePos {
    public const float BLOCK_SIZE = 0.0625f; //size of the texture sheet
    public const float ITEM_SIZE = 0.0625f;

    public int x;
    public int y;

    public TexturePos(int x, int y) {
        this.x = x;
        this.y = y;
    }
}

