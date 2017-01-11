using System.Collections.Generic;
using UnityEngine;
using VoxelEngine.Blocks;
using VoxelEngine.Level;

namespace VoxelEngine.Generation {

    public class OrePlacer {
        public static OreType RUBY = new OreType(Block.rubyOre, 0.5f, 2, 2, 2, 3, 3, 3);
    }

    public class OreType {

        public Block block;
        public float chance;
        public List<Box> boxes;

        public OreType(Block block, float chance, int minX, int minY, int minZ, int maxX, int maxY, int maxZ) {
            this.block = block;
            this.chance = chance;
            this.boxes = new List<Box>();
            for(int x = minX; x < maxX; x++) {
                for (int y = minY; y < maxY; y++) {
                    for (int z = minZ; z < maxZ; z++) {
                        this.boxes.Add(new Box(x, y, z));
                    }
                }
            }
        }

        public void generate(Chunk c, int x, int y, int z) {
            Box b = this.boxes[Random.Range(0, this.boxes.Count)];
            for(int x1 = -b.sizeX / 2; x1 < b.sizeX + 1; x1++) {
                for (int y1 = -b.sizeY / 2; y1 < b.sizeY + 1; y1++) {
                    for (int z1 = -b.sizeZ / 2; z1 < b.sizeZ + 1; z1++) {
                        if(Random.value < this.chance) {
                            int x2 = x + x1;
                            int y2 = y + y1;
                            int z2 = z + z1;
                            if(x2 >= 0 && y2 >= 0 && z2 >= 0 && x2 < 16 && y2 < 16 && z2 < 16) {  
                                c.setBlock(x2, y2, z2, this.block);
                            }
                        }
                    }
                }
            }
        }
    }

    public class Box {

        public int sizeX;
        public int sizeY;
        public int sizeZ;

        public Box(int x, int y, int z) {
            this.sizeX = x;
            this.sizeY = y;
            this.sizeZ = z;
        }
    }
}
