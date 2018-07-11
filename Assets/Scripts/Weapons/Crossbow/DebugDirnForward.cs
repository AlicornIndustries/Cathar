using UnityEngine;

public class DebugDirnForward : MonoBehaviour {

    private void Update () {
        Debug.DrawLine(transform.position, transform.position + (transform.forward * 10f));
	}
}
