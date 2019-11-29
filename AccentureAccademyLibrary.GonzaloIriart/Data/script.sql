CREATE DATABASE AccentureAccademyLibrary;
GO

USE AccentureAccademyLibrary;
GO

CREATE TABLE Genre
(
    Id int primary key identity(1,1),
	[Name] varchar(50) not null,
    CONSTRAINT UQ_NOMBRE_Genero
	    UNIQUE([Name])
);

CREATE TABLE Author
(
    Id int primary key identity(1,1),
	[Name] varchar(70) not null,
    CONSTRAINT UQ_NAME_Autor
	    UNIQUE([Name])
);

CREATE TABLE Publisher
(
    Id int primary key identity(1,1),
	[Name] varchar(70) not null,
    CONSTRAINT UQ_NAME_Publisher
	    UNIQUE([Name])
);

CREATE TABLE Book
(
    Id int primary key identity(1,1),
	ISBN varchar(25) not null,
	Title varchar(70) not null,
    Cover varchar(300),
    [Description] varchar(500),
	ReleaseDate date not null,
    Id_Publisher int not null,
    Id_Genre int not null,
    CONSTRAINT FK_Book_Publisher
		FOREIGN KEY (Id_Publisher) 
		REFERENCES Publisher(Id),
    CONSTRAINT FK_Book_Genre 
		FOREIGN KEY (Id_Genre) 
		REFERENCES Genre(Id),
    CONSTRAINT UQ_Title
	    UNIQUE(Title)
);

CREATE TABLE WrittenBy
(
	Book_Id int not null,
	Author_Id int not null,
	CONSTRAINT FK_WrittenBy_Author 
		FOREIGN KEY (Author_Id) 
		REFERENCES Author(Id)
		ON DELETE CASCADE,
    CONSTRAINT FK_WrittenBy_Book 
		FOREIGN KEY (Book_Id) 
		REFERENCES Book(Id)
		ON DELETE CASCADE,
		CONSTRAINT PK_WRITTEN_BY PRIMARY KEY(Author_Id, Book_Id)

);

INSERT INTO Genre
([Name])
VALUES
('Fiction'),
('Poetry'),
('Policial'),
('History'),
('Fantasy'),
('Thriller'),
('Terror'),
('Cience Fiction'),
('Chilrens book'),
('Romance'),
('Other')

INSERT INTO Publisher
([Name])
VALUES
('Editorial uno'),
('Editorial dos'),
('Editorial tres'),
('Editorial cuatro'),
('Editorial cinco'),
('Editorial seis')

INSERT INTO Author
([Name])
VALUES
('George Martin'),
('JK Rowling'),
('Brandon Sanderson'),
('JRR Tolkien'),
('Borges'),
('Kafka')

INSERT INTO Book
([Title],[ISBN],[Description],[Cover],[Id_Genre],[Id_Publisher],[ReleaseDate])
VALUES
('Lorem','12343','Description','link',1,1,'2010-1-1'),
('IPSUM','235567','Es un libro','link',2,2,'2012-2-23'),
('ABC','356345','Hola soy un libro','link',3,3,'2001-2-3'),
('1234','4592474','Otro libro','link',4,4,'1900-3-15'),
('Hola','98766474','Mas libros','link',5,5,'1947-8-31'),
('Aloha','12355523','Descripcion de otro libro','link',6,6,'1500-12-25')

INSERT INTO WrittenBy
([Book_Id],[Author_Id])
VALUES
(1,1),
(1,2),
(1,3),
(2,4),
(3,5),
(4,6),
(5,1),
(6,2),
(3,3),
(5,4),
(4,5)


