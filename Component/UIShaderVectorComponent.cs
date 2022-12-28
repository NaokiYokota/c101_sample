using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class UIShaderVectorComponent : BaseMeshEffect, IMaterialModifier
{
    [SerializeField]
    private Shader _shader;

    [SerializeField]
    private Color _fillColor = Color.white;

    public Color FillColor
    {
        get => _fillColor;
        set
        {
            _fillColor = value;
            SetMaterialDirty();
        }
    }

    [SerializeField]
    private float _radius;

    public float Radius
    {
        get => _radius;
        set
        {
            _radius = value;
            SetMaterialDirty();
        }
    }

    [SerializeField]
    private Color _outlineColor = Color.white;

    public Color OutlineColor
    {
        get => _outlineColor;
        set
        {
            _outlineColor = value;
            SetMaterialDirty();
        }
    }

    [SerializeField]
    private float _outlineSize;

    public float OutlineSize
    {
        get => _outlineSize;
        set
        {
            _outlineSize = value;
            SetMaterialDirty();
        }
    }

    private Material _material;

    private Graphic _targetGraphic;

    private RectTransform _rectTransform;

    private static readonly int RadiusShaderPropertyId = Shader.PropertyToID("_Radius");
    private static readonly int FillColorShaderPropertyId = Shader.PropertyToID("_FillColor");
    private static readonly int SizeXShaderPropertyId = Shader.PropertyToID("_SizeX");
    private static readonly int SizeYShaderPropertyId = Shader.PropertyToID("_SizeY");
    private static readonly int OutlineSizeShaderPropertyId = Shader.PropertyToID("_OutlineSize");
    private static readonly int OutlineColorShaderPropertyId = Shader.PropertyToID("_OutlineColor");

    protected override void OnEnable()
    {
        base.OnEnable();
        _targetGraphic = GetComponent<Graphic>();
        _rectTransform = transform as RectTransform;
        SetMaterialDirty();
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        SetMaterialDirty();
    }

    public Material GetModifiedMaterial(Material baseMaterial)
    {
        if (_material == null)
        {
            _material = new Material(_shader)
            {
                hideFlags = HideFlags.HideAndDontSave
            };
        }

        _material.CopyPropertiesFromMaterial(baseMaterial);
        _material.SetColor(FillColorShaderPropertyId, _fillColor);
        _material.SetFloat(RadiusShaderPropertyId, _radius);
        _material.SetFloat(SizeXShaderPropertyId, _targetGraphic.rectTransform.rect.width / 2);
        _material.SetFloat(SizeYShaderPropertyId, _targetGraphic.rectTransform.rect.height / 2);
        _material.SetColor(OutlineColorShaderPropertyId, _outlineColor);
        _material.SetFloat(OutlineSizeShaderPropertyId, _outlineSize);
        return _material;
    }

    private void SetMaterialDirty()
    {
        if (_targetGraphic == null) return;
        _targetGraphic.SetMaterialDirty();
    }

    public override void ModifyMesh(VertexHelper vh)
    {
        var vertex = new UIVertex();
        var offset = Vector2.Scale(_rectTransform.rect.size, _rectTransform.pivot - new Vector2(0.5f, 0.5f));
        for (int i = 0; i < vh.currentVertCount; i++)
        {
            vh.PopulateUIVertex(ref vertex, i);
            vertex.uv1 = new Vector2(vertex.position.x, vertex.position.y) - offset;
            vh.SetUIVertex(vertex, i);
        }
    }

#if UNITY_EDITOR
    protected override void OnValidate()
    {
        base.OnValidate();
        SetMaterialDirty();
    }

    protected override void Reset()
    {
        base.Reset();
    }
#endif
}
