using UnityEngine;
using VoxelEngine.Blocks;
using VoxelEngine.Entities;

namespace VoxelEngine.Util {

    /// <summary>
    /// Represents the hit of a raycast send out by the player.
    /// </summary>
    public class PlayerRayHit {

        public RaycastHit unityRaycastHit;
        public BlockState hitState;
        public Entity entity;

        public PlayerRayHit(Block hitBlock, int meta, BlockPos hitPos, RaycastHit unityRaycastHit) : this(unityRaycastHit) {
            this.hitState = new BlockState(hitBlock, meta, hitPos);
        }

        public PlayerRayHit(Entity entity, RaycastHit unityRaycastHit) : this(unityRaycastHit) {
            this.entity = entity;
        }

        private PlayerRayHit(RaycastHit unityRaycastHit) {
            this.unityRaycastHit = unityRaycastHit;
        }

        /// <summary>
        /// Returns true if the PlayerRayHit struck a block.
        /// </summary>
        public bool hitBlock() {
            return this.hitState != null;
        }

        /// <summary>
        /// Returns true if the PlayerRayHit struck an entity.
        /// </summary>
        public bool hitEntity() {
            return this.entity != null;
        }

        /// <summary>
        /// Returns the clicked face of the hit block.
        /// </summary>
        public Direction getClickedBlockFace() {
            if(this.hitBlock()) {
                Vector3 normal = this.unityRaycastHit.normal;
                foreach (Direction direction in Direction.all) {
                    if (normal == direction.direction.toVector()) {
                        return direction;
                    }
                }
            }
            return Direction.NONE;
        }
    }
}
