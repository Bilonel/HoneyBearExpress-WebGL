// Assets/Scripts/Core/QuestDatabase.cs
using System;
using UnityEngine;
using HoneyBearExpress.Buildings;

namespace HoneyBearExpress.Core
{
    [Serializable]
    public struct QuestRequirement
    {
        public HoneyItemType Type;
        public int BaseAmount;
    }

    [Serializable]
    public struct QuestTemplate
    {
        public int TargetLevel; // Bu şablonun görüneceği oyuncu seviyesi
        public QuestRequirement[] Requirements; // İstenen eşyalar (Max 2 tip)
        public int BaseCoinReward;
        public int BaseXpReward;
    }

    [CreateAssetMenu(fileName = "QuestDatabase", menuName = "HoneyBearExpress/Quest Database")]
    public class QuestDatabase : ScriptableObject
    {
        [Header("Müşteri İsimleri (TXT yerine Optimize Dizi)")]
        public string[] customerNames;

        [Header("Seviye Bazlı Görev Havuzu")]
        public QuestTemplate[] templates;
    }
}
