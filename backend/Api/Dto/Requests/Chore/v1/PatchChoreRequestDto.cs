
namespace Api.Dto.Request.Chore.v1;

public class PatchChoreRequestDto
{
       
    public Guid Id { get; set; }
    public string CategoryId { get; set; }
    public string Description { get; set; }
    public string Title { get; set; }
}