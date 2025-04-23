output "photomanager_backend_instance_public_id" {
  value         = aws_instance.photomanager_backend.public_ip
  description = "Public IP address of Photomanager Backend EC2 instance"
}

