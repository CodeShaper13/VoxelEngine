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
            //for(int i = 0; i < this.mineshaftList.Count; i++) {
            //    this.mineshaftList[i].debugDisplay();
            //}
        }

        public override void generateChunk(Chunk chunk) {
            this.rnd = new System.Random(this.seed + chunk.chunkPos.GetHashCode());

            bool inCrackChunk = chunk.chunkPos.y % 2 != 0;
            byte closerToOrgin = this.stoneLayers.getStone(Mathf.FloorToInt(chunk.chunkPos.y / 2));
            byte fartherFromOrgin = this.stoneLayers.getStone(Mathf.FloorToInt(chunk.chunkPos.y / 2 + (chunk.chunkPos.y < 0 ? -1 : 1)));
            Block block;
            byte meta;

            float noise;

            //Iterate through all blocks in the chunk, setting them to the correct stone
            for (int x = 0; x < Chunk.SIZE; x++) {
                for (int z = 0; z < Chunk.SIZE; z++) {
                    for (int y = 0; y < Chunk.SIZE; y++) {
                        meta = (chunk.chunkPos.y < 0 && inCrackChunk) ? fartherFromOrgin : closerToOrgin;

                        noise = Noise.Generate(chunk.pos.x + x, chunk.pos.y, chunk.pos.z + z);

                        if (inCrackChunk) {
                            if (y > (Chunk.SIZE / 2) + ((noise * 0.05f) * 2)) {
                                meta = chunk.chunkPos.y > 0 ? fartherFromOrgin : closerToOrgin;
                            }
                        }

                        if(noise * 0.05f >= 0.85f && rnd.Next(0, 4) < 3) {
                            block = Block.coalOre;
                        } else {
                            block = Block.stone;
                        }

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

        /// <summary>
        /// Gets noise and multiplies it by a scale.
        /// </summary>
        private float getNoise(int x, int y, int z, float scale) {
            return Noise.Generate(x * scale, y * scale, z * scale);
        }
    }
}
