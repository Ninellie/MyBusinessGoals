using Assets;
using Bitsplash.DatePicker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Bitsplash.Vector
{
    public class FlexibleFrame : MaskableGraphic , IDatePickerSettingsItem
    {
        [SerializeField]
        [HideInInspector]
        private bool isOpen;

        public string EditorTitle { get { return gameObject.name; } }

        public int Order { get { return 3; } }

        public bool ShowLeft = true;
        public bool ShowTop = true;        
        public bool ShowRight = true;
        public bool ShowBottom = true;

        public float LineThickness = 2f;
        public float Offset = 0f;
        public float TextureTile = 1f;

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();
            var rect = GetPixelAdjustedRect();
            Rect xRect = CommonMethods.VerticalTextureTile(TextureTile);

            if(ShowLeft)
                CommonMethods.DrawVertical(rect.x - Offset, rect, LineThickness, xRect, color, vh);
            if(ShowRight)
                CommonMethods.DrawVertical(rect.x + rect.width + Offset, rect, LineThickness, xRect, color, vh);

            Rect yRect = CommonMethods.HorizontalTextureTile(TextureTile);

            if(ShowTop)
                CommonMethods.DrawHorizontal(rect.y - Offset, rect, LineThickness, yRect, color, vh);

            if (ShowBottom)
                CommonMethods.DrawHorizontal(rect.y + rect.height + Offset, rect, LineThickness, yRect, color, vh);
        }
    }

}