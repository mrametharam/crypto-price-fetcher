CREATE PROCEDURE [dbo].[spUsers_Update]
	@Id uniqueidentifier,
	@UserName nvarchar(50),
	@FirstName nvarchar(50),
	@LastName nvarchar(50),
	@Email nvarchar(50),
	@Password nvarchar(50),
	@IsActive bit
AS

Begin

Update dbo.[Users] Set 
	[UserName] = @UserName,
	[FirstName] = @FirstName, 
	[LastName] = @LastName, 
	[Email] = @Email, 
	[Password] = @Password, 
	[IsActive] = @IsActive
Where [Id] = @Id

End

Return 0;