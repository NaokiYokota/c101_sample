using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[DisallowMultipleComponent]
[ExecuteAlways]
[RequireComponent(typeof(Graphic))]
public class UIShaderGradientComponent : UIBehaviour, IMaterialModifier
{
    [SerializeField]
    private Shader _shader;

    [SerializeField]
    private Color _startColor = Color.white;

    public Color StartColor
    {
        get => _startColor;
        set
        {
            _startColor = value;
            SetMaterialDirty();
        }
    }

    [SerializeField]
    private Color _endColor = Color.white;
    
    public Color EndColor
    {
        get => _endColor;
        set
        {
            _endColor = value;
            SetMaterialDirty();
        }
    }
    
    [SerializeField]
    private Vector2 _startPosition;
    
    [SerializeField]
    private Vector2 _endPosition;

    private Material _material;

    private Graphic _targetGraphic;

    private static readonly int StartColorPropertyId = Shader.PropertyToID("_StartColor");
    private static readonly int EndColorPropertyId = Shader.PropertyToID("_EndColor");
    private static readonly int StartPositionPropertyId = Shader.PropertyToID("_StartPosition");
    private static readonly int EndPositionPropertyId = Shader.PropertyToID("_EndPosition");

    protected override void Awake()
    {
        base.Awake();
        _targetGraphic = GetComponent<Graphic>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
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
        _material.SetColor(StartColorPropertyId, _startColor);
        _material.SetColor(EndColorPropertyId, _endColor);
        _material.SetVector(StartPositionPropertyId, _startPosition);
        _material.SetVector(EndPositionPropertyId, _endPosition);

        return _material;
    }

    private void SetMaterialDirty()
    {
        if (_targetGraphic == null) return;
        _targetGraphic.SetMaterialDirty();

    }

#if UNITY_EDITOR
    protected override void OnValidate()
    {
        base.OnValidate();
        SetMaterialDirty();
    }
#endif
}
