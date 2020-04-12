using TMPro;

namespace SO.UI
{
    public class UpdateTextFromIntVariable : UIPropertyUpdater
    {
        public IntVariable targetVariable;
        public TextMeshProUGUI targetText;

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
