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

        //Initially, the line renderer has zero points on screen. Every time a sphere is instantiated, we will add another point to the line renderer.
        _lineRenderer.positionCount++;
        //The coordinates of the last point of line renderer would be set to the placement position of the sphere.
        _lineRenderer.SetPosition(_lineRenderer.positionCount - 1,args.placementObject.transform.position);

        //if the points of lineRenderer are more than 1, only then we can measure objects.
        if (_lineRenderer.positionCount > 1)
        {
            //we will get vector positions of the last two points of the line renderer and store them as Vector A and Vector B.
            Vector3 pointA = _lineRenderer.GetPosition(_lineRenderer.positionCount - 1);
            Vector3 pointB = _lineRenderer.GetPosition(_lineRenderer.positionCount - 2);
 
            //then we use the distance formula to calculate the distance between vectors a and b. Let us say the distance is D.
            float dist=Vector3.Distance(pointA,pointB);
            
            //Next we instantiate the textMeshPro gameObject and store it in a variable called distText.
            TextMeshPro distText = Instantiate(_textMeshPro);
            
            //We set the value of distText equal to D. We also shorten it down to two decimal places and convert it from meters to centimeters.
            distText.text = (dist * 100f).ToString("F2")+" cm";

            //next, we need to align our distText with the line. we need to make sure its angle aligns with the line and that it is clearly visible to the users.
            //directionVector stores the difference between pointB and pointA.
            Vector3 directionVector = (pointB - pointA);
            
            //normal stores the direction of the normal of the sphere.
            Vector3 normal = args.placementObject.transform.up;
            
            //We get the cross product of the directionVector and the normal and then use Quaternions to set the rotation of the text.
            Vector3 upd=Vector3.Cross(directionVector, normal).normalized;
            Quaternion rotation = Quaternion.LookRotation(-normal,upd);

            distText.transform.rotation = rotation;
            
            //the i=only thing we have to do now is to set the size of the text and center the text between the lines.
            distText.transform.position = (pointA + directionVector * 0.5f) + upd * 0.008f;
        }

        
    }

    void Reload(){
            SceneManager.LoadScene(SceneManager.GetActiveScene().name); // reloads current scene
    }

}
