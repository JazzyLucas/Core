using System;
using System.Collections;
using System.Collections.Generic;
using JazzyLucas.Core;
using UnityEngine;

namespace JazzyLucas.Sample
{
    public class ClickableTest : MonoBehaviour
    {
        [field: SerializeField] public Clickable clickable { get; private set; }

        private void Awake()
        {
            clickable.OnLeftClick += () =>
            {
                Debug.Log("Left click");
            };
            clickable.OnRightClick += () =>
            {
                Debug.Log("Right click");
            };
            clickable.OnMiddleClick += () =>
            {
                Debug.Log("Middle click");
            };
        }
    }
}
