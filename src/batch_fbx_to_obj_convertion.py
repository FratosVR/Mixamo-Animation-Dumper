import bpy
import os
import json
import tqdm
import sys
import shutil

from kaggle.api.kaggle_api_extended import KaggleApi


def main():
    api = KaggleApi()
    api.authenticate()

    print("Downloading dataset")
    api.dataset_download_cli(
        'alk222/raw-mixamo-animations', path='./dataset', unzip=True)

    path = "./dataset"

    subfolders = [f.path for f in os.scandir(path) if f.is_dir()]

    print("Conversion from fbx to obj")
    for subfolder in subfolders:
        files = os.listdir(subfolder)
        for f in tqdm.tqdm(files, file=sys.stderr):
            if f.endswith('.fbx'):
                # print("Proccessing file: " + f)
                mesh_file = os.path.join(subfolder, f)
                obj_file = os.path.splitext(mesh_file)[0] + ".obj"

                bpy.ops.object.select_all(action='SELECT')
                bpy.ops.object.delete()

                bpy.ops.import_scene.fbx(filepath=mesh_file)

                bpy.ops.object.select_all(action='SELECT')

                bpy.ops.wm.obj_export(filepath=obj_file)

    print("Uploading dataset")
    api.dataset_create_version(
        folder="./dataset",
        version_notes='Updated dataset with new data',
        convert_to_csv=False,  # Set to True if you want to convert files to CSV
        dir_mode='zip'  # Set to 'overwrite' if you want to replace existing files
    )

    print("Copying obj files to unity asset folder")

    for subfolder in subfolders:
        files = os.listdir(subfolder)
        for file in tqdm.tqdm(files, file=sys.stderr):
            if file.endswith('.obj'):
                shutil.copy(os.path.join(subfolder, file),
                            './obj_to_assetbundle_automatization/Assets/animations')


if __name__ == "__main__":
    main()
