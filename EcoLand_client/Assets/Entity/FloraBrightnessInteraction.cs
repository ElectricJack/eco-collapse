using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace EntitySystem
{
    // This behavior represents a plant that absorbs sunlight better and so increases the brightness of the square its in
    public class FloraBrightnessInteraction : BaseComponent
    {
        public float brightness = 0.001f;

        private void Awake() {
            base.Awake();
            entity.currentTile.brightness += brightness;
        }

        private void OnDestroy() {
            entity.currentTile.brightness -= brightness;
        }
    }
}

