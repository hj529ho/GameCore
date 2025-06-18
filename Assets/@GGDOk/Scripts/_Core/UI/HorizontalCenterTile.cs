using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
[RequireComponent(typeof(RawImage))]
public sealed class HorizontalCenterTile : MonoBehaviour
{
    RawImage _image;
    public RawImage Image
    {
        get
        {
            if (_image == null)
            {
                _image = GetComponent<RawImage>();
            }
            return _image;
        }
    }
    RectTransform _rectTransform;
    public RectTransform RectTransform
    {
        get
        {
            if (_rectTransform == null)
            {
                _rectTransform = transform as RectTransform;
            }
            return _rectTransform;
        }
    }

    bool _isDirty;

    private void Update()
    {
        if (_isDirty) OnRectTransformDimensionsChange();
    }

    private void OnRectTransformDimensionsChange()
    {
        var image = Image;
        if (image == null)
        {
            _isDirty = true;
            return;
        }
        _isDirty = false;

        var texture = image.texture;
        var size = RectTransform.rect;
        var desireSize = size.x / size.y * texture.height;

        var rect = image.uvRect;
        rect.width = desireSize / texture.width;
        rect.x = (1f - rect.width) / 2f;
        image.uvRect = rect;
    }
}
