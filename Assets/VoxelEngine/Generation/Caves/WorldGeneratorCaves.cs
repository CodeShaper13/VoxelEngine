using SimplexNoise;
using System.Collections.Generic;
using UnityEngine;
using VoxelEngine.Blocks;
using VoxelEngine.Level;
using fNbt;
using VoxelEngine.Generation.Caves.Structure.Mineshaft;

namespace VoxelEngine.Generation.Caves {

    public class WorldGeneratorCaves : WorldGeneratorBase {

        private System.Random rnd;

        private StoneLayers stoneLayers;
        private List<StructureMineshaft> mineshaftList;


        public WorldGeneratorCaves(World world, int seed) : base(world, seed) {
            this.stoneLayers = new StoneLayers(seed);

            this.mineshaftList = new List<StructureMineshaft>();
        }

        public override bool generateLevelData() {
            this.mineshaftList.Add(new StructureMineshaft(Vector3.zero, this.seed));

            return true;
        }

        public override Vector3 getSpawnPoint() {
            return new Vector3(0, 3, 0);
        }

        public override NbtCompound writeToNbt(NbtCompound tag) {
            base.writeToNbt(tag);

            tag.Add(this.stoneLayers.writeToNbt(new NbtCompound("stoneLayers")));

            NbtList tag1 = new NbtList("mineshafts", NbtTagType.Compound);
            foreach (StructureMineshaft m in this.mineshaftList) {
                tag1.Add(m.writeToNbt(new NbtCompound()));
            }

            tag.Add(tag1);

            return tag;
        }

        public override void readFromNbt(NbtCompound tag) {
            base.readFromNbt(tag);

            this.stoneLayers.readFromNbt(tag.Get<NbtCompound>("stoneLayers"));

            foreach (NbtCompound compound in tag.Get<NbtList>("mineshafts")) {
                StructureMineshaft s = new StructureMineshaft();
                s.readFromNbt(compound);
                this.mineshaftList.Add(s);
            }
        }

        public void debugDisplay() {
            for(int i = 0; i < this.mineshaftList.Count; i++) {
                this.mineshaftList[i].debugDisplay();
            }
        }

        public override void generateChunk(Chunk chunk) {
            this.rnd = new System.Random(this.seed + chunk.chunkPos.GetHashCode());

            bool inCrackChunk = chunk.chunkPos.y % 2 != 0;
            byte closerToOrgin = this.stoneLayers.getStone(Mathf.FloorToInt(chunk.chunkPos.y / 2));
            byte fartherFromOrgin = this.stoneLayers.getStone(Mathf.FloorToInt(chunk.chunkPos.y / 2 + (chunk.chunkPos.y < 0 ? -1 : 1)));
            Block block;
            byte meta;
            float crackNoise;

            //Iterate through all blocks in the chunk, setting them to the correct stone
            for (int x = 0; x < Chunk.SIZE; x++) {
                for (int z = 0; z < Chunk.SIZE; z++) {
                    for (int y = 0; y < Chunk.SIZE; y++) {
                        meta = (chunk.chunkPos.y < 0 && inCrackChunk) ? fartherFromOrgin : closerToOrgin;

                        if (inCrackChunk) {
                            crackNoise = getNoise(chunk.pos.x + x, chunk.pos.y, chunk.pos.z + z, 0.05f);
                            if (y > (Chunk.SIZE / 2) + (crackNoise * 2)) {
                                meta = chunk.chunkPos.y > 0 ? fartherFromOrgin : closerToOrgin;
                            }
                        }

                        if (this.getNoise(chunk.pos.x + x, chunk.pos.y + y, chunk.pos.z + z, 0.05f) >= 0.85f && rnd.Next(0, 4) < 3) {
                            block = Block.coalOre;
                        } else {
                            block = Block.stone;
                        }

                        //if (this.getNoise(c.pos.x + x, (c.pos.y + y) * 2, c.pos.z + z, 0.01f) >= 0.85f) {
                        //    block = Block.lava;
                        //}

                        chunk.setBlock(x, y, z, block);
                        chunk.setMeta(x, y, z, 0 /*meta*/);
                    }
                }
            }

            chunk.setBlock(rnd.Next(0, Chunk.SIZE), rnd.Next(0, Chunk.SIZE), rnd.Next(0, Chunk.SIZE), Block.uraniumOre);

            this.generateRubyPatch(chunk, rnd.Next(0, Chunk.SIZE - 2), rnd.Next(0, Chunk.SIZE - 2), rnd.Next(0, Chunk.SIZE - 2), rnd);

            foreach (StructureMineshaft m in this.mineshaftList) {
                foreach(PieceBase p in m.pieces) {
                    if(chunk.chunkBounds.Intersects(p.pieceBounds)) {
                        p.carvePiece(chunk, rnd);
                    }
                }
            }
        }

        private void generateRubyPatch(Chunk c, int x, int y, int z, System.Random rnd) {
            for(int i = 0; i < 3; i++) {
                for(int j = 0; j < 3; j++) {
                    for(int k = 0; k < 3; k++) {
                        if(rnd.Next(0, 20) > 7) {
                            c.setBlock(x + i, y + j, z + k, Block.rubyOre);
                        }
                    }
                }
            }
        }

        //Helper function for getting noise
        public float getNoise(int x, int y, int z, float scale) {
            return Noise.Generate(x * scale, y * scale, z * scale);
        }





        private void generateShaft(Chunk chunk) {
            for (int x = 6; x < 11; x++) {
                for (int z = 6; z < 11; z++) {
                    if (x > 6 && x < 10 && z > 6 && z < 10) {
                        this.setColumn(chunk, x, z, x == 8 && z == 8 ? Block.cable : Block.air);
                    }
                    else {
                        this.setColumn(chunk, x, z, Block.mossyBrick);
                    }
                }
            }
        }

        private void setColumn(Chunk chunk, int x, int z, Block block) {
            for (int y = 0; y < Chunk.SIZE; y++) {
                chunk.setBlock(x, y, z, block);
            }
        }

        private void generateStartRoom(Chunk chunk) {
            for (int x = 0; x < Chunk.SIZE; x++) {
                for (int z = 0; z < Chunk.SIZE; z++) {
                    for (int y = 0; y < Chunk.SIZE; y++) {
                        chunk.setBlock(x, y, z, Block.air);
                    }
                }
            }

            this.generateShaft(chunk);
            for (int x = 2; x < 14; x++) {
                for (int z = 10; z < Chunk.SIZE; z++) {
                    chunk.setBlock(x, 0, z, Block.mossyBrick);
                }
            }
        }

        public int GetNoise(int x, int y, int z, float scale, int max) {
            return Mathf.FloorToInt((Noise.Generate(x * scale, y * scale, z * scale) + 1f) * (max / 2f));
        }
    }
}
