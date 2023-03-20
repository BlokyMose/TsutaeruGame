using PlasticGui.WorkspaceWindow;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace UnityUtility.Editor
{
    public static class CreateScriptTemplate
    {
        const int PRIORIT_INDEX = -100;

        [MenuItem("Assets/Create/New Code/MonoBehaviour", priority = PRIORIT_INDEX)]
        public static void CreateMonoBehaviour() => CreateScriptFromTemplateName("MonoBehaviour");

        [MenuItem("Assets/Create/New Code/ScriptableObject", priority = PRIORIT_INDEX)]
        public static void CreateScriptableObject() => CreateScriptFromTemplateName("ScriptableObject");

        [MenuItem("Assets/Create/New Code/Static Class", priority = PRIORIT_INDEX)]
        public static void CreateStaticClass() => CreateScriptFromTemplateName("StaticClass");

        public static void CreateScriptFromTemplateName(string templateName)
        {
            var TEMPLATES = "Templates";
            var parentPath = GetParentPath(nameof(CreateScriptTemplate));
            var templatePath = parentPath + "/" + TEMPLATES + "/" + templateName + ".cs.txt";

            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(templatePath, "New"+templateName+".cs");
        }

        public static string GetParentPath(string assetName)
        {
            var guids = AssetDatabase.FindAssets(assetName);
            var path = AssetDatabase.GUIDToAssetPath(guids[0]);
            var folderPath = path[..(path.Length - "/".Length - assetName.Length - ".cs".Length )];
            return folderPath;
        }
    }
}
