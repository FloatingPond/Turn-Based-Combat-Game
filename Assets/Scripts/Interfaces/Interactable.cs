using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PG
{
    public interface IInteractable 
    {
        void OnHoverEnter();
        void OnHoverStay();
        void OnHoverExit();
        void OnClick();
    }
}
