// Assets/Scripts/UI/ItemIconConfig.cs
using System;
using UnityEngine;
using HoneyBearExpress.Buildings;

namespace HoneyBearExpress.UI
{
    [Serializable]
    public struct ItemIconConfig
    {
        public HoneyItemType Type;
        public Sprite Icon;
    }
}
