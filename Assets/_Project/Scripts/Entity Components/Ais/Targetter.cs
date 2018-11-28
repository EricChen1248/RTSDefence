using System;
using System.Collections.Generic;
using System.Linq;
using Scripts.Controllers;
using Scripts.Scriptable_Objects;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Scripts.Entity_Components.Ais
{
    public static class Targetter
    {
        public enum BuildType { ActiveDefence, StaticDefence, Structure, Traps}

        public static Transform GetClosestBuildable(Vector3 position, BuildType type)
        {
            var builder = CoreController.BuildController;

            BuildData[] list;
            switch (type)
            {
                case BuildType.ActiveDefence:
                    list = builder.ActiveDefences;
                    break;
                case BuildType.StaticDefence:
                    list = builder.StaticDefences;
                    break;
                case BuildType.Structure:
                    list = builder.Structure;
                    break;
                case BuildType.Traps:
                    list = builder.Traps;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }

            var dictionary = new Dictionary<int, GameObject>();
            foreach (var buildData in list)
            {
                builder.BuiltObjects[buildData].ToList().ForEach(pair => dictionary[pair.Key] = pair.Value);
            }
            
            return dictionary.OrderBy(key => Vector3.SqrMagnitude(key.Value.transform.position - position)).First().Value.transform;
        }

        public static Transform GetRandomBuildable(Vector3 position, BuildType type)
        {
            var builder = CoreController.BuildController;

            BuildData[] list;
            switch (type)
            {
                case BuildType.ActiveDefence:
                    list = builder.ActiveDefences;
                    break;
                case BuildType.StaticDefence:
                    list = builder.StaticDefences;
                    break;
                case BuildType.Structure:
                    list = builder.Structure;
                    break;
                case BuildType.Traps:
                    list = builder.Traps;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }

            var dictionary = new Dictionary<int, GameObject>();
            foreach (var buildData in list)
            {
                builder.BuiltObjects[buildData].ToList().ForEach(pair => dictionary[pair.Key] = pair.Value);
            }

            return dictionary.ToList()[Random.Range(0, dictionary.Count)].Value.transform;
        }
    }
}
