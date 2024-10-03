CREATE PROCEDURE [dbo].[spUsers_GetAll]
AS
begin

Select * 
	From dbo.[Users] with (nolock)

end