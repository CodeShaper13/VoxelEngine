using System;
using UnityEngine;
using VoxelEngine.Blocks;
using VoxelEngine.Util;

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
                verticalVelocity -= 12 * Time.deltaTime;
            }

            if (Input.GetKeyDown(KeyCode.Space) && characterController.isGrounded) {
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
        /// <returns></returns>
        private float getMoveSpeed() {
            if(Input.GetKey(KeyCode.LeftShift) && this.characterController.isGrounded) {
                return PlayerMover.runSpeed;
            } else {
                return PlayerMover.walkSpeed;
            }
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
