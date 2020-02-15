dotnet publish -c Release
cd bin/Release/netcoreapp2.1/publish
zip -r KinesisHandler.zip *
aws s3 cp --acl public-read KinesisHandler.zip s3://us-east-1.andyhoppatamazon.com/KinesisDemo/
