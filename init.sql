create user myuser with password 'Aungaung123!';
create database "imageuploadmanagementsystem.dev";
grant all privileges on database "imageuploadmanagementsystem.dev" to myuser;
grant all privileges on all tables in schema public to myuser;
alter default privileges in schema public grant all privileges on tables to myuser;