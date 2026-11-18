using System;
using System.Collections.Generic;
using _Scripts.Runtime.Entities.Scripts;
using _Scripts.Runtime.Entities.Scripts.Combat;
using AYellowpaper.SerializedCollections;
using Sirenix.OdinInspector;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using Sirenix.Utilities.Editor;
#endif

//misc directory is temp name? 
namespace _Scripts.Runtime.Combat
{
    [CreateAssetMenu(fileName = "NewEncounter", menuName = "Scriptable Objects/Misc/Encounter")]
    public class EncounterSO : ScriptableObject
    {
        public const int TILE_COUNT = CombatTileCoordinates.TileCount;
        public const int TILE_MAX_UNITS = 4;

        [System.Serializable]
        public class UnitListWrapper
        {
            public List<UnitDataSO> Units = new();
        }
        
        [field: SerializeField, HideInInspector] public List<UnitListWrapper> Tiles { get; private set; } = new List<UnitListWrapper>();

#if UNITY_EDITOR
        [NonSerialized] private int _selectedTileIndex;
        private static GUIStyle _tileTitleStyle;
        private static GUIStyle _tileUnitStyle;

        /// <summary>
        /// Shows the encounter's 19 serialized tile slots in the same center-out spiral used by CombatManager,
        /// arranged spatially as a three-ring hex grid.
        /// </summary>
        [OnInspectorGUI]
        private void DrawEncounterGrid()
        {
            EnsureTileData();

            SirenixEditorGUI.BeginBox("Encounter Layout");
            EditorGUILayout.LabelField("Select a tile, then place up to four units on it.", EditorStyles.miniLabel);

            IReadOnlyList<Vector3Int> coordinatesByTileIndex = CombatTileCoordinates.GetAll();
            float tileSize = Mathf.Clamp((EditorGUIUtility.currentViewWidth - 110f) / 5f - 4f, 56f, 88f);
            // Draw negative z at the top so the inspector matches the combat board orientation.
            for (int z = -CombatTileCoordinates.Radius; z <= CombatTileCoordinates.Radius; z++)
            {
                List<int> rowTileIndices = new List<int>();
                for (int x = -CombatTileCoordinates.Radius; x <= CombatTileCoordinates.Radius; x++)
                {
                    int y = -x - z;
                    if (Mathf.Abs(y) > CombatTileCoordinates.Radius)
                        continue;

                    if (CombatTileCoordinates.TryGetTileIndex(new Vector3Int(x, y, z), out int tileIndex))
                        rowTileIndices.Add(tileIndex);
                }

                DrawGridRow(rowTileIndices, tileSize);
                EditorGUILayout.Space(3f);
            }

            DrawSelectedTile(coordinatesByTileIndex);
            SirenixEditorGUI.EndBox();
        }

        private void DrawGridRow(List<int> tileIndices, float tileSize)
        {
            const float spacing = 4f;
            Rect rowRect = EditorGUILayout.GetControlRect(false, tileSize);
            float rowWidth = tileIndices.Count * tileSize + (tileIndices.Count - 1) * spacing;
            float startX = rowRect.x + (rowRect.width - rowWidth) * 0.5f;

            for (int i = 0; i < tileIndices.Count; i++)
            {
                int tileIndex = tileIndices[i];
                Rect tileRect = new Rect(startX + i * (tileSize + spacing), rowRect.y, tileSize, tileSize);
                bool selected = DrawTileButton(tileRect, tileIndex);
                if (selected)
                    _selectedTileIndex = tileIndex;
            }
        }

        private bool DrawTileButton(Rect rect, int tileIndex)
        {
            bool isSelected = _selectedTileIndex == tileIndex;
            bool isHovered = rect.Contains(Event.current.mousePosition);
            Color fillColor = EditorGUIUtility.isProSkin
                ? new Color(0.24f, 0.24f, 0.24f)
                : new Color(0.78f, 0.78f, 0.78f);
            if (isHovered)
                fillColor *= 1.15f;
            if (isSelected)
                fillColor = EditorGUIUtility.isProSkin
                    ? new Color(0.20f, 0.38f, 0.50f)
                    : new Color(0.48f, 0.68f, 0.84f);

            EditorGUI.DrawRect(rect, fillColor);
            Color borderColor = EditorGUIUtility.isProSkin
                ? new Color(0.10f, 0.10f, 0.10f)
                : new Color(0.40f, 0.40f, 0.40f);
            EditorGUI.DrawRect(new Rect(rect.x, rect.y, rect.width, 1f), borderColor);
            EditorGUI.DrawRect(new Rect(rect.x, rect.yMax - 1f, rect.width, 1f), borderColor);
            EditorGUI.DrawRect(new Rect(rect.x, rect.y, 1f, rect.height), borderColor);
            EditorGUI.DrawRect(new Rect(rect.xMax - 1f, rect.y, 1f, rect.height), borderColor);

            bool clicked = GUI.Button(rect, GUIContent.none, GUIStyle.none);

            float padding = Mathf.Max(2f, rect.width * 0.04f);
            float titleHeight = rect.height * 0.20f;
            UnitListWrapper tile = Tiles[tileIndex];
            GUI.Label(
                new Rect(rect.x + padding, rect.y + 2f, rect.width - padding * 2f, titleHeight),
                $"Tile {tileIndex} ({tile.Units.Count}/{TILE_MAX_UNITS})",
                GetTileTitleStyle());

            float contentTop = rect.y + titleHeight;
            float unitLineHeight = (rect.height - titleHeight - 2f) / TILE_MAX_UNITS;
            for (int unitIndex = 0; unitIndex < tile.Units.Count; unitIndex++)
            {
                UnitDataSO unit = tile.Units[unitIndex];
                string unitName = unit == null
                    ? "(Empty)"
                    : string.IsNullOrWhiteSpace(unit.Name) ? unit.name : unit.Name;

                GUI.Label(
                    new Rect(
                        rect.x + padding,
                        contentTop + unitIndex * unitLineHeight,
                        rect.width - padding * 2f,
                        unitLineHeight),
                    unitName,
                    GetTileUnitStyle());
            }

            return clicked;
        }

        private static GUIStyle GetTileTitleStyle()
        {
            if (_tileTitleStyle == null)
            {
                _tileTitleStyle = new GUIStyle(EditorStyles.miniBoldLabel)
                {
                    alignment = TextAnchor.MiddleCenter,
                    wordWrap = false,
                    clipping = TextClipping.Ellipsis
                };
            }

            return _tileTitleStyle;
        }

        private static GUIStyle GetTileUnitStyle()
        {
            if (_tileUnitStyle == null)
            {
                _tileUnitStyle = new GUIStyle(EditorStyles.miniLabel)
                {
                    alignment = TextAnchor.MiddleCenter,
                    wordWrap = false,
                    clipping = TextClipping.Ellipsis
                };
            }

            return _tileUnitStyle;
        }

        private void DrawSelectedTile(IReadOnlyList<Vector3Int> coordinatesByTileIndex)
        {
            _selectedTileIndex = Mathf.Clamp(_selectedTileIndex, 0, TILE_COUNT - 1);
            UnitListWrapper tile = Tiles[_selectedTileIndex];
            Vector3Int coordinate = coordinatesByTileIndex[_selectedTileIndex];
            int ring = Mathf.Max(Mathf.Abs(coordinate.x), Mathf.Abs(coordinate.y), Mathf.Abs(coordinate.z));

            EditorGUILayout.Space(6f);
            EditorGUILayout.LabelField($"Tile {_selectedTileIndex}  |  Ring {ring}", EditorStyles.boldLabel);
            EditorGUI.BeginChangeCheck();
            Undo.RecordObject(this, "Edit Encounter Layout");

            for (int unitIndex = 0; unitIndex < tile.Units.Count; unitIndex++)
            {
                GUILayout.BeginHorizontal();
                tile.Units[unitIndex] = (UnitDataSO)EditorGUILayout.ObjectField(
                    $"Unit {unitIndex + 1}", tile.Units[unitIndex], typeof(UnitDataSO), false);

                if (GUILayout.Button("Remove", EditorStyles.miniButton, GUILayout.Width(58f)))
                {
                    tile.Units.RemoveAt(unitIndex);
                    unitIndex--;
                }
                GUILayout.EndHorizontal();
            }

            using (new EditorGUI.DisabledScope(tile.Units.Count >= TILE_MAX_UNITS))
            {
                if (GUILayout.Button("+ Add Unit", GUILayout.Height(30f)))
                    tile.Units.Add(null);
            }

            if (EditorGUI.EndChangeCheck())
                EditorUtility.SetDirty(this);

        }

        private void EnsureTileData()
        {
            if (Tiles == null)
                Tiles = new List<UnitListWrapper>();

            while (Tiles.Count < TILE_COUNT)
                Tiles.Add(new UnitListWrapper());

            if (Tiles.Count > TILE_COUNT)
                Tiles.RemoveRange(TILE_COUNT, Tiles.Count - TILE_COUNT);

            for (int i = 0; i < Tiles.Count; i++)
            {
                if (Tiles[i] == null)
                    Tiles[i] = new UnitListWrapper();
                if (Tiles[i].Units == null)
                    Tiles[i].Units = new List<UnitDataSO>();
                if (Tiles[i].Units.Count > TILE_MAX_UNITS)
                    Tiles[i].Units.RemoveRange(TILE_MAX_UNITS, Tiles[i].Units.Count - TILE_MAX_UNITS);
            }
        }

#endif
        
        private void OnValidate()
        {
            Validate();
        }

        public void Validate()
        {
            if (Tiles == null)
                Tiles = new List<UnitListWrapper>();

            while (Tiles.Count < TILE_COUNT)
                Tiles.Add(new UnitListWrapper());
            
            if (Tiles.Count > TILE_COUNT)
                Tiles = Tiles.GetRange(0, TILE_COUNT);

            for (int i = 0; i < Tiles.Count; i++)
            {
                UnitListWrapper listWrapper = Tiles[i];
                if (listWrapper == null)
                    Tiles[i] = listWrapper = new UnitListWrapper();
                if (listWrapper.Units == null)
                    listWrapper.Units = new List<UnitDataSO>();

                // Truncate the list if more than TILE_MAX_UNITS
                if (listWrapper.Units.Count > TILE_MAX_UNITS)
                    listWrapper.Units = listWrapper.Units.GetRange(0, TILE_MAX_UNITS);
            }
        }
        
        [Button("Clear Empty Units", ButtonSizes.Large)]
        public void CleanNullUnits()
        {
            foreach (UnitListWrapper listWrapper in Tiles)
            {
                // Remove null units starting from the end
                for (int i = listWrapper.Units.Count - 1; i >= 0; i--)
                {
                    if(listWrapper.Units[i] ==null)
                        listWrapper.Units.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// Creates a runtime instance of this encounter with the same units, but can be modified
        /// </summary>
        public EncounterSO CreateRuntimeCopy()
        {
            EncounterSO copy = ScriptableObject.CreateInstance<EncounterSO>();
            copy.Tiles = new List<UnitListWrapper>(Tiles);
            return copy;
        }
        
        /// <summary>
        /// Creates a runtime instance of an encounter with empty data
        /// </summary>
        public static EncounterSO CreateEmptyRuntimeInstance()
        {
            EncounterSO instance = ScriptableObject.CreateInstance<EncounterSO>();
            instance.Validate();
            return instance;
        }
        
        /// <summary>
        /// Creates a Encounter data runtime instance from battle sim configuration
        /// </summary>
        public static EncounterSO CreateRuntimeInstanceFromBattleSim(List<BattlefieldTile> tiles)
        {
            EncounterSO encounterData = CreateEmptyRuntimeInstance();
           
            for (int tileIndex = 0; tileIndex < tiles.Count; tileIndex++)
            {
                if (tiles[tileIndex].FilledSlots.Count < 1)
                    continue;

                foreach (UnitPieceSlot unitSlot in tiles[tileIndex].FilledSlots)
                    encounterData.TryAddUnit(tileIndex, unitSlot.Piece.Sim.Data);
            }

            return encounterData;
        }

        public bool TryAddUnit(int tileIndex, UnitDataSO unitData)
        {
            if (tileIndex < 0 || tileIndex >= TILE_COUNT)
            {
                Debug.LogWarning($"Can't add unit in invalid tileIndex {tileIndex}");
                return false;
            }

            if (unitData == null)
            {
                Debug.LogWarning($"Can't add null unit at tileIndex {tileIndex}");
                return false;
            }

            if (Tiles[tileIndex].Units.Count >= TILE_MAX_UNITS)
            {
                Debug.LogWarning($"Can't add unit to full tile at tileIndex {tileIndex}");
                return false;
            }
            
            Tiles[tileIndex].Units.Add(unitData);
            return true;
        }

        public bool TryRemoveUnit(int tileIndex, UnitDataSO unitData)
        {
            if (tileIndex < 0 || tileIndex >= TILE_COUNT)
            {
                Debug.LogWarning($"Can't remove unit in invalid tileIndex {tileIndex}");
                return false;
            }

            if (unitData == null)
            {
                Debug.LogWarning($"Can't remove null unit at tileIndex {tileIndex}");
                return false;
            }

            if (!Tiles[tileIndex].Units.Contains(unitData))
            {
                Debug.LogWarning($"Can't remove nonexistent unit {unitData.name} at tileIndex {tileIndex}");
                return false;
            }
            
            Tiles[tileIndex].Units.Remove(unitData);
            return true;
        }
    }
}
