using UnityEngine;
using System.Collections;

public class Modify : MonoBehaviour {
    //Vector2 rot;

    void Update() {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 4)) {
            if (Input.GetMouseButtonDown(0)) {
                EditTerrain.SetBlock(hit, Block.air);
            }
            if(Input.GetMouseButtonDown(1)) {
                EditTerrain.SetBlock(hit, Block.grass, true);
            }
        }
    }
}