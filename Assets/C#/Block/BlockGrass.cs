public class BlockGrass : Block {
    public override TexturePos getTexturePos(Direction direction, byte meta) {
        TexturePos tile = new TexturePos();
        if(direction == Direction.UP) {
            tile.x = 2;
            tile.y = 0;
        } else if(direction == Direction.DOWN) {
            tile.x = 1;
            tile.y = 0;
        } else {
            tile.x = 3;
            tile.y = 0;
        }
        return tile;
    }
}