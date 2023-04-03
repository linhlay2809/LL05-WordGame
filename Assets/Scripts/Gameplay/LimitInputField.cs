using System;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Gameplay
{
    public class LimitInputField : MonoBehaviour
    {
        private TMP_InputField _inputField;
        private void Awake()
        {
            _inputField = GetComponent<TMP_InputField>();
        }

        private void Start()
        {
            FocusInput();
        }

        // Convert to 1 character when value changes > 1 character
        public void OnValueChange(string text)
        {
            if (text.Length <= 1) return;
            _inputField.text = text.Last().ToString();
        }

        // Focus to inputField
        public void FocusInput()
        {
            _inputField.Select();
            _inputField.ActivateInputField();
        }
    }
}
