packer {
  required_plugins {
    amazon = {
      source  = "github.com/hashicorp/amazon"
      version = "~> 1"
    }
  }
}

source "amazon-ebs" "photomanagerapi" {
  ami_name      = "my_phtomanagerapi_aws_ami_3"
  instance_type = "t2.micro"
  region        = "ap-southeast-1"

  source_ami_filter {
    filters = {
      image-id            = "ami-01938df366ac2d954"
      root-device-type    = "ebs"
      virtualization-type = "hvm"
    }
    most_recent = true
    owners      = ["099720109477"]
  }

  ssh_username = "ubuntu"
}

build {
  sources = ["source.amazon-ebs.photomanagerapi"]

  provisioner "file" {
    source      = "C:\\Users\\Louis\\.ssh\\id_rsa.pub"
    destination = "~/.ssh/authorized_keys"
  }

  provisioner "file" {
    source      = "D:\\New folder (6)\\imageuploadandmanagementsystem\\myapp.service"
    destination = "/tmp/myapp.service"
  }

  provisioner "file" {
    source      = "D:\\New folder (6)\\imageuploadandmanagementsystem\\init.sql"
    destination = "/tmp/my_init.sql"
  }

  provisioner "file" {
    source      = "D:\\New folder (6)\\imageuploadandmanagementsystem\\myapp.conf"
    destination = "/tmp/myapp.conf"
  }

  provisioner "shell" {
    script = "./setup.sh"
  }

}  