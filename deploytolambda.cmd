
set funciton_name=photomanagertesting
set publish_path=publish
set zip_path=publish.zip

dotnet publish -c Release -o ./%publish_path% -r linux-x64 --self-contained false
7z a -tzip  ./%zip_path% ./%publish_path%/*
aws lambda update-function-code --function-name %funciton_name% --zip-file fileb://%zip_path%
rmdir /s /q %publish_path%
del /q %zip_path%