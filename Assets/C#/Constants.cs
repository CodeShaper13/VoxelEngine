using UnityEngine;

public class Constants : MonoBehaviour {
    public static Constants instance;

    public Material blockMaterial;
    public Material itemMaterial;
    
    public void Awake() {
        if(Constants.instance == null) {
            Constants.instance = this;
        }
    }
}
