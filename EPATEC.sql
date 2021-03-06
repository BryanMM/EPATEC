
CREATE DATABASE EPATEC;
go
USE EPATEC;

CREATE TABLE CATEGORY(
	CA_ID varchar(255) NOT NULL,
	CDescription varchar(255) NULL,
	PRIMARY KEY(CA_ID)
)

CREATE TABLE CLIENT(
	C_ID int NOT NULL,
	FName varchar(255) NOT NULL,
	LName varchar(255) NOT NULL,
	CAddress varchar(255) NOT NULL,
	Phone int NOT NULL,
	Day int NOT NULL,
	Month int NOT NULL,
	Year int NOT NULL,
	Penalization int DEFAULT 0 NOT NULL,
	CPassword varchar(255) NOT NULL,
	PRIMARY KEY(C_ID)
	)

CREATE TABLE EMPLOYEE(
	E_ID int NOT NULL,
	CName varchar(255) NULL,
	LName varchar(255) NULL,
	CAddress varchar(255) NULL,
	Charge varchar(255) NULL,
	S_ID int NULL,
	EPassword varchar(255) DEFAULT '1234' NULL,
	PRIMARY KEY(E_ID)
)

CREATE TABLE EORDER(
	O_ID int NOT NULL,
	OPriority int DEFAULT 0 NULL,
	OStatus char(255) DEFAULT 'not delivered' NOT NULL,
	OrderDate date  DEFAULT getdate() NULL,
	S_ID int NULL,
	OPlatform varchar(255) NOT NULL,
	C_ID int NULL,
	PRIMARY KEY(O_ID)
)


CREATE TABLE EPROVIDER(
	P_ID int NOT NULL,
	PName char(255) NOT NULL,
	LName char(255) NOT NULL,
	PAddress char(255) NOT NULL,
	Phone int NOT NULL,
	Day int NOT NULL,
	Month int NOT NULL,
	Year int NOT NULL,
	PRIMARY KEY(P_ID)
)

CREATE TABLE HAS(
	O_ID int NOT NULL,
	PRName varchar(255) NOT NULL,
	PRAmount int NOT NULL,
	
) 


CREATE TABLE NEED(
	ID int IDENTITY(1,1) NOT NULL,
	S_ID int NULL,
	PDR_ID int NULL,

) 


CREATE TABLE PC(
	PR_ID int NOT NULL,
	CA_ID varchar(255) NOT NULL
) 


CREATE TABLE PRODUCT(
	PR_ID int NOT NULL,
	PName varchar(255) NOT NULL,
	Extent bit NOT NULL,
	PDescription varchar(255) NULL,
	Quantity int NOT NULL,
	Price int NOT NULL,
	S_ID int NULL,
	P_ID int NULL,
	PRIMARY KEY(PR_ID)
)


CREATE TABLE SUCURSAL(
	S_ID int NOT NULL,
	SName char(255) NOT NULL,
	SAddress char(255) NOT NULL,
	PRIMARY KEY(S_ID)

)

ALTER TABLE EMPLOYEE  ADD FOREIGN KEY(S_ID)
REFERENCES SUCURSAL (S_ID)

ALTER TABLE EORDER ADD FOREIGN KEY(C_ID)
REFERENCES CLIENT (C_ID)

ALTER TABLE EORDER  WITH CHECK ADD FOREIGN KEY(S_ID)
REFERENCES SUCURSAL (S_ID)

ALTER TABLE HAS ADD FOREIGN KEY(O_ID)
REFERENCES EORDER (O_ID)

ALTER TABLE NEED ADD FOREIGN KEY(PDR_ID)
REFERENCES EPROVIDER (P_ID)

ALTER TABLE NEED ADD FOREIGN KEY(S_ID)
REFERENCES SUCURSAL (S_ID)

ALTER TABLE PC ADD FOREIGN KEY(CA_ID)
REFERENCES CATEGORY (CA_ID)

ALTER TABLE PC ADD FOREIGN KEY(PR_ID)
REFERENCES PRODUCT (PR_ID)

ALTER TABLE PRODUCT ADD FOREIGN KEY(P_ID)
REFERENCES EPROVIDER (P_ID)

ALTER TABLE PRODUCT ADD FOREIGN KEY(S_ID)
REFERENCES SUCURSAL (S_ID)

