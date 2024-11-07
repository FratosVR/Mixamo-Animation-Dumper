import os
import subprocess
import shutil
from pathlib import Path
import unittest


class ObjToAssetbundleConversor:

    def __init__(self):

        # Paths
        self.__unity_editor_exe: str = "C:/Program Files/Unity/Hub/Editor/2022.3.45f1/Editor/Unity.exe"
        self.__unity_project_path: str = os.path.join(
            os.getcwd(), "animations_to_csv_automatization")
        self.__project_persistentDataPath: str = os.path.join(
            Path.home(), "AppData/LocalLow/FratosVR/obj_to_assetbundle_automatization")
        self.__obj_folder_path = os.path.join(
            self.__project_persistentDataPath, "Assets/ObjFiles")
        # Command
        self.__exec_unity = [self.__unity_editor_exe, "-batchmode", "-quit", "-projectPath",
                             self.__unity_project_path, "-executeMethod", "AssetBundleCreator.BuildAllAssetBundles"]

    def unity_editor_exe(self) -> str:
        return self.__unity_editor_exe

    def unity_project_path(self) -> str:
        return self.__unity_project_path

    def project_persistentDataPath(self) -> str:
        return self.__project_persistentDataPath

    def obj_folder_path(self) -> str:
        return self.__obj_folder_path

    def copy_obj_files(self, input_folder: str) -> None:
        for file in os.listdir(input_folder):
            shutil.copy(os.path.join(input_folder, file), os.path.join(
                self.__project_persistentDataPath, "ObjFiles"))

    def run_unity_conversor(self):
        try:
            subprocess.run(self.__exec_unity, check=True)
            print("AssesBundles created successfully")
        except subprocess.CalledProcessError as e:
            print(f"Failed to create AssetBundles: {e}")


class TestConversorPaths(unittest.TestCase):
    def setUp(self):
        self.conversor: ObjToAssetbundleConversor = ObjToAssetbundleConversor()

    def test_unity_exe_path(self):
        self.assertTrue(os.path.exists(self.conversor.unity_editor_exe()))

    def test_unity_project_path(self):
        self.assertTrue(os.path.exists(self.conversor.unity_project_path()))

    def test_project_persistentDataPath(self):
        self.assertTrue(os.path.exists(
            self.conversor.project_persistentDataPath()))

    def test_obj_folder_path(self):
        self.assertTrue(os.path.exists(self.conversor.obj_folder_path()))


if __name__ == "__main__":
    unittest.main()
