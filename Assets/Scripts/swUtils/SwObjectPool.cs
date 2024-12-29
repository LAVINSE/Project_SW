using NUnit.Framework;
using System;
using UnityEngine;

public class SwObjectPool : SwSingleton<SwObjectPool>
{
    [Serializable]
    public class Pool
    {
        public string Tag;
        public GameObject Prefab;
        public int Size;
    }

    #region 변수
    [SerializeField] private Pool[] pools;

    //private List<GameObject> spawnObjects;
    #endregion // 변수

    #region 함수
    #endregion // 함수
}
