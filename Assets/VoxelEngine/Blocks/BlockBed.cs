using UnityEngine;
using VoxelEngine.Containers;
using VoxelEngine.Items;
using VoxelEngine.Level;
using VoxelEngine.Render;
using VoxelEngine.Util;

namespace VoxelEngine.Blocks {

    public class BlockBed : Block {

        public BlockBed(int id) : base(id) {
            this.setRenderer(RenderManager.BED);
            this.setTransparent();
            this.setType(EnumBlockType.WOOD);
        }

        public override ItemStack[] getDrops(World world, BlockPos pos, int meta, ItemTool brokenWith) {
            return base.getDrops(world, pos, 0, brokenWith);
        }

        public override void onNeighborChange(World world, BlockPos pos, int meta, Direction neighborDir) {
            base.onNeighborChange(world, pos, meta, neighborDir);
        }

        public override void onPlace(World world, BlockPos pos, int meta) {
            base.onPlace(world, pos, meta);
        }

        public override int adjustMetaOnPlace(World world, BlockPos pos, int meta, Direction clickedDirNormal, Vector3 angle) {
            return base.adjustMetaOnPlace(world, pos, meta, clickedDirNormal, angle);
        }

        public override TexturePos getTexturePos(Direction direction, int meta) {
            return new TexturePos();
        }
    }
}
