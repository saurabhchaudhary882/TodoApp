/*
SQL script to initialize the database and create the todos table
*/
CREATE DATABASE IF NOT EXISTS TodoDb;
USE TodoDb;

CREATE TABLE IF NOT EXISTS todos (
    Id CHAR(36) PRIMARY KEY,
    Title VARCHAR(255) NOT NULL,
    Description TEXT,
    IsCompleted BOOLEAN NOT NULL DEFAULT FALSE,
    CreatedAt DATETIME NOT NULL,
    UpdatedAt DATETIME NULL,
    DueDate DATETIME NULL
);