using UnityEngine;

public class PlayerManager : MonoBehaviour {

    #region Singleton

    public static PlayerManager instance;

    private void Awake()
    {
        instance = this;
    }

    #endregion Singleton

    public GameObject player;



}
