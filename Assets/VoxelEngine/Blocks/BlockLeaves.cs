using UnityEngine;
using VoxelEngine.Containers;
using VoxelEngine.Items;
using VoxelEngine.Level;
using VoxelEngine.Util;

namespace VoxelEngine.Blocks {

    public class BlockLeaves : Block {

        public BlockLeaves(int id) : base(id) {
            this.setTexture(0, 1);
            this.setTransparent();
        }

        public override ItemStack[] getDrops(World world, BlockPos pos, int meta, ItemTool brokenWith) {
            int i = Random.Range(0, 40);
            if(i == 0) {
                return new ItemStack[] { new ItemStack(Item.apple) };
            } else if(i == 1) {
                return new ItemStack[] { new ItemStack(Item.stick) };
            }
            return null;
        }
    }
}
