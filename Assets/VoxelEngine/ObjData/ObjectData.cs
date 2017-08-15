using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;
using VoxelEngine.Blocks;
using VoxelEngine.Items;

namespace VoxelEngine.ObjData {

    public class ObjectData {

        private static ObjectData singleton;

        private string[] dataEntries;

        public ObjectData() {
            this.dataEntries = new string[Block.BLOCK_LIST.Length + Item.ITEM_LIST.Length];

            try {
                string[] lines = Regex.Split(Resources.Load<TextAsset>("Text/objectDataText").text, "\n");

                int id;
                string[] tokens;

                for (int i = 0; i < lines.Length; i++) {
                    tokens = Regex.Split(lines[i], "=");
                    id = int.Parse(tokens[0]);
                    this.dataEntries[id] = tokens[1];
                }

            } catch(DirectoryNotFoundException e) {
                Debug.LogError("Could not find object data file!");
            }

            ObjectData.singleton = this;
        }

        public static string getInfo(Block block) {
            return ObjectData.singleton.func(block.id);
        }

        public static string getInfo(Item item) {
            return ObjectData.singleton.func(item.id);
        }

        private string func(int id) {
            if(id < 0 || id > this.dataEntries.Length) {
                return "OUT_OF_BOUND!";
            }
            string de = this.dataEntries[id];
            if(de == null) {
                return "NULL_ENTRY_ERROR";
            }
            return de;
        }
    }
}
