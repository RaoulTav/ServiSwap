CREATE DATABASE IF NOT EXISTS serviswap;

USE serviswap;

CREATE TABLE Addresses(
    addressId INT AUTO_INCREMENT PRIMARY KEY,
    streetNumber VARCHAR(10) NOT NULL,
    streetName VARCHAR(50) NOT NULL,
    city VARCHAR(50) NOT NULL
);

CREATE TABLE Categories(
	categoryId INT AUTO_INCREMENT PRIMARY KEY,
    categoryName VARCHAR(50) NOT NULL UNIQUE
);

CREATE TABLE People(
	personId INT AUTO_INCREMENT PRIMARY KEY,
    username VARCHAR(20) NOT NULL UNIQUE,
    password VARCHAR(100) NOT NULL,
    lastName VARCHAR(50) NOT NULL,
    firstName VARCHAR(50) NOT NULL,
    email VARCHAR(50) NOT NULL UNIQUE,
    creditHours INT NOT NULL,
    latitude DOUBLE NOT NULL,
    longitude DOUBLE NOT NULL,
    addressId INT NOT NULL,
    CHECK (CHAR_LENGTH(username) BETWEEN 7 AND 20),
    CHECK (REGEXP_LIKE(username, '^[A-Za-z0-9]+$')),
    FOREIGN KEY (addressId) REFERENCES Addresses(addressId)
);

CREATE TABLE Services(
	serviceId INT AUTO_INCREMENT PRIMARY KEY,
    serviceName VARCHAR(50) NOT NULL,
    serviceDescription VARCHAR(1000) NOT NULL,
    dateOfPublication DATE NOT NULL,
    categoryId INT NOT NULL,
    providerId INT NOT NULL,
    FOREIGN KEY (categoryId) REFERENCES Categories(categoryId),
    FOREIGN KEY (providerId) REFERENCES People(personId)
);

CREATE TABLE RequestedServices(
	reqServiceId INT AUTO_INCREMENT PRIMARY KEY,
    dateOfRequest DATE NOT NULL,
    state INT NOT NULL,
    requesterId INT NOT NULL,
    serviceId INT NOT NULL,
    CHECK (state BETWEEN 1 AND 5),
    FOREIGN KEY (requesterId) REFERENCES People(personId),
    FOREIGN KEY (serviceId) REFERENCES Services(serviceId)
);