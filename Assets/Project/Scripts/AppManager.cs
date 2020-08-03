using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppManager : MonoBehaviour
{
    public GameObject _PlayerPrefab;
    [HideInInspector]
    public GameObject _PlayerInstance;
    public Transform _PlayerSpawn;

    private void Awake()
    {
        Init();
    }
    private void Init()
    {
        _PlayerInstance = Instantiate(_PlayerPrefab);
        _PlayerInstance.transform.SetPositionAndRotation(_PlayerSpawn.position, _PlayerSpawn.rotation);
    }
}
