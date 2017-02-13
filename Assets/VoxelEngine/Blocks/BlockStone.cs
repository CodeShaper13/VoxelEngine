using UnityEngine;
using VoxelEngine.Containers;
using VoxelEngine.Items;
using VoxelEngine.Level;
using VoxelEngine.Util;

namespace VoxelEngine.Blocks {

    public class BlockStone : Block {

        public TexturePos[] texturePosArray;

        public BlockStone(byte id) : base(id) {
            this.texturePosArray = new TexturePos[] {new TexturePos(0, 4), new TexturePos(1, 4), new TexturePos(2, 4), new TexturePos(3, 4), new TexturePos(4, 4) };
        }

        public override TexturePos getTexturePos(Direction direction, byte meta) {
            return this.texturePosArray[meta];
        }

        public override string getName(byte meta) {
            switch (meta) {
                case 0:
                    return "Stone 0";
                case 1:
                    return "Stone 1";
                case 2:
                    return "Stone 2";
                case 3:
                    return "Stone 3";
                case 4:
                    return "Stone 4";
            }
            return base.getName(meta);
        }

        public override ItemStack[] getDrops(World world, BlockPos pos, byte meta, ItemTool brokenWith) {
            ItemStack stack;
            if (brokenWith != null && brokenWith.toolType == ItemTool.ToolType.PICKAXE) {
                stack = new ItemStack(this.asItem(), meta);
            }
            else {
                stack = new ItemStack(Item.pebble);
            }
            return new ItemStack[] { stack };
        }
    }
}
