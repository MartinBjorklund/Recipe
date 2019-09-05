use master
go

create database recipe
go

use recipe
go

create table Recipe(
id int primary key identity,
RecipeName nvarchar(255) not null,

)

create table BaseIngredients(
id int primary key identity,
Name nvarchar(255) not null,
)

create table Ingredients(
primary key(baseingredientid, recipeid),
BaseIngredientid int references baseingredients(id),
Recipeid int references recipe(id),
Amount nvarchar(255) not null,

)
create table Person(
id int primary key identity,
firstName nvarchar(255) not null,
lastName nvarchar(255) not null,
email nvarchar(255) not null,
)

create table Rating(
primary key(personId, recipeid),
personId int references person(id),
RecipeId int references recipe(id),
Rating int not null,
)

create table tag(
Id int primary key identity,
tagName nvarchar(100)
)

create table RecipeTag(
primary key(tagId, RecipeId),
tagId int references tag(Id),
RecipeId int references Recipe(id)
)

Insert Person(firstName, lastName, email)
values
('martin', 'björklund','hej@fjan.com'),
('sven', 'Olofsson','hejdå@fjan.com'),
('Lars', 'Larsson','hejsan@fjan.com')

insert Recipe(RecipeName)
values
('flygande jacob'),
('Tacos'),
('fruktbröd')
go

insert BaseIngredients(Name)
values
('mjöl'),
('Kyckling'),
('Jordnötter'),
('Grädde'),
('Gurka'),
('Tomat'),
('Majs'),
('Aprikoser')
go

insert Ingredients(BaseIngredientid, Recipeid, Amount)
values
(2,1, '400 gram'),
(3,1, '100 gram'),
(1,3, '4dl'),
(8,3, '100 gram'),
(4,2, '50 gram'),
(5, 2, '50 gram'),
(6,2, '100 gram')


insert Rating(personId, RecipeId, Rating)
values
(1, 1, 1),
(1, 2, 2),
(1, 3, 3),
(2, 1, 4),
(3, 1, 5)

insert tag(tagName)
values
('kyckling'),
('mjöl'),
('Jordnötter'),
('Grädde'),
('Gurka'),
('Tomat'),
('Majs'),
('Aprikoser')

insert RecipeTag(RecipeId, tagId)
values
(1, 1),
(1, 3),
(1, 4),
(2, 5),
(2, 6),
(2, 7),
(3, 2),
(3, 8)



select * from BaseIngredients
inner join Ingredients on BaseIngredients.id = Ingredients.BaseIngredientid
go

create procedure usp_getRecipe 
(
	@TagName nvarchar(100)
)
as
begin

select Recipe.id, RecipeName
from Recipe
inner join Ingredients on Ingredients.Recipeid = Recipe.id
inner join BaseIngredients on BaseIngredients.id = Ingredients.BaseIngredientid
inner join RecipeTag on RecipeTag.RecipeId = Recipe.id
inner join tag on tag.Id = RecipeTag.tagId
where tagName = @TagName
group by Recipe.id, RecipeName

end
go

CREATE PROCEDURE usp_DeleteRecipe
(
	@Id int
)
AS
BEGIN

begin transaction 

begin try
	
	DELETE FROM Ingredients WHERE RecipeId = @Id
	DELETE FROM RecipeTag WHERE RecipeId = @Id
	DELETE FROM Rating WHERE RecipeId = @Id
	DELETE FROM Recipe WHERE id = @Id

end try

begin catch
	if @@TRANCOUNT > 0
	    rollback transaction
end catch

if @@TRANCOUNT > 0
	commit transaction
END

exec usp_DeleteRecipe 1
exec usp_getRecipe 'kyckling'


select *
from Recipe

select *
from Ingredients 
where Recipeid = 1

DELETE FROM Ingredients WHERE RecipeId = 1
	DELETE FROM RecipeTag WHERE RecipeId = 1
	DELETE FROM Recipe WHERE id = 1

	select BaseIngredients.id, BaseIngredients.Name, Amount 
	From Ingredients
	inner join baseingredients on BaseIngredients.id = Ingredients.BaseIngredientid
	where Recipeid = 3

	select * from Recipe
