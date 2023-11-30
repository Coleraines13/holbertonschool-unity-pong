using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZPong
{

    public class Goal : MonoBehaviour
    {
        private BoxCollider2D myCollider;


        // Start is called before the first frame update
        void Start()
        {
            myCollider = GetComponent<BoxCollider2D>();

            SetHeightBounds();

            GameManager.Instance.SetGoalObj(this);
        }

        public void SetHeightBounds()
        {
            myCollider.size = new Vector2(myCollider.size.x, UIScaler.Instance.GetUIHeight());
        }
    }

}