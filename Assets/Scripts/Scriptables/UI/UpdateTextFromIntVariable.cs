using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SO;
using UnityEngine.UI;

namespace SO.UI
{
    public class UpdateTextFromIntVariable : UIPropertyUpdater
    {
        public IntVariable targetVariable;
        public Text targetText;
        
        /// <summary>
        /// Use this to update a text UI element based on the target integer variable
        /// </summary>
        public override void Raise()
        {
            targetText.text = targetVariable.value.ToString();
        }
        
        public void Raise(string target)
        {
            targetText.text = target;
        }
    }
}
