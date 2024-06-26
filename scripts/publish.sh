#!/bin/bash

dotnet publish -c release -o ./cslox \
    -p:PublishReadyToRun=true \
    -p:SelfContained=true \
    -p:PublishSingleFile=true \
    -p:PublishTrimmed=true -p:TrimMode=link