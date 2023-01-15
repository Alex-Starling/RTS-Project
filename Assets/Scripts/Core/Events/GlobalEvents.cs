using RTSEngine.DataBase;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RTSEngine.Events 
{
    public static class GlobalEvents
    {
        
    }
    public static class InputEvents
    {
        private static bool MouseUp;
        public static Action eventClick;
        public static Action<bool> eventClickPass;
        public static Action eventRightClick; 
        public static void CallOnMouseClick()
        {
            eventClick?.Invoke();
        }
        public static void CallOnMouseRightClick()
        {
            eventRightClick?.Invoke();
        }
        public static void CallOnMouseClickPass()
        {
            eventClickPass?.Invoke(MouseUp);
            MouseUp = !MouseUp;
        }
    }
    public static class SelectionEvents
    {
        public static Action<UnitModel> eventSelectObject;
        public static void CallOnSelectObject(UnitModel _selectedObject)
        {
            eventSelectObject?.Invoke(_selectedObject);
        }
        public struct SelectedObject
        {
            public string selectedObjectKey;
            public UnitType selectedObjectType;

            public UnitModel selectedUnitEntity;
        }
    }
   
}

