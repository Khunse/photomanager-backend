
terraform {

  required_providers {
    aws = {
      source  = "hashicorp/aws"
      version = "~> 4.16"
    }
  }

  required_version = ">= 1.2.0"

}

provider "aws" {

}

data "aws_route53_zone" "photomanager_domain" {
  name         = "photomanager.site"
  private_zone = false
}

resource "aws_eip" "photomanger_backend_instance_eip" {
  
  instance = aws_instance.photomanager_backend.id
}

resource "aws_route53_record" "photomanager_api_domain" {
  zone_id = "${data.aws_route53_zone.photomanager_domain.zone_id}"
  name    = "api.photomanager.site"
  type    = "A"
  ttl     = "300"
  records = [aws_eip.photomanger_backend_instance_eip.public_ip]
}


resource "aws_instance" "photomanager_backend" {
  ami                    = "ami-04d5959246ae9f7ed"
  instance_type          = "t2.micro"
  vpc_security_group_ids = [aws_security_group.photomanager_backend_SG.id]
  tags = {
    Name = var.photomanager_backend_instance_name
  }

  user_data = file("D:\\New folder (6)\\imageuploadandmanagementsystem\\photomanager_backend_config.sh")
}

resource "aws_security_group" "photomanager_backend_SG" {
  name        = "Photomanager Backend SG"
  description = "allow on port 80 and 22 for Photomanager Backend EC2 instance"

  ingress {
    from_port        = 80
    to_port          = 80
    protocol         = "tcp"
    cidr_blocks      = ["0.0.0.0/0"]
    ipv6_cidr_blocks = ["::/0"]
  }

  ingress {
    from_port        = 443
    to_port          = 443
    protocol         = "tcp"
    cidr_blocks      = ["0.0.0.0/0"]
    ipv6_cidr_blocks = ["::/0"]
  }

  ingress {
    from_port        = 22
    to_port          = 22
    protocol         = "tcp"
    cidr_blocks      = ["0.0.0.0/0"]
    ipv6_cidr_blocks = ["::/0"]
  }

  egress {
    from_port        = 0
    to_port          = 0
    protocol         = "-1"
    cidr_blocks      = ["0.0.0.0/0"]
    ipv6_cidr_blocks = ["::/0"]
  }
}