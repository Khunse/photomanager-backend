#!/bin/bash

sudo apt-get update -y
sudo apt-get upgrade -y

sudo wget https://builds.dotnet.microsoft.com/dotnet/Sdk/9.0.203/dotnet-sdk-9.0.203-linux-x64.tar.gz
mkdir -p $HOME/dotnet-9-sdk && tar zxf dotnet-sdk-9.0.203-linux-x64.tar.gz -C $HOME/dotnet-9-sdk
sudo rm -rf dotnet-sdk-9.0.203-linux-x64.tar.gz

$HOME/dotnet-9-sdk/dotnet tool install -g dotnet-ef

export DOTNET_ROOT=$HOME/dotnet-9-sdk
export PATH=$PATH:$HOME/dotnet-9-sdk

# sudo apt-get install -y nginx
sudo apt-get install -y postgresql postgresql-contrib
# sudo apt-get install -y certbot
# sudo apt-get install -y python3-nginx-certbot 

sudo systemctl start postgresql
sudo systemctl enable postgresql

sudo -u postgres psql -f /tmp/my_init.sql
# sudo certbot --nginx -d api.photomanager.site

sudo git clone https://github.com/Khunse/photomanager-backend.git
# cd $HOME/photomanager-backend
# dotnet publish -c Release -o ./publish

# sudo mv /tmp/myfile/myapp.service /etc/systemd/system/
# sudo mv /tmp/myfile/myapp.conf /etc/nignx/conf.d/

# sudo systemctl start myapp.service
# sudo systemctl enable myapp.service

# sudo systemctl start nginx
# sudo systemctl enable nginx


# sudo rm -rf /tmp/myfile