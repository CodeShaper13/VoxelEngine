using UnityEngine;
using VoxelEngine.Blocks;

namespace VoxelEngine.Render.BlockRender {

    public abstract class BlockRenderer {

        public static BlockRenderer CUBE = new BlockRendererPrimitiveCube();
        public static BlockRenderer CROSS = new BlockRendererPrimitiveCross();
        public static BlockRenderer RAIL = new BlockRendererPrimitiveRail();
        public static BlockRenderer FENCE = new BlockRendererPrimitiveFence();
        public static BlockRenderer LADDER = new BlockRendererPrimitiveLadder();
        public static BlockRenderer LANTERN = new BlockRendererMesh(References.list.lanternPrefab).setRenderInWorld(false);
        public static BlockRenderer TORCH = new BlockRendererMesh(References.list.torchPrefab).setRenderInWorld(false);
        public static BlockRenderer MUSHROOM = new BlockRendererMesh(References.list.mushroomPrefab).useRandomMirror().setOffsetVector(new Vector3(0, -0.5f, 0)).useColliderComponent();
        public static BlockRenderer CHEST = new BlockRendererMesh(References.list.chestPrefab).setRenderInWorld(false);

        /// <summary> False will make blocks not be baked into the world. </summary>
        public bool bakeIntoChunks = true;
        /// <summary> If true, adjacent light level will be looked up and passes into the MeshData. </summary>
        public EnumLightLookup lookupAdjacentLight = EnumLightLookup.CURRENT;

        public abstract MeshBuilder renderBlock(Block b, byte meta, MeshBuilder meshData, int x, int y, int z, bool[] renderFace, Block[] surroundingBlocks);

        public BlockRenderer setRenderInWorld(bool flag) {
            this.bakeIntoChunks = flag;
            return this;
        }
    }
}
