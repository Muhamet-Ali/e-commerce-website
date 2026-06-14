using E_StoreMVCTemplate.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;



namespace E_StoreMVCTemplate.Web.ViewComponents
{
    public class HeaderImagesViewComponent:ViewComponent
    {
        private readonly IHeadeImagesService _headeImagesService;

        public HeaderImagesViewComponent(IHeadeImagesService headeImagesService)
        {
            _headeImagesService = headeImagesService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var headerImages=await _headeImagesService.GetAllAsync();
            return View(headerImages);
        }

    }
}
