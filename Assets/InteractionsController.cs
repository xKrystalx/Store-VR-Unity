using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Valve.VR.InteractionSystem{
    [RequireComponent( typeof( Interactable ) )]
	[RequireComponent( typeof( Rigidbody ) )]
    public class InteractionsController : MonoBehaviour{

        Transform initialParent;
        public bool isHeld;
        private int initialLayer;
        // Start is called before the first frame update
        void Start()
        {
            isHeld = false;
            //initialParent = gameObject.transform.parent;
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        protected virtual void OnHandHoverBegin(Hand hand)
        {
            // GameObject canvas = gameObject.transform.Find("ProductCanvas").gameObject;
            // if(canvas != null){
            //     canvas.SetActive(true);
            // }
        }
        protected virtual void OnHandHoverEnd(Hand hand)
        {
            // GameObject canvas = gameObject.transform.Find("ProductCanvas").gameObject;
            // if(canvas != null){
            //     canvas.SetActive(false);
            // }
            
        }

        protected virtual void OnAttachedToHand(Hand hand){
            // if(!isTouching){
            //     gameObject.GetComponent<Rigidbody>().isKinematic = true;
            // }
            isHeld = true;
            initialLayer = this.gameObject.layer;
            this.gameObject.layer = 8;
        }
        
        protected virtual void OnDetachedFromHand(Hand hand){
            // if(!isTouching){
            //     gameObject.GetComponent<Rigidbody>().isKinematic = false;
            // }
            // else{
            //     gameObject.GetComponent<Rigidbody>().isKinematic = true;
            // }
            // gameObject.GetComponent<Rigidbody>().isKinematic = false;
            isHeld = false;
            this.gameObject.layer = initialLayer;
        }
    }

}
