using UnityEngine;
using VoxelEngine.Blocks;
using VoxelEngine.Util;

namespace VoxelEngine.Render.Blocks {

    public abstract class BlockModel {

        public Block block;
        public byte meta;
        public MeshData meshData;

        public abstract MeshData renderBlock(Block block, byte meta, MeshData meshData, int x, int y, int z, bool[] renderFace);

        //Adds a cube to the model
        public void addCube(Vector3 p1, Vector3 p2) {
            //this.addFace(new Vector3(p1.x, p1.y, p1.z), new Vector3(p2.x, p1.y, p2.z), Direction.DOWN);
            //this.addFace(new Vector3(p1.x, p2.y, p1.z), new Vector3(p2.x, p2.y, p2.x), Direction.UP);
            
            //this.addFace(new Vector3(), new Vector3(), Direction.UP);
            //this.addFace(new Vector3(), new Vector3(), Direction.UP);
            //this.addFace(new Vector3(), new Vector3(), Direction.UP);
            //this.addFace(new Vector3(), new Vector3(), Direction.UP);
        }

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
