#!/bin/bash

sudo apt-get update -y
sudo apt-get upgrade -y

sudo wget https://builds.dotnet.microsoft.com/dotnet/Sdk/9.0.203/dotnet-sdk-9.0.203-linux-x64.tar.gz
mkdir -p $HOME/dotnet-9-sdk && tar zxf dotnet-sdk-9.0.203-linux-x64.tar.gz -C $HOME/dotnet-9-sdk
sudo rm -rf dotnet-sdk-9.0.203-linux-x64.tar.gz

$HOME/dotnet-9-sdk/dotnet tool install -g dotnet-ef

export DOTNET_ROOT=$HOME/dotnet-9-sdk
export PATH=$PATH:$HOME/dotnet-9-sdk
export PATH=$PATH:$HOME/.dotnet/tools

sudo apt-get install -y nginx
# sudo apt-get install -y postgresql postgresql-contrib
sudo apt-get install -y certbot
sudo apt-get install -y python3-certbot-nginx

sudo systemctl stop nginx

# sudo systemctl start postgresql
# sudo systemctl enable postgresql

# sudo -u postgres psql -f /tmp/my_init.sql
# sudo certbot certonly --nginx --non-interactive --agree-tos --redirect --email louisgin32@gmail.com -d api.photomanager.site

sudo git clone https://github.com/Khunse/photomanager-backend.git
sudo chown ubuntu:ubuntu -R $HOME/photomanager-backend
cd $HOME/photomanager-backend

export JWT_KEY=lajdkf
export AWS_ACCESS_KEY_ID=kadjlfd
export AWS_SECRET_ACCESS_KEY=dajlfkjd

# dotnet ef migrations add 'create table'
# dotnet ef database update
dotnet publish -c Release -o ./publish

sudo mv /tmp/myapp.service /etc/systemd/system/
sudo mv /tmp/myapp.conf $HOME/

sudo systemctl start myapp.service
sudo systemctl enable myapp.service

# sudo systemctl stop nginx
# sudo systemctl start nginx
# sudo systemctl enable nginx
