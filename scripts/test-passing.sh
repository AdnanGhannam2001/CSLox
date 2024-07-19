#!/bin/bash

files=`find test/passing -name "*.lox"`
filesarray=($files)

for file in "${filesarray[@]}"
do
    echo "Testing file: $file"
    ./scripts/run.sh $file
    echo ""
done