using UnityEngine;
using VoxelEngine.Blocks;
using VoxelEngine.Entities;

namespace VoxelEngine.Util {

    public class PlayerRayHit {
        public RaycastHit unityRaycastHit;
        public BlockState hitState;
        public Entity entity;

        public PlayerRayHit(RaycastHit unityRaycastHit) {
            this.unityRaycastHit = unityRaycastHit;
        }

        public PlayerRayHit(Block hitBlock, byte meta, BlockPos hitPos, RaycastHit unityRaycastHit) : this(unityRaycastHit) {
            this.hitState = new BlockState(hitBlock, meta, hitPos);
        }

        public PlayerRayHit(Entity entity, RaycastHit unityRaycastHit) : this(unityRaycastHit) {
            this.entity = entity;
        }
    }
}
