using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Linq;

[CustomEditor(typeof(StructureRepresentationLib))]
public class StructureRepresentationLibEditor : Editor
{

    static readonly string[] IGNORED_ASSEMBLY_PREFIXES = {
        "UnityEditor",
        "UnityEngine",
        "Unity",
        "System",
        "mscorlib"
    };

    private TypeObjectLibrary<StructureBase, StructureWorldRepresentation> library;
    private Type[] derivedTypes;

    void OnEnable()
    {
        library = target as TypeObjectLibrary<StructureBase, StructureWorldRepresentation>;
        derivedTypes = System.AppDomain.CurrentDomain.GetAssemblies()
        .Where(assembly => !IGNORED_ASSEMBLY_PREFIXES.Any(prefix => assembly.FullName.StartsWith(prefix)))
        .SelectMany(x => x.GetTypes())
        .Where((t) => (!t.IsAbstract) && (!t.IsGenericType) && t.IsSubclassOf(typeof(StructureBase)))
        .Where(IsValidType)
        .ToArray();        
        derivedTypes.OrderBy(x => x.Name);
    }

    public virtual bool IsValidType(Type type) => true;

    public override void OnInspectorGUI()
    {
        if (library == null)
            return;

        foreach (var type in derivedTypes)
            library[type] = EditorGUILayout.ObjectField(type.Name, library[type], typeof(StructureWorldRepresentation), allowSceneObjects: true) as StructureWorldRepresentation;
        if (GUI.changed)
            EditorUtility.SetDirty(library);

    }    
}
