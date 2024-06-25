#!/bin/bash

# Recursive function to replace text in files and rename directories and filenames
replace_text() {
  for file in "$1"/*; do
    if [ -d "$file" ]; then
      # Rename directories
      new_dir="${file/dotnet-template-app/$PROJECT_NAME}"
      if [ "$file" != "$new_dir" ]; then
        mv "$file" "$new_dir"
      fi
      replace_text "$new_dir"   # Recursively process subdirectories
    elif [ -f "$file" ]; then
      # Rename files and replace text in .csproj files
      new_file="${file/dotnet-template-app/$PROJECT_NAME}"
      if [ "$file" != "$new_file" ]; then
        mv "$file" "$new_file"
      fi
      if [[ "$new_file" == *.csproj || "$new_file" == *.cs || "$new_file" == *.json ]]; then
        # Replace hyphens with underscores in .csproj, .cs and .json files
        perl -pi -e "s/dotnet-template-app/${PROJECT_NAME//-/_}/g" "$new_file"
      else
        perl -pi -e "s/dotnet-template-app/$PROJECT_NAME/g" "$new_file"
      fi
    fi
  done
}

rename() {
  mv "$1" "$2"
}

remove_file() {
  rm "$1"
}

replace_text "."
remove_file ".github/workflows/configure-repository.yaml"
remove_file "RUNME.sh"
remove_file "README.md"
rename "after.md" "README.md"

echo "Text replacement complete."