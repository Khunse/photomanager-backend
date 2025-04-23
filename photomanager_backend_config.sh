#!/bin/bash

sudo certbot certonly --nginx --non-interactive --email louisgin32@gmail.com --agree-tos --redirect -d api.photomanager.site
sudo mv $HOME/myapp.conf /etc/nginx/conf.d/
sudo systemctl daemon-reload

