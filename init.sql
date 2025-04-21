create user myuser with password 'Aungaung123!';
create database "imageuploadmanagementsystem.dev";
\c "imageuploadmanagementsystem.dev";
grant usage, create on schema public to myuser;
grant all privileges on database "imageuploadmanagementsystem.dev" to myuser;
alter default privileges in schema public grant select,insert, update,delete on tables to myuser;