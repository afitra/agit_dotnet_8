#!/bin/bash

# Memeriksa apakah argumen direktori proyek diberikan
if [ -z "$1" ]; then
  echo "Usage: $0 <project_directory>"
  exit 1
fi

project_directory="$1"


echo 'Running dotnet-format  all file'
echo "."
echo "."
echo "."
echo "."
echo "."
echo "."
echo "."
echo "."
echo "."

dotnet format                       

echo "."
echo "."
echo "."
echo "."
echo "."
echo "."
echo "."
echo "."
echo "."
echo 'Finish dotnet-format  all file'