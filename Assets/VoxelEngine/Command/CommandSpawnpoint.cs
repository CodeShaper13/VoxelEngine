using UnityEngine;
using VoxelEngine.Entities;
using VoxelEngine.Level;

namespace VoxelEngine.Command {

    public class CommandSpawnpoint : CommandBase {

        public CommandSpawnpoint() : base("spawnpoint", "spawnpoint", "Teleports the player to there spawn") { }

        public override string runCommand(World world, EntityPlayer player, string[] args) {
            player.transform.position = world.worldData.spawnPos + new Vector3(0.5f, 0.5f, 0.5f);
            return null;
        }
    }
}
