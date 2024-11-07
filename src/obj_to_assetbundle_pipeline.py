import os
import subprocess


class ObjToAssetbundleConversor:

    # Paths
    self.__unity_editor_exe = "C:/Program Files/Unity/Hub/Editor/2022.3.45f1/Editor/Unity.exe"
    self.__unity_project_path = os.path.join(
        os.getcwd(), "animations_to_csv_automatization")
    self.__project_persistentDataPath = "C:/Users/alex_/AppData/LocalLow/FratosVR/obj_to_assetbundle_automatization"
    self.__obj_folder_path = os.path.join(
        self.__project_persistentDataPath, "Assets/ObjFiles")

    # Commands
    self.exec_unity = [self.__unity_editor_exe, "-batchmode", "-quit", "-projectPath",
                       self.__unity_project_path, "-executeMethod", "AssetBundleCreator.BuildAllAssetBundles"]


# Paths (adjust if necessary)
unity_project_path = os.path.join(
    os.getcwd(), "obj_to_assetbundle_automatization")
obj_folder_path = os.path.join(unity_project_path, "Assets/ObjFiles")
output_bundle_path = os.path.join(unity_project_path, "AssetBundles")

# Run Unity in batch mode to execute the asset bundle creation
unity_editor = "C:/Program Files/Unity/Hub/Editor/2022.3.45f1/Editor/Unity.exe"
try:
    subprocess.run([
        unity_editor, "-batchmode", "-quit",
        "-projectPath", unity_project_path,
        "-executeMethod", "AssetBundleCreator.BuildAllAssetBundles"
    ], check=True)
    print("Asset Bundles created successfully.")
except subprocess.CalledProcessError as e:
    print(f"Failed to create asset bundles: {e}")
