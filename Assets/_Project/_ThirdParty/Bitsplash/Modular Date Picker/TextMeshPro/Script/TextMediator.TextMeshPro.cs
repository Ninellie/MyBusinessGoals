using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bitsplash.DatePicker
{
    public partial class TextMediator
    {

        partial void MediateTextMeshProText(string text)
        {
            var tmp = GetComponent<TMPro.TextMeshProUGUI>();
            if (tmp != null)
                tmp.text = text;
        }
        partial void MediateTextMeshProColor(Color color)
        {
            var tmp = GetComponent<TMPro.TextMeshProUGUI>();
            if (tmp != null)
                tmp.color =color;

        }
    }
}