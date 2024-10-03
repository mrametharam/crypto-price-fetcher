CREATE PROCEDURE [dbo].[spUsers_Get]
	@Id uniqueidentifier
AS
begin

Select * 
	From dbo.[Users] with (nolock)
	Where Id = @Id

end