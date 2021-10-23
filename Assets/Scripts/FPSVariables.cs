using System;
using UnityEngine;

namespace Scripts.FPSController
{
    [Serializable]
    public class BobVariables
    {

        [Header("Run Bob")]
        [Tooltip("Default: 0.1")]
        public float RunHorizontalBob;
        [Tooltip("Default: 0.1")]
        public float RunVerticalBob;
        [Tooltip("Less is Faster. Default: 4")]
        public float RunBobInterval;
        [Header("Jog Bob")]
        public float JogHorizontalBob;
        public float JogVerticalBob;
        public float JogBobInterval;
        [Header("Walk Bob")]
        public float WalkHorizontalBob;
        public float WalkVerticalBob;
        public float WalkBobInterval;
        [Header("Crouch Bob")]
        public float CrouchHorizontalBob;
        public float CrouchVerticalBob;
        public float CrouchBobInterval;
    }

    [Serializable]
    public class AudioValues
    {
        public float WalkAudioSpeed;
        public float JogAudioSpeed;
        public float RunAudioSpeed;
    }
}
