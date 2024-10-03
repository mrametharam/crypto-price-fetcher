CREATE PROCEDURE [dbo].[spUsers_Delete]
	@Id uniqueidentifier
AS
begin

Delete
	From dbo.[Users] 
	Where Id = @Id

end

Return 0