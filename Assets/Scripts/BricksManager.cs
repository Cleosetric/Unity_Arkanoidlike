using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BricksManager : MonoBehaviour
{
    #region Singleton

    private static BricksManager _instance;

    public static BricksManager Instance => _instance;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    #endregion

    public List<GameObject> RemainingBricks = new List<GameObject>();

    private void Start()
    {
        InitializeBricks();
    }

    private void InitializeBricks(){
        foreach(GameObject brick in GameObject.FindGameObjectsWithTag("Brick")) {
             RemainingBricks.Add(brick);
        }
    }
}