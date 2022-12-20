using WebAPI.ViewModels;

namespace WebAPI.Services
{
    public interface IAugmentBuildValidationService
    {
        Task Validate(BuildViewModel buildViewModel);
    }
}
