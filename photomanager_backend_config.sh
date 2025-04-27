#!/bin/bash

sudo certbot certonly --nginx --non-interactive --email louisgin32@gmail.com --agree-tos --redirect -d api.photomanager.site >> /var/log/user-data.log
sudo mv /home/ubuntu/myapp.conf /etc/nginx/conf.d/
sudo systemctl daemon-reload
sudo systemctl stop nginx
sudo systemctl start nginx

