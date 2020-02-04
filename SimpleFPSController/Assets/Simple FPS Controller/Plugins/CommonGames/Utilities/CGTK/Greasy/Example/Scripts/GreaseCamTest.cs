using UnityEngine;

using CommonGames.Utilities.CGTK.Greasy;

public class GreaseCamTest : MonoBehaviour
{
    //public Transform enemy;

    //[InfoBox("Material ought to be assigned In-Editor.", InfoMessageType.Warning)]
    [SerializeField] private Material material;
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            ///material.color
            transform.ScaleTo(transform.localScale + Vector3.one, .5f, EaseType.ElasticInOut);

            //Camera.main.FieldOfViewTo(90, 1f, EaseType.ElasticOut);
        }
        
        if (Input.GetKeyDown(KeyCode.Q))
        {
            transform.ScaleTo(transform.localScale - Vector3.one, .5f, EaseType.ElasticInOut);            
        }
        
    }
}
