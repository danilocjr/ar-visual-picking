using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
public class MultitargetBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Transform target1;
    [SerializeField] private Transform target2;

    [SerializeField] private Transform target1Start;
    [SerializeField] private Transform target2Start;

    [SerializeField] private GameObject[] boxes;
    public void SetTarget(ARTrackedImage img)
    {
        if (img.referenceImage.name == "QRCode")
        {
            target2.parent = null;
            target2.SetParent(target2Start);
            target2.localPosition = Vector3.zero;

            target1.parent = null;

            transform.SetParent(target1);

            target1.SetParent(img.transform);
            target1.localPosition = Vector3.zero;
        }
        else if (img.referenceImage.name == "QRCode2")
        {
            target1.parent = null;
            target1.SetParent(target1Start);
            target1.localPosition = Vector3.zero;

            target2.parent = null;

            transform.SetParent(target2);

            target2.SetParent(img.transform);
            target2.localPosition = Vector3.zero;
        }
    }

    public void SetBoxes(Camera eventCamera, int layer)
    {
        foreach (GameObject box in boxes)
            box.layer = layer;

        foreach (Canvas canvas in GetComponentsInChildren<Canvas>())
        {
            canvas.worldCamera = eventCamera;
            canvas.gameObject.layer = 5;
        }
    }
    
}
