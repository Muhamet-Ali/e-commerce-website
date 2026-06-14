using E_StoreMVCTemplate.Application.DTOs.HeaderImage;
using E_StoreMVCTemplate.Application.DTOs.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_StoreMVCTemplate.Application.Interfaces
{
    public interface IHeadeImagesService :IGenericService
         <GetHeadeImagesListDto , GetHeadeImagesByIdDto, CreateHeadeImagesDto, UpdateHeadeImagesDto>
    {
    }
}
