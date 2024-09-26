using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Bitsplash.DatePicker
{
    partial class TextMediator : MonoBehaviour
    {
        partial void MediateTextMeshProText(string text);
        partial void MediateTextMeshProColor(Color color);

        public void SetText(string text)
        {
            MediateTextMeshProText(text);
            var comp = GetComponent<Text>();
            if (comp != null)
                comp.text = text;
        }
        public void SetColor(Color color)
        {
            MediateTextMeshProColor(color);
            var comp = GetComponent<Text>();
            if (comp != null)
                comp.color = color;

        }
    }
}
