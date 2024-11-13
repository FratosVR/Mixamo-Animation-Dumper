import os
import subprocess
import shutil
from pathlib import Path
import unittest


class ObjToAssetBundleConverter:
    """Class to convert obj files to assetbundles using Unity3D
    """

    def __init__(self):
        """Constructor of the class ObjToAssetBundleConverter
        """
        # Paths

        self.__unity_editor_exe: str = "C:/Program Files/Unity/Hub/Editor/2022.3.45f1/Editor/Unity.exe"
        """path to the Unity3D editor executable"""
        self.__unity_project_path: str = os.path.join(
            os.getcwd(), "animations_to_csv_automatization")
        """path to the Unity3D project"""
        self.__project_persistentDataPath: str = os.path.join(
            Path.home(), "AppData/LocalLow/FratosVR/obj_to_assetbundle_automatization")
        """path to the Unity3D project persistent data path"""
        self.__obj_folder_path = os.path.join(
            self.__project_persistentDataPath, "Assets/ObjFiles")
        """path to the folder where the obj files are stored"""

        # Command
        self.__exec_unity = [self.__unity_editor_exe, "-batchmode", "-quit", "-projectPath",
                             self.__unity_project_path, "-executeMethod", "AssetBundleCreator.BuildAllAssetBundles"]
        """command to run the Unity3D editor in terminar to make the automatization"""

    def unity_editor_exe(self) -> str:
        """Returns thi path to the Unity3D editor executable

        Returns:
            str: path to the Unity3D editor executable
        """
        return self.__unity_editor_exe

    def unity_project_path(self) -> str:
        """Returns the path to the Unity3D project

        Returns:
            str: path to the Unity3D project
        """
        return self.__unity_project_path

    def project_persistentDataPath(self) -> str:
        """Returns the path to the Unity3D project persistent data path

        Returns:
            str: path to the Unity3D project persistent data path
        """
        return self.__project_persistentDataPath

    def obj_folder_path(self) -> str:
        """Returns the path to the folder where the obj files are stored

        Returns:
            str: path to the folder where the obj files are stored
        """

        return self.__obj_folder_path

    def copy_obj_files(self, input_folder: str) -> None:
        """Copy the obj files from the input folder to the Unity3D project folder

        Args:
            input_folder (str): path to the folder where the obj files are stored
        """
        for file in os.listdir(input_folder):
            shutil.copy(os.path.join(input_folder, file), os.path.join(
                self.__project_persistentDataPath, "ObjFiles"))

    def run_unity_converter(self):
        """Run the Unity3D editor in terminal to convert the obj files to assetbundles

        Raises:
            subprocess.CalledProcessError: if the Unity3D editor fails to create the AssetBundles
        """
        try:
            subprocess.run(self.__exec_unity, check=True)
            print("AssesBundles created successfully")
        except subprocess.CalledProcessError as e:
            print(f"Failed to create AssetBundles: {e}")


class TestConverterPaths(unittest.TestCase):
    """Unit test for the class ObjToAssetBundleConverter
    """

    def setUp(self):
        self.converter: ObjToAssetBundleConverter = ObjToAssetBundleConverter()

    def test_unity_exe_path(self):
        """Test the unity editor executable path
        """
        self.assertTrue(os.path.exists(self.converter.unity_editor_exe()))

    def test_unity_project_path(self):
        """Test the unity project path
        """
        self.assertTrue(os.path.exists(self.converter.unity_project_path()))

    def test_project_persistentDataPath(self):
        """Test the project persistent data path
        """
        self.assertTrue(os.path.exists(
            self.converter.project_persistentDataPath()))

    def test_obj_folder_path(self):
        """Test the obj folder path
        """
        self.assertTrue(os.path.exists(self.converter.obj_folder_path()))


if __name__ == "__main__":
    unittest.main()
