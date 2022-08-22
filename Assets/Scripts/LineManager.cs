using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.AR;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LineManager : MonoBehaviour
{

    [SerializeField]
    private Button restart;

    [SerializeField]
    private LineRenderer _lineRenderer;

    [SerializeField]
    private ARPlacementInteractable _placementInteractable;

    [SerializeField]
    private TextMeshPro _textMeshPro;

    private void Start()
    {
        _placementInteractable.objectPlaced.AddListener(DrawLine);
        restart.onClick.AddListener(() => Reload());
        
    }

    void DrawLine(ARObjectPlacementEventArgs args) {

        _lineRenderer.positionCount++;
        _lineRenderer.SetPosition(_lineRenderer.positionCount - 1,args.placementObject.transform.position);

        if (_lineRenderer.positionCount > 1)
        {
            Vector3 pointA = _lineRenderer.GetPosition(_lineRenderer.positionCount - 1);
            Vector3 pointB = _lineRenderer.GetPosition(_lineRenderer.positionCount - 2);
            float dist=Vector3.Distance(pointA,pointB);
            TextMeshPro distText = Instantiate(_textMeshPro);
            distText.text = (dist * 100f).ToString("F2")+" cm";
            Vector3 directionVector = (pointB - pointA);
            Vector3 normal = args.placementObject.transform.up;
            Vector3 upd=Vector3.Cross(directionVector, normal).normalized;
            Quaternion rotation = Quaternion.LookRotation(-normal,upd);
            distText.transform.rotation = rotation;
            distText.transform.position = (pointA + directionVector * 0.5f) + upd * 0.008f;
        }

        
    }

    void Reload(){
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
