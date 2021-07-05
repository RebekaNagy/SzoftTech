using System.Collections.Generic;
using UnityEngine;

namespace Assets.View
{
    public static class ViewPrefabManager
    {
        private static readonly Dictionary<string, GameObject> Views; //Unityben létrehozott objektumok prefabok listája.

        static ViewPrefabManager() //Ezek betöltése.
        {
            Views = new Dictionary<string, GameObject>();
            var viewArray = Resources.LoadAll<GameObject>("ViewPrefabs");
            foreach (var view in viewArray)
            {
                Views.Add(view.name, view);
            }
        }

        public static GameObject GetViewObject(string viewName) //Getter
        {
            return !Views.ContainsKey(viewName) ? null : Views[viewName];
        }
    }
}
