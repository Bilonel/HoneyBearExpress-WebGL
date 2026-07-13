// Assets/Scripts/UI/MachineStatusIndicator.cs
using UnityEngine;
using HoneyBearExpress.Buildings;

namespace HoneyBearExpress.UI
{
    public class MachineStatusIndicator : MonoBehaviour
    {
        [Header("Progress Circle")]
        [SerializeField] private SpriteRenderer progressCircleRenderer;

        [Header("Warning Icons")]
        [SerializeField] private SpriteRenderer warningIconRenderer;
        [SerializeField] private Sprite unconnectedSprite;
        [SerializeField] private Sprite cloggedSprite;

        private IMachineStatus _machine; // Soyutlanan makine referansı
        private MaterialPropertyBlock _propBlock;
        private static readonly int FillAmountHash = Shader.PropertyToID("_FillAmount");

        private void Awake()
        {
            _propBlock = new MaterialPropertyBlock();
            
            // Üst hiyerarşide IMachineStatus taşıyan herhangi bir komponenti (Hive, Extractor vb.) bulur
            _machine = GetComponentInParent<IMachineStatus>();
            
            if (_machine == null)
            {
                Debug.LogError("MachineStatusIndicator: Üst objede IMachineStatus arayüzünü uygulayan bir makine bulunamadı!", this);
            }
        }
	void Start()
	{
		transform.rotation=transform.localRotation;
	}
        private void Update()
        {
            if (_machine == null) return;

            // Ortak Arayüz üzerinden durum kontrolü (Gerçek Polymorphism)
            if (_machine.IsUnconnected())
            {
                ShowWarning(unconnectedSprite);
                HideProgressCircle();
            }
            else if (_machine.IsClogged())
            {
                ShowWarning(cloggedSprite);
                HideProgressCircle();
            }
            else if (_machine.IsActiveProcessing())
            {
                HideWarning();
                ShowAndUpdateProgress(_machine.GetProgress());
            }
            else
            {
                HideWarning();
                HideProgressCircle();
            }
        }

        private void ShowWarning(Sprite icon)
        {
            if (warningIconRenderer != null)
            {
                warningIconRenderer.sprite = icon;
                warningIconRenderer.enabled = true;
            }
        }

        private void HideWarning()
        {
            if (warningIconRenderer != null)
            {
                warningIconRenderer.enabled = false;
            }
        }

        private void ShowAndUpdateProgress(float progress)
        {
            if (progressCircleRenderer == null) return;

            progressCircleRenderer.enabled = true;

            progressCircleRenderer.GetPropertyBlock(_propBlock);
            _propBlock.SetFloat(FillAmountHash, progress);
            progressCircleRenderer.SetPropertyBlock(_propBlock);
        }

        private void HideProgressCircle()
        {
            if (progressCircleRenderer != null)
            {
                progressCircleRenderer.enabled = false;
            }
        }
    }
}
