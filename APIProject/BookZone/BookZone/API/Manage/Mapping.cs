using AutoMapper;
using BookZone.API.Client.DTOs.AccountDtos;
using BookZone.API.Client.DTOs.BookDtos;
using BookZone.API.Client.DTOs.CategoryDtos;
using BookZone.API.Client.DTOs.GenreDtos;
using BookZone.API.Client.DTOs.SettingDtos;
using BookZone.API.Client.DTOs.SliderDtos;
using BookZone.API.Client.DTOs.TagDtos;
using BookZone.API.Manage.DTOs.AccountDtos;
using BookZone.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookZone.API.Manage
{
    public class Mapping:Profile
    {
        public Mapping()
        {
            CreateMap<AppUser, AccountGetDto>();
            CreateMap<AppUser, ClientAccountGetDto>();
            CreateMap<Tag, TagGetDto>();
            CreateMap<Slider, SliderGetDto>();
            CreateMap<Setting, SettingGetDto>();
            CreateMap<Category, CategoryGetDto>();
            CreateMap<Genre, GenreGetDto>();
            CreateMap<Book, BookGetDto>();
        }
    }
}
