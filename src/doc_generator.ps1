# PowerShell Script to Generate Sphinx Documentation in HTML and LaTeX

param (
    [string]$sourceDir = ".\source", # Folder containing Python files
    [string]$outputDir = ".\docs"    # Folder for documentation output
)

# Ensure Sphinx is installed
if (!(Get-Command sphinx-quickstart -ErrorAction SilentlyContinue)) {
    Write-Output "Sphinx is not installed. Installing..."
    pip install sphinx
}

# Initialize Sphinx if the documentation folder doesn't already exist
if (!(Test-Path $outputDir)) {
    sphinx-quickstart -q -p "FBX to CSV automatization" -a "Alejandro Barrachina Argudo, Pablo Sánchez Martín" -v "1.0" -l en -t "" `
        --sep --makefile --batchfile `
        --ext-autodoc --ext-viewcode `
        --quiet `
        $outputDir
}

# Copy Python files to the source directory within the docs folder
if (!(Test-Path "$outputDir\source")) {
    New-Item -ItemType Directory -Path "$outputDir\source"
}

# Generate documentation for all Python files in the source directory
Write-Output "Generating documentation for all Python files in $sourceDir..."
sphinx-apidoc -o "$outputDir\source" $sourceDir -f

# Build HTML and LaTeX versions of the documentation
sphinx-build -b html "$outputDir\source" "$outputDir\html"
sphinx-build -b latex "$outputDir\source" "$outputDir\latex"

Write-Output "Documentation generated in HTML and LaTeX formats in the docs folder."
