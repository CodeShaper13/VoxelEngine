using UnityEngine;
using VoxelEngine.Blocks;

namespace VoxelEngine.Render.BlockRender {

    public abstract class BlockRenderer {

        public static BlockRenderer CUBE = new BlockRendererPrimitiveCube();
        public static BlockRenderer CROSS = new BlockRendererPrimitiveCross();
        public static BlockRenderer RAIL = new BlockRendererPrimitiveRail();
        public static BlockRenderer FENCE = new BlockRendererPrimitiveFence();
        public static BlockRenderer LANTERN = new BlockRendererMesh(References.list.lanternPrefab).setRenderInWorld(false);
        public static BlockRenderer TORCH = new BlockRendererMesh(References.list.torchPrefab).setRenderInWorld(false);
        public static BlockRenderer MUSHROOM = new BlockRendererMesh(References.list.mushroomPrefab).useRandomMirror().setOffsetVector(new Vector3(0, -0.5f, 0)).useColliderComponent();
        public static BlockRenderer CHEST = new BlockRendererMesh(References.list.chestPrefab).setRenderInWorld(false);
        
        // false will make blocks not be baked into the world
        public bool renderInWorld;

        public abstract MeshData renderBlock(Block b, byte meta, MeshData meshData, int x, int y, int z, bool[] renderFace, Block[] surroundingBlocks);

        public BlockRenderer setRenderInWorld(bool flag) {
            this.renderInWorld = flag;
            return this;
        }
    }
}
