using UnityEngine;
using VoxelEngine.Blocks;
using VoxelEngine.Util;

namespace VoxelEngine.Entities.Player {

    /// <summary>
    /// Class that handles the player movement.
    /// </summary>
    public class PlayerMover {

        private const float WALK_SPEED = 5f;
        private const float RUN_SPEED = 10f;
        private const float CLIMB_SPEED = 2f;

        private EntityPlayer player;
        private CharacterController characterController;

        public float mouseSensitivity = 3.5f;

        private float mouseVerticalRotation;
        private float verticalVelocity;
        private bool runningWhenLeftGround;

        private AudioSource soundSource;
        private float footstepTimer;

        public PlayerMover(EntityPlayer player) {
            this.player = player;
            this.characterController = this.player.GetComponent<CharacterController>();

            this.soundSource = this.player.GetComponents<AudioSource>()[0];
        }

        public void updateMover() {            
            // Rotate camera.
            this.player.transform.Rotate(0.0f, Input.GetAxis("Mouse X") * mouseSensitivity, 0.0f);
            this.mouseVerticalRotation -= Input.GetAxis("Mouse Y") * mouseSensitivity;
            this.mouseVerticalRotation = Mathf.Clamp(this.mouseVerticalRotation, -90, 90);
            this.player.mainCamera.localRotation = Quaternion.Euler(mouseVerticalRotation, 0, 0);

            // WASD Movement.
            float f = this.getMoveSpeed();
            float forwardSpeed = Input.GetAxis("Vertical") * f;
            float sideSpeed = Input.GetAxis("Horizontal") * f;

            // Jump/fall.
            if (this.verticalVelocity > 0) {
                this.verticalVelocity -= 15 * Time.deltaTime;
            }

            if (Input.GetKeyDown(KeyCode.Space) && characterController.isGrounded) {
                this.runningWhenLeftGround = this.isRunKeyDown();
                this.verticalVelocity = 15; // Jump speed
            }

            // Move the Entity.
            Vector3 speed = new Vector3(sideSpeed, this.getUpDownSpeed(), forwardSpeed);
            speed = this.player.transform.rotation * speed;

            characterController.Move(speed * Time.deltaTime);

            if(this.footstepTimer > 0) {
                this.footstepTimer -= Time.deltaTime;
            }

            if(this.isGrounded() && this.footstepTimer <= 0 && (forwardSpeed != 0 || sideSpeed != 0)) {
                this.soundSource.PlayOneShot(this.getRndFootstep());
                this.footstepTimer = this.isRunKeyDown() ? 0.3f : 0.5f;
            }
        }

        /// <summary>
        /// Returns the footstep sound for the player.
        /// </summary>
        private AudioClip getRndFootstep() {
            return SoundManager.singleton.footstep;
        }

        private float getUpDownSpeed() {
            bool flag = this.isOnLadder();
            if(flag) {
                return Input.GetKey(KeyCode.W) ? CLIMB_SPEED:  -CLIMB_SPEED;
            } else if(Input.GetKey(KeyCode.LeftControl)) {
                return 0;
            } else {
                return Physics.gravity.y + this.verticalVelocity;
            }
        }

        /// <summary>
        /// Returns the players movement speed.
        /// </summary>
        private float getMoveSpeed() {
            if(this.characterController.isGrounded) {
                if (this.isRunKeyDown()) {
                    return PlayerMover.RUN_SPEED;
                }
            } else if(this.runningWhenLeftGround) {
                return PlayerMover.RUN_SPEED;
            }
            return PlayerMover.WALK_SPEED;
        }

        private bool isRunKeyDown() {
            return Input.GetKey(KeyCode.LeftShift);
        }

        /// <summary>
        /// Returns true if the player is climbing a ladder.
        /// </summary>
        private bool isOnLadder() {
            // Coppied from Minecraft...
            Bounds playerBounds = new Bounds(this.player.transform.position, new Vector3(0.5f, 2, 0.5f));
            int mX = MathHelper.floor(playerBounds.min.x);
            int mY = MathHelper.floor(playerBounds.min.y);
            int mZ = MathHelper.floor(playerBounds.min.z);
            for (int y = mY; y < playerBounds.max.y; y++) {
                for (int x = mX; x < playerBounds.max.x; x++) {
                    for (int z = mZ; z < playerBounds.max.z; z++) {
                        if (this.player.world.getBlock(x, y, z) == Block.ladder) {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool isGrounded() {
            return this.characterController.isGrounded || this.isOnLadder();
        }
    }
}
