using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PG
{
    public interface Interactable 
    {
        void OnHoverEnter();
        void OnHoverStay();
        void OnHoverExit();
        void OnClick();
    }
}
