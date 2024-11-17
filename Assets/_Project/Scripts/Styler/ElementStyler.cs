using TMPro;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.UI;

public class ElementStyler : MonoBehaviour
{
    [SerializeField] private TMP_ColorGradient color;

    private Image _image;
    private SVGImage _svgImage;

    private void OnValidate()
    {
        if (color == null)
        {
            return;
        }

        if (_svgImage == null)
        {
            if (TryGetComponent(out SVGImage svgImage))
            {
                _svgImage = svgImage;
                _svgImage.color = color.bottomLeft;
            }
        }
        else
        {
            _svgImage.color = color.bottomLeft;
        }

        if (_image == null)
        {
            if (!TryGetComponent(out Image image)) return;
            _image = image;
            _image.color = color.bottomLeft;
        }
        else
        {
            _image.color = color.bottomLeft;
        }
    }

    public void SetStyle(TMP_ColorGradient colorGradient)
    {
        color = colorGradient;
        OnValidate();
    }
}