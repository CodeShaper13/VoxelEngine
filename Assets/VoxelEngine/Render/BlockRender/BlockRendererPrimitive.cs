using UnityEngine;
using VoxelEngine.Blocks;
using VoxelEngine.Util;

namespace VoxelEngine.Render.BlockRender {

    public abstract class BlockRendererPrimitive : BlockRenderer {

        public MeshData meshData;
        public Block block;
        public byte meta;

        public BlockRendererPrimitive() : base(true) { }

        //Returns the UV's to use for the passed direction
        public virtual Vector2[] getUVs(Block block, byte meta, Direction direction) {
            TexturePos tilePos = block.getTexturePos(direction, meta);
            float x = TexturePos.BLOCK_SIZE * tilePos.x;
            float y = TexturePos.BLOCK_SIZE * tilePos.y;
            Vector2[] UVs = new Vector2[4] {
            new Vector2(x, y),
            new Vector2(x, y + TexturePos.BLOCK_SIZE),
            new Vector2(x + TexturePos.BLOCK_SIZE, y + TexturePos.BLOCK_SIZE),
            new Vector2(x + TexturePos.BLOCK_SIZE, y)};
            return UVs;
        }
    }
}
