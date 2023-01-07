using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ContentFitter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RectTransform[] rects = GetComponentsInChildren<RectTransform>();
        foreach (RectTransform r in rects) {
            if (r == GetComponent<RectTransform>()) {
                continue;
            }
            if (r.gameObject.activeInHierarchy) {

                GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, r.rect.height);
                break;
            }
        }
    }
}
