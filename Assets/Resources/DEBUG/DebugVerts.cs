using System.Collections.Generic;
using UnityEngine;

public class DebugVerts : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Mesh m = this.GetComponent<MeshFilter>().mesh;
        List<Color> l = new List<Color>();
        List<Vector3> v = new List<Vector3>();
        for(int i = 0; i < m.vertexCount; i++) {
            int j = 0; // = Random.Range(0, 3);
            if (j == 0) {
                l.Add(new Color(1f / m.vertexCount * i, 1f / m.vertexCount * i, 1f / m.vertexCount * i));
            } else if(j == 1) {
                l.Add(Color.gray);
            }          else {
                l.Add(Color.black);
            }
        }
        m.SetColors(l);

        this.GetComponent<MeshFilter>().mesh = m;

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
