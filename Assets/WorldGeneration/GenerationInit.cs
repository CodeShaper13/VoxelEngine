using fNbt;
using System.Collections.Generic;
using UnityEngine;
using VoxelEngine.Generation.Caves.Structure.Mineshaft;
using WorldGeneration.Cave;

namespace WorldGeneration {

    public class GenerationInit : MonoBehaviour {

        public static GenerationInit singleton;

        public bool drawNorth = true;
        public bool drawEast = true;
        public bool drawSouth = true;
        public bool drawWest = true;
        public bool drawUp = true;
        public bool drawDown = true;
        public int i = 0;

        public Region region;

        public List<StructureMineshaft> mineshaftList;
        public List<CaveSegment> caveSegements;
        public List<CaveDrawer> caveDrawers;

        void Awake() {
            GenerationInit.singleton = this;
            this.region = new Region(this.transform.position);

            //this.mineshaftList = new List<StructureMineshaft>();
            //this.mineshaftList.Add(new StructureMineshaft(Vector3.zero, 100));

            this.caveSegements = new List<CaveSegment>();
            this.caveDrawers = new List<CaveDrawer>();
            this.caveDrawers.Add(new CaveDrawer(Vector3.zero, Vector3.forward));
            this.caveDrawers.Add(new CaveDrawer(Vector3.zero, Vector3.right));
            this.caveDrawers.Add(new CaveDrawer(Vector3.zero, Vector3.back));
            this.caveDrawers.Add(new CaveDrawer(Vector3.zero, Vector3.left));

            for(int i = 0; i < 10; i++) {
                for(int j = 0; j < this.caveDrawers.Count; j++) {
                    CaveDrawer cd = this.caveDrawers[j];
                    cd.addNextSegment(this.caveDrawers, this.caveSegements);
                }

                for(int j = 0; j < this.caveSegements.Count; j++) {
                    //check both points                    
                }
            }

            for (int i = this.caveDrawers.Count - 1; i >= 0; i--) {
                CaveDrawer cd = this.caveDrawers[i];
                if(cd.outOfBounds()) {
                    this.caveDrawers.Remove(cd);
                }
            }

            //NbtFile file = new NbtFile(this.saveData());
            //file.SaveToFile("mapData.dat", NbtCompression.None);
        }

        public NbtCompound saveData() {
            NbtCompound tag = new NbtCompound("root");

            NbtList tag1 = new NbtList("mine", NbtTagType.Compound);
            foreach(StructureMineshaft m in this.mineshaftList) {
                tag1.Add(m.writeToNbt(new NbtCompound()));
            }

            tag.Add(tag1);

            return tag;
        }

        public void loadData(NbtCompound tag) {
            foreach (NbtCompound compound in tag.Get<NbtList>("mineshafts")) {
                StructureMineshaft mineshaft = new StructureMineshaft();
                mineshaft.readFromNbt(compound);
                this.mineshaftList.Add(mineshaft);
            }
        }

        void Update() {
            this.region.debugDisplay();

            foreach(CaveSegment cs in this.caveSegements) {
                cs.debugDisplay();
            }
            //foreach(StructureMineshaft m in this.mineshaftList) {
            //    m.debugDisplay();
            //}
        }
    }
}
