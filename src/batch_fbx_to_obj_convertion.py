import os
import json
import tqdm
import sys
import shutil
import kaggle

from kaggle.api.kaggle_api_extended import KaggleApi
from objtocsvconverter import ObjToCSVConverter


def copy_csv_files(input_folder: str, output_folder: str) -> None:
    """Copy the csv files from the input folder to the output folder

    Args:
        input_folder (str): path to the folder where the csv files are stored
        output_folder (str): path to the folder where the csv files will be stored
    """
    shutil.copytree(input_folder, output_folder,
                    dirs_exist_ok=True)
    for item in os.listdir(output_folder):
        if item.endswith(".meta"):
            os.remove(os.path.join(output_folder, item))


def main():
    api = KaggleApi()
    api.authenticate()

    print("Downloading dataset")
    api.dataset_download_cli(
        'alk222/raw-mixamo-animations', path='./dataset', unzip=True)
    metadata = kaggle.api.dataset_metadata(
        'alk222/raw-mixamo-animations', path='./dataset')

    converter = ObjToCSVConverter()
    # print("Copying fbx files")
    # converter.copy_fbx_files("./dataset")
    # print("Running Unity converter")
    # converter.run_unity_converter()
    # print("Finished conversion")
    # print("Downloading latest version of csv dataset")
    # api.dataset_download_cli('alk222/csv-pose-animations',
    #                          path='./csv_dataset', unzip=True)
    # metadata = kaggle.api.dataset_metadata(
    #     'alk222/csv-pose-animations', path='./csv_dataset')
    print("Updating csv dataset")
    copy_csv_files(converter.csv_folder_path(), "./csv_dataset")
    print("Uploading csv dataset")
    api.dataset_create_version(
        "csv_dataset", version_notes="new animations")


if __name__ == "__main__":
    main()
