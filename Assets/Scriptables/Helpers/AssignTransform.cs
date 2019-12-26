﻿using SO;
using UnityEngine;

namespace SA
{
    public class AssignTransform : MonoBehaviour
    {
        public TransformVariable transformVariable;

        private void OnEnable()
        {
            transformVariable.value = this.transform;
            Destroy(this);
        }

    }
}