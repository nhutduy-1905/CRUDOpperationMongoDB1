
using AutoMapper;
using CRUDOpperationMongoDB1.Application.DTO;
using CRUDOpperationMongoDB1.Domain.Entities;
public class PostProfile : Profile
{
    public PostProfile()
    {
        CreateMap<Post, PostDto>();
    }
}

