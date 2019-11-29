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
('Planeta'),
('Humano'),
('Universal'),
('Libroides buenardos'),
('De bolsillo'),
('Booket')

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
('Game of Thrones','12343','uego de tronos es una novela de fantasía escrita por el autor estadounidense George R. R. Martin en 1996 y ganadora del Premio Locus a la mejor novela de fantasía en 1997. Se trata de la primera entrega de la serie de gran popularidad Canción de hielo y fuego.','http://prodimage.images-bn.com/pimages/9780553573404_p0_v1_s1200x630.jpg',1,1,'2010-1-1'),
('Harry Potter','235567','El día de su cumpleaños, Harry Potter descubre que es hijo de dos conocidos hechiceros, de los que ha heredado poderes mágicos. Debe asistir a una famosa escuela de magia y hechicería, donde entabla una amistad con dos jóvenes que se convertirán en sus compañeros de aventura. Durante su primer año en Hogwarts, descubre que un malévolo y poderoso mago llamado Voldemort está en busca de una piedra filosofal que alarga la vida de quien la posee.','https://www.easons.com/globalassets/harry-philosophers-9781408855652.jpeg',2,2,'2012-2-23'),
('Mistborn','356345','Mistborn: The Final Empire is set in an analog of the early 18th century in the dystopian world of Scadrial, where ash constantly falls from the sky, all plants are brown, and supernatural mists cloak the landscape every night. One thousand years before the start of the novel, the prophesied Hero of Ages ascended to godhood at the Well of Ascension in order to repel the Deepness, a terror threatening the world whose true...','https://images-na.ssl-images-amazon.com/images/I/51P1gigjHSL._SX302_BO1,204,203,200_.jpg',3,3,'2001-2-3'),
('Lord of the rings','4592474','Otro libro','https://m.media-amazon.com/images/M/MV5BN2EyZjM3NzUtNWUzMi00MTgxLWI0NTctMzY4M2VlOTdjZWRiXkEyXkFqcGdeQXVyNDUzOTQ5MjY@._V1_.jpg',4,4,'1900-3-15'),
('El hacedor','98766474','«Yo vivo, yo me dejo vivir, para que Borges pueda tramar su literatura y esa literatura me justifica.»En El hacedor confluyen las simbologías de Oriente y Occidente, el cosmos y las cosmogonías, los siglos, las dinastías, lo universal y lo profundamente local: Heráclito, Homero, Dante con Rosas, Facundo y Juan Muraña.','http://quelibroleo.hola.com/images/libros/libro_1362276822.jpg',5,5,'1947-8-31'),
('metamorphosis','12355523','The Metamorphosis (German: Die Verwandlung) is a novella by Franz Kafka, first published in 1915. The story begins with a traveling salesman, Gregor Samsa, waking to find himself transformed into a "monstrous vermin".','https://images-eu.ssl-images-amazon.com/images/I/51mGSj0VDLL.jpg',6,6,'1500-12-25')

INSERT INTO WrittenBy
([Book_Id],[Author_Id])
VALUES
(1,1),
(2,2),
(3,3),
(4,4),
(5,5),
(6,6)

select *
from Book
join WrittenBy
on WrittenBy.Book_Id = Book.Id
join Author
on WrittenBy.Author_Id = Author.Id

