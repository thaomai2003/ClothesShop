using AutoMapper;
using MyFinalExam.Data;
using MyFinalExam.Areas.Admin.Models;
using MyFinalExam.Areas.Admin.Models.ViewModels;

namespace MyFinalExam.Areas.Admin.HelperAdmin

{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<RegisterViewModel, Customer >();
            //.ForMember(customer => customer.Name, option 
            //=> option.MapFrom( RegisterVM => RegisterVM.Name))
            //.ReverseMap();
        }
    }
}


