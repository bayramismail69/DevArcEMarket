using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Handlers.Categories.Queries;
using Core.Utilities.Results;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;

namespace Business.Handlers.Categories.BusinessRules
{
   public class CategoryBusinessRules : ICategoryBusinessRules
   {
       private readonly ICategoryRepository _categoryRepository;

       public CategoryBusinessRules(ICategoryRepository categoryRepository)
       {
           _categoryRepository = categoryRepository;
       }


       public async Task<IResult> NameControl(string name)
       {
           var result= await _categoryRepository.GetAsync(c => c.Name.ToUpper() == name.ToUpper());
            if (  result==null)
            {
                return new SuccessResult();
            }
            return new ErrorResult();
        }
      
    }
}
