import os
import json
import tqdm
import sys
import shutil
import kaggle

from kaggle.api.kaggle_api_extended import KaggleApi
from objtocsvconverter import ObjToCSVConverter


def main():
    api = KaggleApi()
    api.authenticate()

    print("Downloading dataset")
    api.dataset_download_cli(
        'alk222/raw-mixamo-animations', path='./dataset', unzip=True)
    metadata = kaggle.api.dataset_metadata(
        'alk222/raw-mixamo-animations', path='./dataset')

    converter = ObjToCSVConverter()
    print("Copying fbx files")
    converter.copy_fbx_files("./dataset")
    converter.run_unity_converter()


if __name__ == "__main__":
    main()
