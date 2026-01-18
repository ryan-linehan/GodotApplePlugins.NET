#!/bin/bash
# Generates C# wrapper code from GodotApplePlugins XML documentation

set -e

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
DOC_CLASSES="$SCRIPT_DIR/doc_classes"
OUTPUT="$SCRIPT_DIR/src/GodotApplePlugins.NET/Generated"

echo "=== GodotApplePlugins.NET Code Generator ==="
echo ""

# Check if doc_classes exists
if [ ! -d "$DOC_CLASSES" ]; then
    echo "Error: doc_classes directory not found at $DOC_CLASSES"
    echo "Please download the XML files from https://github.com/migueldeicaza/GodotApplePlugins/tree/main/doc_classes"
    exit 1
fi

# Build and run the generator
echo "Building generator..."
dotnet build "$SCRIPT_DIR/src/GodotApplePlugins.Generator" -c Release -v q

echo ""
echo "Running generator..."
dotnet run --project "$SCRIPT_DIR/src/GodotApplePlugins.Generator" -c Release -- "$DOC_CLASSES" "$OUTPUT"

echo ""
echo "=== Generation complete ==="
