
namespace Api.Dto.Request.Chore.v1;

public class PutChoreRequestDto
{
       
    public Guid Id { get; set; }
    public string SubCategoryId { get; set; }
    public string Description { get; set; }
    public string Title { get; set; }
}