using UnityEngine;
using VoxelEngine.Containers;
using VoxelEngine.Entities;
using VoxelEngine.Items;
using VoxelEngine.Level;
using VoxelEngine.Render;
using VoxelEngine.Util;

namespace VoxelEngine.Blocks {

    public class BlockTnt : Block, IExplosiveObject {

        public BlockTnt(int id) : base(id) {
            this.setTransparent();
            this.setRenderer(RenderManager.TNT);
        }

        public override bool onRightClick(World world, EntityPlayer player, ItemStack heldStack, BlockPos pos, int meta, Direction clickedFace, Vector3 clickedPos) {
            if(heldStack.item == Block.torch.asItem()) {
                world.setBlock(pos, Block.air, -1, false, false);
                world.makeExplosion(this, pos.toVector());
                return true;
            }
            return false;
        }

        public override TexturePos getTexturePos(Direction direction, int meta) {
            if(direction.axis == EnumAxis.Y) {
                return new TexturePos(8, 1);
            } else {
                return new TexturePos(7, 1);
            }
        }

        public override bool acceptsWire(Direction directionOfWire, int meta) {
            return true;
        }

        public float getExplosionSize() {
            return 4.25f;
        }
    }
}
