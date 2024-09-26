using Assets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Bitsplash.Vector
{
    public class FlexibleGrid : MaskableGraphic , DatePicker.IDatePickerSettingsItem
    {
        [SerializeField]
        [HideInInspector]
        private bool isOpen;

        public string EditorTitle { get { return gameObject.name; } }

        public int Order { get { return 1; } }

        public int TotalColumns = 7;
        public int TotalRows = 6;
        public float ColumnLineThickness = 2f;
        public float RowLineThickness = 2f;
        public float TextureTile = 1f;

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();
            var rect = GetPixelAdjustedRect();
            Rect xRect = CommonMethods.VerticalTextureTile(TextureTile);

            for (int i = 1; i < TotalColumns; i++)
            {
                float factor = ((float)i) / (float)TotalColumns;
                float x = CommonMethods.InterpolateInRectX(factor, rect);
                CommonMethods.DrawVertical(x, rect, ColumnLineThickness, xRect, color, vh);
            }

            Rect yRect = CommonMethods.HorizontalTextureTile(TextureTile);

            for (int i = 1; i < TotalRows; i++)
            {
                float factor = ((float)i) / (float)TotalRows;
                float y = CommonMethods.InterpolateInRectY(factor, rect);
                CommonMethods.DrawHorizontal(y, rect, RowLineThickness, yRect, color, vh);
            }
        }
    }

}