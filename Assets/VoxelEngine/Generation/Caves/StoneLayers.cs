using fNbt;
using System;
using System.Collections.Generic;

namespace VoxelEngine.Generation.Caves {

    public class StoneLayers {

        private Random rnd;
        private byte zeroMeta;
        private List<byte> positiveMeta;
        private List<byte> negativeMeta;

        public StoneLayers(int seed) {
            this.rnd = new Random(seed);
            this.zeroMeta = this.getNewLayer(255);
            this.positiveMeta = new List<byte>() { 1, 2, 3, 4 };
            this.negativeMeta = new List<byte>() { 1, 2, 3, 4 };
        }

        public byte getStone(int y) {
            //is y always 0 or greater?
            if(y == 0) {
                return this.zeroMeta;
            } else if(y > 0) {
                int i = y - 1;
                return this.getLayer(this.positiveMeta, i);
            } else {
                int i = (y*-1) - 1;
                return this.getLayer(this.negativeMeta, i);
            }
        }

        public NbtCompound writeToNbt(NbtCompound tag) {
            tag.Add(new NbtByte("zeroMeta", this.zeroMeta));
            tag.Add(new NbtByteArray("positiveLayers", this.positiveMeta.ToArray()));
            tag.Add(new NbtByteArray("negativeLayers", this.negativeMeta.ToArray()));
            return tag;
        }

        public void readFromNbt(NbtCompound tag) {
            this.zeroMeta = tag.Get<NbtByte>("zeroMeta").ByteValue;
            this.positiveMeta = new List<byte>(tag.Get<NbtByteArray>("positiveLayers").ByteArrayValue);
            this.negativeMeta = new List<byte>(tag.Get<NbtByteArray>("negativeLayers").ByteArrayValue);
        }

        private byte getLayer(List<byte> list, int index) {
            if (index >= list.Count) {
                this.expandTo(list, index);
            }
            return list[index];
        }

        private List<byte> expandTo(List<byte> list, int targetSize) {
            while(list.Count < targetSize + 1) {
                list.Add(this.getNewLayer(list.Count == 0 ? this.zeroMeta : list[list.Count - 1]));
            }
            return list;
        }

        private byte getNewLayer(byte previousByte) {
            for(int i = 0; i < 100; i++) { //Try to get a different value than the previous up to 100 times
                byte b = (byte)this.rnd.Next(0, 5);
                if(b != previousByte) {
                    return b;
                }
            }
            return (byte)this.rnd.Next(0, 5);
        }
    }
}
