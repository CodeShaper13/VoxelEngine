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

            /*
            bool inCrackChunk = chunk.chunkPos.y % 2 != 0;
            int closerToOrgin = this.stoneLayers.getStone(Mathf.FloorToInt(chunk.chunkPos.y / 2));
            int fartherFromOrgin = this.stoneLayers.getStone(Mathf.FloorToInt(chunk.chunkPos.y / 2 + (chunk.chunkPos.y < 0 ? -1 : 1)));

            //Iterate through all blocks in the chunk, setting them to the correct stone
            for (int x = 0; x < Chunk.SIZE; x++) {
                for (int z = 0; z < Chunk.SIZE; z++) {
                    for (int y = 0; y < Chunk.SIZE; y++) {
                        // Removed layered stone because of expensive noise lookup and lack of textures
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

                        chunk.setBlock(x, y, z, Block.stone);
                    }
                }
            }
            */

            for(int i = 0; i < Chunk.BLOCK_COUNT; i++) {
                chunk.blocks[i] = Block.stone;
            }
            chunk.isDirty = true;

            // Ores.
            this.generateOrePatch(chunk, 2, Block.gravel, 7, rnd);

            for (int i = 0; i < 3; i++) {
                this.generateOrePatch(chunk, 2, Block.coalOre, 6, rnd);
                this.generateOrePatch(chunk, 2, Block.dirt, 7, rnd);
            }

            this.generateOrePatch(chunk, 2, Block.ironOre, 4, rnd);
            this.generateOrePatch(chunk, 1, Block.rubyOre, 3, rnd);

            StructureMineshaft shaft;
            PieceBase piece;
            for(int i = 0; i < this.mineshaftList.Count; i++) {
                shaft = this.mineshaftList[i];
                for(int j = 0; j < shaft.pieces.Count; j++) {
                    piece = shaft.pieces[j];
                    if(chunk.chunkBounds.Intersects(piece.pieceBounds)) {
                        piece.carvePiece(chunk, rnd);
                    }
                }
            }
        }

        /// <summary>
        /// Generates and places a random patch of ore.
        /// </summary>
        private void generateOrePatch(Chunk c, int size, Block block, int chance, System.Random rnd) {
            int x = rnd.Next(0, Chunk.SIZE);
            int y = rnd.Next(0, Chunk.SIZE);
            int z = rnd.Next(0, Chunk.SIZE);
            int x1, y1, z1;
            for (int i = -size; i <= size; i++) {
                for(int j = -size; j <= size; j++) {
                    for(int k = -size; k <= size; k++) {
                        if(rnd.Next(0, 10) < chance) {
                            x1 = x + i;
                            y1 = y + j;
                            z1 = z + k;
                            if(x1 >= 0 && y1 >= 0 && z1 >= 0 && x1 < Chunk.SIZE && y1 < Chunk.SIZE && z1 < Chunk.SIZE) {
                                c.setBlock(x1, y1, z1, block);
                            }
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
