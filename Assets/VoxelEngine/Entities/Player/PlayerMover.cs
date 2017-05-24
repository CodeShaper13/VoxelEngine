using UnityEngine;
using VoxelEngine.Blocks;

namespace VoxelEngine.Entities.Player {

    /// <summary>
    /// Class that handles the player movement.
    /// </summary>
    public class PlayerMover {

        private const float walkSpeed = 5f;
        private const float runSpeed = 10f;
        private const float climbSpeed = 2f;

        private EntityPlayer player;
        private CharacterController characterController;

        public float mouseSensitivity = 3.5f;

        private float verticalRotation;
        private float verticalVelocity;
        /// <summary>
        /// If true, the player was running when they left the ground.
        /// </summary>
        private bool flag;

        public PlayerMover(EntityPlayer player) {
            this.player = player;
            this.characterController = this.player.GetComponent<CharacterController>();
        }

        public void updateMover() {            
            // Rotate camera.
            this.player.transform.Rotate(0.0f, Input.GetAxis("Mouse X") * mouseSensitivity, 0.0f);
            this.verticalRotation -= Input.GetAxis("Mouse Y") * mouseSensitivity;
            this.verticalRotation = Mathf.Clamp(this.verticalRotation, -90, 90);
            this.player.mainCamera.localRotation = Quaternion.Euler(verticalRotation, 0, 0);

            // WASD Movement.
            float f = this.getMoveSpeed();
            float forwardSpeed = Input.GetAxis("Vertical") * f;
            float sideSpeed = Input.GetAxis("Horizontal") * f;

            // Jump/fall.
            if (verticalVelocity > 0) {
                verticalVelocity -= 15 * Time.deltaTime;
            }

            if (Input.GetKeyDown(KeyCode.Space) && characterController.isGrounded) {
                this.flag = this.isRunKeyDown();
                verticalVelocity = 15; // Jump speed
            }

            // Move the Entity.
            Vector3 speed = new Vector3(sideSpeed, this.isOnLadder() && Input.GetKey(KeyCode.W) ? climbSpeed : Input.GetKey(KeyCode.LeftControl) ? 0 : Physics.gravity.y + verticalVelocity, forwardSpeed);
            speed = this.player.transform.rotation * speed;

            characterController.Move(speed * Time.deltaTime);
        }

        /// <summary>
        /// Returns the players movement speed.
        /// </summary>
        private float getMoveSpeed() {
            if(this.characterController.isGrounded) {
                if (this.isRunKeyDown()) {
                    return PlayerMover.runSpeed;
                }
            } else {
                if(this.flag) {
                    return PlayerMover.runSpeed;
                }
            }

            return PlayerMover.walkSpeed;
        }

        private bool isRunKeyDown() {
            return Input.GetKey(KeyCode.LeftShift);
        }

        /// <summary>
        /// Returns true if the player is climbing a ladder.
        /// </summary>
        private bool isOnLadder() {
            // Coppied from Minecraft...
            Bounds bb = new Bounds(this.player.transform.position, new Vector3(0.5f, 2, 0.5f));
            int mX = (int)Mathf.Floor(bb.min.x);
            int mY = (int)Mathf.Floor(bb.min.y);
            int mZ = (int)Mathf.Floor(bb.min.z);
            for (int y = mY; y < bb.max.y; y++) {
                for (int x = mX; x < bb.max.x; x++) {
                    for (int z = mZ; z < bb.max.z; z++) {
                        if (this.player.world.getBlock(x, y, z) == Block.ladder) {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool isGrounded() {
            return this.characterController.isGrounded;
        }
    }
}
