using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace ScenesNavigators.Core
{
    public class ScenesNavigator : EditorWindow
    {
        private Vector2 _scrollPosition;
        private bool _useMultiScene;

        private string _sceneToSearch = "";
        private bool _isOptionsActivated;

        private GUIStyle _sceneButtonStyle;
        private GUIStyle _foldoutStyle;

        [MenuItem("Tools/ScenesNavigator")]
        private static void ShowWindow()
        {
            EditorWindow.GetWindow(typeof(ScenesNavigator), false, "Scenes Navigator");
        }

        private void OnGUI()
        {
            DrawOptions();

            DrawScenesList();
        }

        private void DrawOptions()
        {
            GUILayout.BeginVertical("Box");

            _foldoutStyle = new GUIStyle(EditorStyles.foldoutHeader);
            _foldoutStyle.fontSize = 10;

            _isOptionsActivated = EditorGUILayout.BeginFoldoutHeaderGroup(_isOptionsActivated, "Options", _foldoutStyle);

            if (_isOptionsActivated)
            {
                GUILayout.BeginHorizontal();

                GUILayout.Label("Open Additive");
                _useMultiScene = EditorGUILayout.Toggle("", _useMultiScene);

                GUILayout.EndHorizontal();

                DrawSearchBar();
            }

            EditorGUILayout.EndFoldoutHeaderGroup();

            GUILayout.EndVertical();
        }

        private void DrawSearchBar()
        {
            _sceneButtonStyle = new GUIStyle(EditorStyles.toolbarButton);
            _sceneButtonStyle.fontSize = 10;

            GUILayout.BeginHorizontal();

            _sceneToSearch = GUILayout.TextField(_sceneToSearch, new GUIStyle("ToolbarSeachTextField"));

            if (_sceneToSearch != "")
            {
                if (GUILayout.Button(EditorGUIUtility.IconContent("winbtn_mac_close_h"), EditorStyles.iconButton, GUILayout.Width(18), GUILayout.Height(18)))
                    _sceneToSearch = "";
            }

            GUILayout.EndHorizontal();

            if (_sceneToSearch != "")
            {
                GUILayout.BeginVertical();

                for (int i = 0; i < EditorBuildSettings.scenes.Length; i++)
                {
                    string sceneName = GetSceneName(EditorBuildSettings.scenes[i].path);
                    sceneName = sceneName.ToLower();
                    if (!sceneName.Contains(_sceneToSearch.ToLower())) continue;

                    GUILayout.Space(5);

                    if (GUILayout.Button(GetSceneName(EditorBuildSettings.scenes[i].path), _sceneButtonStyle))
                    {
                        if (_useMultiScene)
                            EditorSceneManager.OpenScene(EditorBuildSettings.scenes[i].path, OpenSceneMode.Additive);
                        else
                        {
                            EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
                            EditorSceneManager.OpenScene(EditorBuildSettings.scenes[i].path);
                        }
                    }
                }

                GUILayout.EndVertical();
            }

        }

        private void DrawScenesList()
        {
            GUILayout.BeginVertical("Box");

            _scrollPosition = GUILayout.BeginScrollView(_scrollPosition, false, false, GUILayout.Width(this.position.width - 15), GUILayout.MinHeight(1), GUILayout.MaxHeight(1000), GUILayout.ExpandHeight(true));

            DrawAllScenes();

            GUILayout.EndScrollView();

            GUILayout.EndVertical();
        }

        private void DrawAllScenes()
        {
            _sceneButtonStyle = new GUIStyle(EditorStyles.toolbarButton);
            _sceneButtonStyle.fontSize = 10;

            for (int i = 0; i < EditorBuildSettings.scenes.Length; i++)
            {
                GUILayout.Space(5);

                if (GUILayout.Button(GetSceneName(EditorBuildSettings.scenes[i].path), _sceneButtonStyle))
                {
                    if (_useMultiScene)
                        EditorSceneManager.OpenScene(EditorBuildSettings.scenes[i].path, OpenSceneMode.Additive);
                    else
                    {
                        EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
                        EditorSceneManager.OpenScene(EditorBuildSettings.scenes[i].path);
                    }
                }
            }
        }

        private string GetSceneName(string _path)
        {
            char[] path = _path.ToCharArray();
            string sceneName = "";

            for (int i = path.Length - 1; i >= 0; i--)
            {
                if (path[i] == '/')
                    break;
                sceneName += path[i];
            }

            path = sceneName.ToCharArray();
            sceneName = "";

            for (int i = path.Length - 1; i >= 0; i--)
            {
                if (path[i] == '.')
                    break;
                sceneName += path[i];
            }

            return sceneName;
        }
    }
}