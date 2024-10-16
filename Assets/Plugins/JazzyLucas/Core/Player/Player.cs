using System;
using UnityEngine;
using JazzyLucas.Core.Utils;
using L = JazzyLucas.Core.Utils.Logger;

namespace JazzyLucas.Core
{
    public class Player : MonoBehaviour
    {
        [field: SerializeField] public MovementController MovementController { get; private set; }
        [field: SerializeField] public ViewController ViewController { get; private set; }
        [field: SerializeField] public PerspectiveController PerspectiveController { get; private set; }
        [field: SerializeField] public HitscanController HitscanController { get; private set; }
        [field: SerializeField] public PushController PushController { get; private set; }
        [field: SerializeField] public GroundingController GroundingController { get; private set; }
    }
}
