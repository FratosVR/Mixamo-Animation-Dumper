import os
import subprocess
import shutil
from pathlib import Path
import unittest
import tqdm


class ObjToCSVConverter:
    """Class to convert obj files to CSV using Unity3D
    """

    def __init__(self):
        """Constructor of the class ObjToCSVConverter
        """
        # Paths

        self.__unity_editor_exe: str = "C:/Program Files/Unity/Hub/Editor/2022.3.45f1/Editor/Unity.exe"
        """path to the Unity3D editor executable"""
        self.__unity_project_path: str = os.path.join(
            os.getcwd(), "MixamoAnimationDumper")
        """path to the Unity3D project"""
        self.__project_dataPath: str = os.path.join(
            self.__unity_project_path, "Assets")
        """path to the Unity3D project persistent data path"""
        self.__fbx_folder_path = os.path.join(
            self.__project_dataPath, "Resources")
        """path to the folder where the obj files are stored"""

        # Command
        self.__build_command = [self.__unity_editor_exe, "-projectPath",
                                self.__unity_project_path, "-executeMethod", "PlayModeRunner.RunPlayMode"]
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

    def project_dataPath(self) -> str:
        """Returns the path to the Unity3D project persistent data path

        Returns:
            str: path to the Unity3D project persistent data path
        """
        return self.__project_dataPath

    def fbx_folder_path(self) -> str:
        """Returns the path to the folder where the obj files are stored

        Returns:
            str: path to the folder where the obj files are stored
        """

        return self.__fbx_folder_path

    def copy_fbx_files(self, input_folder: str) -> None:
        """Copy the obj files from the input folder to the Unity3D project folder

        Args:
            input_folder (str): path to the folder where the obj files are stored
        """
        shutil.copytree(input_folder, self.__fbx_folder_path,
                        dirs_exist_ok=True)

    def run_unity_converter(self):
        """Run the Unity3D editor in terminal to convert the obj files to assetbundles

        Raises:
            subprocess.CalledProcessError: if the Unity3D editor fails to create the AssetBundles
        """
        try:
            subprocess.run(self.__build_command, check=True)
            print("Build completed successfully!")
        except subprocess.CalledProcessError as e:
            print(f"Failed to create AssetBundles: {e}")


class TestConverterPaths(unittest.TestCase):
    """Unit test for the class ObjToAssetBundleConverter
    """

    def setUp(self):
        self.converter: ObjToCSVConverter = ObjToCSVConverter()

    def test_unity_exe_path(self):
        """Test the unity editor executable path
        """
        self.assertTrue(os.path.exists(self.converter.unity_editor_exe()))

    def test_unity_project_path(self):
        """Test the unity project path
        """
        self.assertTrue(os.path.exists(self.converter.unity_project_path()))

    def test_project_dataPath(self):
        """Test the project persistent data path
        """
        self.assertTrue(os.path.exists(
            self.converter.project_dataPath()))

    def test_fbx_folder_path(self):
        """Test the fbx folder path
        """
        self.assertTrue(os.path.exists(self.converter.fbx_folder_path()))


if __name__ == "__main__":
    unittest.main()
