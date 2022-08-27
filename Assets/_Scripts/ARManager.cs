using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class ARManager : MonoBehaviour
{

    [SerializeField] ARTrackedImageManager trackedImgManager;
    [SerializeField] AROcclusionManager occlusionManager;
    [SerializeField] ARRaycastManager raycastManager;
    [SerializeField] GameObject boxesPrefab;
    [SerializeField] Camera arCamera;

    //private Texture2D occlusionTex;
    //private Texture2D newOcclusionTex;
    //private RenderTexture occlusionRenderTex;

    [SerializeField] private GameObject boxesInstance;

    private MultitargetBehaviour multiTarget;
    [SerializeField] Text debug;
    private PointerEventData m_PointerEventData;
    [SerializeField] private GraphicRaycaster m_Raycaster;

    void OnEnable()
    {
        trackedImgManager.trackedImagesChanged += OnImageDetected;

    }

    private void OnDisable()
    {
        trackedImgManager.trackedImagesChanged -= OnImageDetected;
    }


    private void OnImageDetected(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (var trackedImg in eventArgs.added)
        {
            if (boxesInstance == null)
            {
                boxesInstance = Instantiate(boxesPrefab, trackedImg.transform);

                multiTarget = boxesInstance.GetComponent<MultitargetBehaviour>();

                multiTarget.SetBoxes(arCamera, 8);
            }

            multiTarget.SetTarget(trackedImg);

        }
        foreach (var trackedImg in eventArgs.updated)
        {
            if (multiTarget != null)
                multiTarget.SetTarget(trackedImg);
        }
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                if (EventSystem.current.currentSelectedGameObject == null)
                {
                    RaycastHit hit;
                    RaycastHit hit2;
                    Ray ray = arCamera.ScreenPointToRay(touch.position);
                    if (!Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << 5))
                    {
                        if (Physics.Raycast(ray, out hit2, Mathf.Infinity, 1 << 8))
                        {
                            if (hit2.collider)
                            {
                                hit2.collider.gameObject.GetComponent<StockManager>().ShowBoxUI();
                            }
                        }
                    }
                }
            }

        }



#if UNITY_EDITOR
        if (Input.mousePresent)
        {
            if (Input.GetMouseButtonDown(0))
            {

                if (EventSystem.current.currentSelectedGameObject == null)
                {

                    RaycastHit hit;
                    RaycastHit hit2;
                    Ray ray = arCamera.ScreenPointToRay(Input.mousePosition);
                    if (!Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << 5))
                    {
                        if (Physics.Raycast(ray, out hit2, Mathf.Infinity, 1 << 8))
                        {
                            if (hit2.collider)
                            {
                                hit2.collider.gameObject.GetComponent<StockManager>().ShowBoxUI();
                            }
                        }
                    }
                }
            }

        }

#endif

        //TENTATIVA DE VERIFICAR DISTÂNCIA

        //occlusionTex = occlusionManager.humanDepthTexture;
        //occlusionRenderTex.format = RenderTextureFormat.ARGBFloat;
        //Graphics.Blit(occlusionTex, occlusionRenderTex);

        //if (newOcclusionTex == null)
        //    newOcclusionTex = new Texture2D(occlusionRenderTex.width, occlusionRenderTex.height, TextureFormat.ARGB32, true);
        //newOcclusionTex.ReadPixels(new Rect(0, 0, occlusionRenderTex.width, occlusionRenderTex.height), 0, 0, false);

        //debug.text = "Distance: " + newOcclusionTex.GetPixel(0, 0).ToString();

    }

}
