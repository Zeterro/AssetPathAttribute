using UnityEngine;

public partial class AssetPath
{
    private const string ASSET_RESOURCES_FOLDER_NAME = "Assets/Resources/";

    /// <summary>
    /// Takes the string from the Asset Path Attribute and converts it into
    /// a usable resources path.
    /// </summary>
    /// <param name="assetPath">The project path that AssetPathAttribute serializes</param>
    /// <returns>The resources path if it exists otherwise returns the same path</returns>
    public static string ConvertToResourcesPath(string projectPath)
    {
        // Make sure it's not empty
        if (string.IsNullOrEmpty(projectPath))
        {
            return string.Empty;
        }

        string convertedPath = null;
        char separator = '.';
        int lastDotIndex = -1;

        // Get the index of the last dot of the path, witch is the extension dot
        lastDotIndex = projectPath.LastIndexOf(separator);

        // Keep the begiging of the path, discard the extension
        convertedPath = projectPath.Substring(0, lastDotIndex);

        // Remove the Assets/Resources/ path, we don't need it
        convertedPath = convertedPath.Replace(ASSET_RESOURCES_FOLDER_NAME, "");

        // Return it.
        return convertedPath;
    }

    /// <summary>
    /// Loads the asset at the following path. If the asset is contained within a resources folder
    /// this uses <see cref="UnityEngine.Resources.Load(string)"/>. If we are in the Editor this will
    /// use <see cref="UnityEditor.AssetDatabase.LoadAssetAtPath(string, Type)"/> instead. This will
    /// allow you to load any type at any path. Keep in mind at Runtime the asset can only be loaded
    /// if it is inside a resources folder.
    /// </summary>
    /// <typeparam name="T">The type of object you want to load</typeparam>
    /// <param name="projectPath">The full project path of the object you are trying to load.</param>
    /// <returns>The loaded asset or null if it could not be found.</returns>
    public static T Load<T>(string projectPath) where T : UnityEngine.Object
    {
        // Make sure our path is not null
        if (string.IsNullOrEmpty(projectPath))
        {
            return null;
        }

        // Get our resources path
        string resourcesPath = ConvertToResourcesPath(projectPath);

        if (!string.IsNullOrEmpty(resourcesPath))
        {
            // The asset is in a resources folder.
            return Resources.Load<T>(resourcesPath);
        }

#if UNITY_EDITOR
        // We could not find it in resources so we just try the AssetDatabase.
        return UnityEditor.AssetDatabase.LoadAssetAtPath<T>(projectPath);
#else
        return null;
#endif
    }
}