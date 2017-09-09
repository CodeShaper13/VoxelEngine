using System.Text;
using UnityEngine;
using UnityEngine.UI;
using VoxelEngine.Blocks;
using VoxelEngine.Util;

namespace VoxelEngine.Entities.Player {

    public class WorldSpaceToolTip : MonoBehaviour {

        private EntityPlayer player;
        private Text text;

        private void Awake() {
            this.text = this.GetComponentInChildren<Text>();
        }

        private void Update() {

            // Update text
            text.text = this.getText();

            // Set position
            if(this.player.posLookingAt != null) {
                this.transform.position = ((BlockPos)this.player.posLookingAt).toVector();
            }

            // Face camera.
            this.transform.rotation = this.player.mainCamera.rotation;
        }

        public WorldSpaceToolTip setPlayer(EntityPlayer player) {
            this.player = player;
            return this;
        }

        private string getText() {
            if (this.player.posLookingAt != null) {
                StringBuilder builder = new StringBuilder();
                BlockPos pos = (BlockPos)this.player.posLookingAt;
                Block block = this.player.world.getBlock(pos);
                if (block is IGoggleData) {
                    return ((IGoggleData)block).getData(this.player.world.getMeta(pos));
                }
            }
            return string.Empty;
        }
    }
}
