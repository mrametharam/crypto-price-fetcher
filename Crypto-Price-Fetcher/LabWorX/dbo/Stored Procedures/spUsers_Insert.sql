CREATE PROCEDURE [dbo].[spUsers_Insert]
	@UserName nvarchar(50),
	@FirstName nvarchar(50),
	@LastName nvarchar(50),
	@Email nvarchar(50),
	@Password nvarchar(50),
	@IsActive bit

AS
Begin

Insert into dbo.[Users]
		([UserName], [FirstName], [LastName], [Email], [Password], [IsActive])
	Values
		(@UserName, @FirstName, @LastName, @Email, @Password, @IsActive)

End

Return @@Identity