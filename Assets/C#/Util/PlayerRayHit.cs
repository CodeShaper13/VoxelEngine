using UnityEngine;

public class PlayerRayHit {
    public RaycastHit unityRaycastHit;
    public BlockState state;
    public Entity entity;
    
    public PlayerRayHit(Block block, byte meta, BlockPos pos, RaycastHit unityRaycastHit) {
        this.state = new BlockState(block, meta, pos);
        this.unityRaycastHit = unityRaycastHit;
    }

    public PlayerRayHit(Entity entity, RaycastHit unityRaycastHit) {
        this.entity = entity;
        this.unityRaycastHit = unityRaycastHit;
    }
}
