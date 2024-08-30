using AutoMapper;
using MyFinalExam.Data;
using MyFinalExam.ViewModels;

namespace MyFinalExam.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<RegisterVM, Customer >();
            //.ForMember(customer => customer.Name, option 
            //=> option.MapFrom( RegisterVM => RegisterVM.Name))
            //.ReverseMap();
        }
    }
}


