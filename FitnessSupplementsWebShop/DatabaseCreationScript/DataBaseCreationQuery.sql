

DROP TABLE IF EXISTS orderitem;
DROP TABLE IF EXISTS review;
DROP TABLE IF EXISTS orders;
DROP TABLE IF EXISTS product;
DROP TABLE IF EXISTS payment;
DROP TABLE IF EXISTS category;
DROP TABLE IF EXISTS manufacturer;
DROP TABLE IF EXISTS users;

CREATE TABLE users(
UserID INT IDENTITY(1,1) PRIMARY KEY,
FirstName VARCHAR(100) NOT NULL,
LastName VARCHAR(100) NOT NULL,
Email VARCHAR(100) NOT NULL,
Password VARCHAR(100) NOT NULL,
Phone VARCHAR(100) NOT NULL,
Address VARCHAR(100) NOT NULL,
Role VARCHAR(100) NOT NULL
);

CREATE TABLE category (
CategoryID INT IDENTITY(1,1) PRIMARY KEY,
Name VARCHAR(100) NOT NULL,
);

CREATE TABLE manufacturer (
ManufacturerID INT IDENTITY(1,1) PRIMARY KEY,
Name VARCHAR(100) NOT NULL
)
CREATE TABLE product (
ProductID INT IDENTITY(1,1) PRIMARY KEY,
Name VARCHAR(100) NOT NULL,
PictureUrl VARCHAR(255) NOT NULL,
Description varchar(255) not null,
Price numeric(10) not null,
Quantity int,
CategoryID int,
ManufacturerID int,
FOREIGN KEY (ManufacturerID) REFERENCES manufacturer(ManufacturerID),
FOREIGN KEY (CategoryID) REFERENCES category(CategoryID)
)
CREATE TABLE review
(
ReviewID INT IDENTITY(1,1) PRIMARY KEY,
Comment VARCHAR(500) NOT NULL,
Rating INT,
CONSTRAINT cRating CHECK (Rating BETWEEN 1 AND 5),
UserID INT,
ProductID INT,
FOREIGN KEY (UserID) REFERENCES users(UserID),
FOREIGN KEY (ProductID) REFERENCES product(ProductID)
)
CREATE TABLE payment (
PaymentID INT IDENTITY(1,1) PRIMARY KEY,
PaymentMethod varchar(20)
)
CREATE TABLE orders (
OrderID INT IDENTITY(1,1) PRIMARY KEY,
UserID INT,
PaymentID INT,
OrderAddress VARCHAR(100) NOT NULL,
City VARCHAR(100) NOT NULL,
NumberOfProducts INT,
Total NUMERIC(10) DEFAULT 0,
FOREIGN KEY (UserID) REFERENCES users(UserID),
FOREIGN KEY (PaymentID) REFERENCES payment(PaymentID)
)
CREATE TABLE orderitem(
OrderItemID INT IDENTITY(1,1) PRIMARY KEY,
ProductID INT,
OrderID INT,
Quantity INT,
FOREIGN KEY (ProductID) REFERENCES product(ProductID),
FOREIGN KEY (OrderID) REFERENCES orders(OrderID)

)

DROP TRIGGER IF EXISTS NumberOfOrderProductsUpdate
GO
CREATE TRIGGER NumberOfOrderProductsUpdate
ON orderitem
AFTER INSERT,DELETE 
AS BEGIN
	DECLARE @order_id int
	DECLARE @quantity int
	if((SELECT OrderID FROM inserted) IS NOT NULL)
	begin 
	SET @order_id = (SELECT OrderID FROM inserted)
	SET @quantity= (SELECT sum(Quantity)
	FROM orderitem
	WHERE @order_id= OrderID)
	end
	IF((SELECT OrderID FROM deleted) IS NOT NULL)
	BEGIN
	SET @order_id = (SELECT OrderID FROM deleted)
	SET @quantity= (SELECT sum(Quantity)
	FROM orderitem
	WHERE @order_id= OrderID)
	END

	update orders 
	SET NumberOfProducts=@quantity
	where @order_id=OrderID
END;
GO

DROP TRIGGER IF EXISTS TotalPriceUpdate
GO
CREATE TRIGGER TotalPriceUpdate
ON orderitem
AFTER INSERT 
AS BEGIN
	DECLARE @orderQuantity as int
	DECLARE @sum AS NUMERIC(10) = 0
	DECLARE @order_id int
	DECLARE @productID int
	SET  @productID = (SELECT ProductID FROM inserted)
	SET @orderQuantity= (SELECT Quantity FROM inserted)
	SET @order_id = (SELECT OrderID FROM inserted)
	SET @sum = ( SELECT Price from product where ProductID=@productID)
	UPDATE orders 
	SET Total+=(@sum*@orderQuantity)
	WHERE @order_id=OrderID
END;
GO

DROP TRIGGER IF EXISTS TotalPriceUpdateAfterDelete
GO
CREATE TRIGGER TotalPriceUpdateAfterDelete
ON orderitem
AFTER DELETE 
AS BEGIN
	DECLARE @orderQuantity as int
	DECLARE @sum AS NUMERIC(10) = 0
	DECLARE @order_id int
	DECLARE @productID int
	SET  @productID = (SELECT ProductID FROM deleted)
	SET @orderQuantity= (SELECT Quantity FROM deleted)
	SET @order_id = (SELECT OrderID FROM deleted)
	SET @sum = ( SELECT Price from product where ProductID=@productID)
	UPDATE orders 
	SET Total-=(@sum*@orderQuantity)
	WHERE @order_id=OrderID
END;
GO

DROP TRIGGER IF EXISTS ProductQuantityUpdate
GO
CREATE TRIGGER ProductQuantityUpdate
ON orderitem
AFTER INSERT,DELETE
AS BEGIN
	
	DECLARE @order_id int
	DECLARE @orderQuantity as int
	SET @orderQuantity= (SELECT Quantity FROM inserted)
	SET @order_id = (SELECT OrderID FROM inserted)
	DECLARE @product_id int
	SET @product_id = (SELECT ProductID from inserted)
	DECLARE @quantity int
	SET @quantity = (select quantity from product where @product_id = ProductID)
	if(@quantity > 0)
	BEGIN
		UPDATE product
		SET quantity -= @orderQuantity 
		WHERE @product_id = ProductID
	END
END;
GO

insert into users
values ('Andrija','Stanojkovic','admin','admin','064325129','Novi Sad  Strazilovska 20','admin');

insert into users
values ('Petar','Petrovic','Petar22@gmail.com','petar111111','066325122','Novi Sad  Strazilovska 60','customer');

insert into manufacturer
values ('YAMAMOTO nutrition');
insert into manufacturer
values ('THE nutrition');

insert into category
values ('Oprema');
insert into category
values ('Kreatini');
insert into category
values ('Vitamini');
insert into category
values ('Proteini');


insert into product
values ('THE AMINO WHEY HYDRO PROTEIN 3.500 G','assets/images/the-amino-whey-hydro3500g.jpg','Amino Whey Hydro protein The Nutrition. je preko 86% Protein i sadrži gotovo ništa Masti ili Ugljenih Hidrata.','7500',20, 4 , 2 );
insert into product
values ('KRE - ALKALYN YAMAMOTO 240 KAPSULA','assets/images/yamamoto-kre-alkalyn-240kaps-2.jpg','Yamamoto® Nutrition Kre-ALKALYN® je dodatak ishrani napravljen od mikronizovanog kreatina monohidrata','5000',30, 2 , 1 );
insert into product
values ('ISO-FUJI® YAMAMOTO NUTRITION PROTEIN 2000 GRAMA',
'assets/images/iso-fuji-yamamoto-nutrition2000-grama.jpg',
'Iso-FUJI® - Dodatak ishrani  koji je nastao izolacijom, odnosno unakrsnom ultrafiltracijom i mikrofiltracijom Volactive® Ultra Whey XP.',
'9390',
20, 
4 ,
 1 );
 insert into product
values ('YAMAMOTO® NUTRITION SHAKER 700 ML',
'assets/images/yamamoto-nutrition-shaker-700-ml.jpg',
'Šejker (700ml) sa logotipom "Yamamoto Nutrition".',
'350',
100, 
1 ,
 1 );

insert into product
values ('VITAMIN C COMPLEX +D3+CINK THE NUTRITION 120 KAPSULA',
'assets/images/vitamin-c-complex-d3cink-100-tableta.jpg',
'Vitamin C complex sadrži Cink, vitamin D3 i prirodni ekstrakt šipka. Ova jedinstvena formula postepeno oslobađa i dodaje hranljive materije vašem organizmu. ".',
'1320',
200, 
3 ,
 2 );


insert into product
values ('THE BASIC 100% WHEY PROTEIN 5400 GRAMA',
'assets/images/the-basic-100-whey-protein-5400-grama.jpg',
'100% Whey Protein je napredna formula proteina surutke, napravljena za sve sportaše, koji žele više mišića, više snage i brži oporavak. ',
'8900',
30, 
4 ,
 2 );

insert into payment
values ('Cash'),('Credit card')

insert into orders(UserID,PaymentID,OrderAddress,City)
values (1,2,'Zeleznicka 21','Novi Sad');
insert into orders(UserID,PaymentID,OrderAddress,City)
values (2,2,'Zeleznicka 22','Novi Sad');

insert into review
values ('10/10, svaka preporuka! Odlican protein! Vrlo lako se muti, ukus izvanredan. Takodje i after-taste. Cist bez ikakvih stetnih dodataka sto je zaista potrebno trzistu, izuzetno brza dostava. Necete se pokajati!',5,1,1)

SELECT * from orders
SELECT * from orderitem
SELECT * from product
select * from category
select * from manufacturer
select * from payment
select * from users
select * from review

delete orderitem where OrderItemID=2
insert into orderitem
values (1,1,2)

