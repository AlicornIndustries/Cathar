using UnityEngine;

public abstract class Weapon : MonoBehaviour {
    public abstract void Fire(Vector3 playerVelocity); // since we don't have {}, any inheritors of this script will need to define their own Fire() functions.
}
