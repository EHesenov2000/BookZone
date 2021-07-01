using BookZone.API.Client.DTOs.BookDtos;
using BookZone.API.Client.DTOs.CommentDtos;
using BookZone.API.Client.DTOs.TagDtos;
using BookZone.Data.Entities;
using BookZone.Repository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookZone.API.Client.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly IAppUserRepository _appUserRepository;
        private readonly UserManager<AppUser> _userManager;

        public BooksController(IBookRepository bookRepository,ICommentRepository commentRepository,IAppUserRepository appUserRepository,UserManager<AppUser> userManager)
        {
            _bookRepository = bookRepository;
            _commentRepository = commentRepository;
            _appUserRepository = appUserRepository;
            _userManager = userManager;
        }
        [HttpGet("")]
        public async Task<IActionResult> GetAll(int? categoryId,int? genreId,int? tagId)
        {
            List<Book> books = await _bookRepository.GetAllAsync(categoryId, genreId);
            if (books.Count == 0) return NotFound();
            List<BookGetDto> getDtos = new List<BookGetDto>();
            foreach (var item in books)
            {
                BookGetDto book = new BookGetDto
                {
                    Id = item.Id,
                    Name = item.Name,
                    Desc = item.Desc,
                    AuthorName = item.Author.FullName,
                    GenreName = item.Genre.Name,
                    Image = item.Image,
                    SalePrice = item.SalePrice,
                    Status = item.Status,
                };
                book.Tags = new List<TagGetDto>();

                if (tagId != null)
                {
                    foreach (var item1 in item.BookTags)
                    {
                        if (item1.TagId == tagId)
                        {
                            TagGetDto tag = new TagGetDto
                            {
                                Id = item1.Id,
                                Name = item1.Tag.Name
                            };
                            book.Tags.Add(tag);
                            getDtos.Add(book);
                        }

                    }

                }
                else
                {
                    foreach (var item1 in item.BookTags)
                    {
                      
                            TagGetDto tag = new TagGetDto
                            {
                                Id = item1.Id,
                                Name = item1.Tag.Name
                            };
                            book.Tags.Add(tag);
                            getDtos.Add(book);
                    }
                }
         

            }
            if (getDtos.Count == 0) return NotFound();
            return StatusCode(200, getDtos);
        }

        [Authorize(Roles = "Member")]
        [HttpPost]
        public async Task<IActionResult> AddComment(CommentCreateDto createDto)
        {
            if (createDto == null ||createDto.BookId==0 || createDto.Text == null) return BadRequest();
            Book book = await _bookRepository.GetAsync(createDto.BookId);
            if (book == null) return BadRequest();
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user == null) return Unauthorized();

            Comment comment = new Comment
            {
                AppUserId = user.Id,
                BookId = book.Id,
                Text = createDto.Text,
                CreatedAt=DateTime.UtcNow.AddHours(4)
            };
            await _commentRepository.CreateAsync(comment);
            await _commentRepository.CommitAsync();
            return Ok();
        }

        [Authorize(Roles = "Member")]
        [HttpGet("getcomment")]
        public async Task<IActionResult> LoadComment(int id, int page = 1)
        {
            if (id == 0) return BadRequest();
            Book book = await _bookRepository.GetAsync(id);
            if (book == null) return NotFound();
            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user == null) return NotFound();
            List<Comment> comments = await _commentRepository.GetAllAsyncById(id, page);
            List<CommentGetDto> getDtos = new List<CommentGetDto>();
            foreach (var item in comments)
            {
                CommentGetDto getDto = new CommentGetDto
                {
                    AppUserName = item.AppUser.FullName,
                    Text = item.Text,
                    CreatedAt = item.CreatedAt
                };
                getDtos.Add(getDto);
            }
            return Ok(getDtos);
        }
    }
}
