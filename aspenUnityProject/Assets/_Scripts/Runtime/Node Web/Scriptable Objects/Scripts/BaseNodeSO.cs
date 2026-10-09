using System.Collections.Generic;
using UnityEngine;

namespace Slinky.NodeWeb
{
    public class BaseNodeSO : ScriptableObject
    {
        [field: SerializeField] public string GUID { get; private set; }
        [field: SerializeField] public bool IsUnlocked { get; private set; } = false;
        [field: SerializeField] public List<string> PrerequisitesIDs {  get; private set; } = new List<string>();
        [field: SerializeField] public List<string> UnlockIDs {  get; private set; } = new List<string>();
        #region EDITOR_STUFF
        [field: SerializeField] public Vector2 EditorPosition { get; private set; }
        #endregion

        public void SetGUID(string id) { GUID = id; }
        public void SetEditorPosition(Vector2 editorPosition) { EditorPosition = editorPosition; }
        public void SetIsUnlocked(bool isUnlocked) { IsUnlocked = isUnlocked; }
    }
}
