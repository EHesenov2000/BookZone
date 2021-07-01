using BookZone.API.Manage.DTOs.BookDtos;
using BookZone.API.Manage.DTOs.TagDtos;
using BookZone.Data.Entities;
using BookZone.Repository.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BookZone.API.Manage.Controllers
{
    [Route("api/manage/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {

        private readonly IWebHostEnvironment _env;
        private readonly IBookRepository _bookRepository;
        private readonly ITagRepository _tagRepository;
        private readonly IBookTagRepository _bookTagRepository;
        private readonly IAuthorRepository _authorRepository;
        private readonly IGenreRepository _genreRepository;

        public BooksController(IWebHostEnvironment env, IBookRepository bookRepository,ITagRepository tagRepository,IBookTagRepository bookTagRepository,IAuthorRepository authorRepository,IGenreRepository genreRepository)
        {
            _env = env;
            _bookRepository = bookRepository;
            _tagRepository = tagRepository;
            _bookTagRepository = bookTagRepository;
            _authorRepository = authorRepository;
            _genreRepository = genreRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] BookCreateDto book)
        {
            if (book == null) return BadRequest();
            if (book.Name == null || book.Desc == null || book.AuthorId == 0 || book.GenreId == 0) return BadRequest();
            Book newBook = new Book { };
            if (book.File != null)
            {
                if (book.File.ContentType != "image/png" && book.File.ContentType != "image/jpeg")
                {
                    return Conflict();
                }
                if (book.File.Length > (1024 * 1024) * 5)
                {
                    return Conflict();
                }
                string rootPath = _env.WebRootPath;
                var fileName = Guid.NewGuid().ToString() + book.File.FileName;
                var path = Path.Combine(rootPath, "uploads/book", fileName);
                using (System.IO.FileStream stream = new FileStream(path, System.IO.FileMode.Create))
                {
                    book.File.CopyTo(stream);
                }
                newBook.Image = fileName;
            }

            else
            {
                return BadRequest();
            }

            newBook.AuthorId = book.AuthorId;
            newBook.GenreId = book.GenreId;
            newBook.Name = book.Name;
            newBook.Desc = book.Desc;
            newBook.SalePrice = book.SalePrice;
            newBook.ProducingPrice = book.ProducingPrice;
            newBook.Status = book.Status;
            newBook.IsDeleted = false;
            newBook.CreatedAt = DateTime.UtcNow.AddHours(4);
            await _bookRepository.CreateAsync(newBook);
            await _bookRepository.CommitAsync();
            if (book.TagIds != null)
            {
                foreach (var item in book.TagIds)
                {
                    Tag tag =await _tagRepository.GetAsync(item);
                    if (tag == null) return NotFound();
                    BookTag bookTag = new BookTag
                    {
                        BookId = newBook.Id,
                        TagId = item,
                    };
                    await _bookTagRepository.CreateAsync(bookTag);
                    await _bookTagRepository.CommitAsync();
                    newBook.BookTags.Add(bookTag);
                }
            }
            await _bookRepository.CommitAsync();
            return StatusCode(201);
        }
        [HttpGet]
        public async Task<IActionResult> GetAll(int page = 1, int count = 10)
        {
            if (await _bookRepository.GetAllAsync(page, count) == null) return NotFound();
            List<Book> books = await _bookRepository.GetAllAsync(page, count);
            List<BookGetDto> getBooks = new List<BookGetDto>();
            foreach (var item in books)
            {
                BookGetDto book = new BookGetDto
                {
                    Id = item.Id,
                    Image = item.Image,
                    Name = item.Name,
                    Desc = item.Desc,
                    SalePrice = item.SalePrice,
                    ProducingPrice = item.ProducingPrice,
                    Status = item.Status,
                    IsDeleted = false,
                    CreatedAt = DateTime.UtcNow.AddHours(4),
                    AuthorName=item.Author.FullName,
                    GenreName=item.Genre.Name,
                    CategoryName=item.Genre.Category.Name
                };

                book.Tags = new List<TagItemDto>();
                foreach (var item1 in item.BookTags)
                {
                    TagItemDto tag = new TagItemDto
                    {
                        Id = item1.Tag.Id,
                        Name = item1.Tag.Name,
                    };
                    book.Tags.Add(tag);
                }
                getBooks.Add(book);
            }
            return StatusCode(200, getBooks);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            Book bookDatabase = await _bookRepository.GetAsync(id);
            if (bookDatabase == null) return NotFound();
            BookGetDto book = new BookGetDto
            {
                Id = bookDatabase.Id,
                Image = bookDatabase.Image,
                Name = bookDatabase.Name,
                Desc = bookDatabase.Desc,
                SalePrice = bookDatabase.SalePrice,
                ProducingPrice = bookDatabase.ProducingPrice,
                Status = bookDatabase.Status,
                IsDeleted = false,
                CreatedAt = DateTime.UtcNow.AddHours(4),
                AuthorName = bookDatabase.Author.FullName,
                GenreName = bookDatabase.Genre.Name,
                CategoryName=bookDatabase.Genre.Category.Name
            };
            if (bookDatabase.BookTags != null)
            {
                book.Tags = new List<TagItemDto>();
                foreach (var item1 in bookDatabase.BookTags)
                {
                    TagItemDto tag = new TagItemDto
                    {
                        Id = item1.Tag.Id,
                        Name = item1.Tag.Name,
                    };
                    book.Tags.Add(tag);

                }
            }
            
            return Ok(book);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (await _bookRepository.GetAsync(id) == null) return NotFound();
            Book book = await _bookRepository.GetAsync(id);
            //if(book.BookTags != null){
            //    foreach (var item in book.BookTags)
            //    {
            //        _bookTagRepository.Remove(item);
            //    }
            //}
            //_bookRepository.Remove(await _bookRepository.GetAsync(id));
            book.IsDeleted = true;
            return Ok(await _bookRepository.CommitAsync());
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromForm] BookCreateDto createDto, int id)
        {
            if (createDto == null || createDto.File == null) return Conflict();
            Book existBook = await _bookRepository.GetAsync(id);
            if (existBook == null) return NotFound();
            if (await _authorRepository.GetAsync(createDto.AuthorId)==null) return BadRequest();
            if (await _genreRepository.GetAsync(createDto.GenreId)==null) return BadRequest();


            if (createDto.File.ContentType != "image/png" && createDto.File.ContentType != "image/jpeg")
            {
                return Conflict();
            }
            if (createDto.File.Length > (1024 * 1024) * 5)
            {
                return Conflict();
            }
            string rootPath = _env.WebRootPath;
            var fileName = Guid.NewGuid().ToString() + createDto.File.FileName;
            var path = Path.Combine(rootPath, "uploads/book", fileName);
            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                createDto.File.CopyTo(stream);
            }
            if (existBook.Image != null)
            {
                string existPath = Path.Combine(_env.WebRootPath, "uploads/book", existBook.Image);
                if (System.IO.File.Exists(existPath))
                {
                    System.IO.File.Delete(existPath);
                }
            }
            existBook.Image = fileName;
            existBook.AuthorId = createDto.AuthorId;
            existBook.GenreId = createDto.GenreId;
            existBook.Name = createDto.Name;
            existBook.Desc = createDto.Desc;
            existBook.SalePrice = createDto.SalePrice;
            existBook.ProducingPrice = createDto.ProducingPrice;
            existBook.Status = createDto.Status;
            existBook.CreatedAt = DateTime.UtcNow.AddHours(4);
            List<BookTag> existBookTags = new List<BookTag>();

            if ( existBook.BookTags != null){
                existBookTags.AddRange(existBook.BookTags);
            }


            if (createDto.TagIds!=null)
            {

                    foreach (var item in createDto.TagIds)
                    {
                        Tag tag = await _tagRepository.GetAsync(item);
                        if (tag == null) return NotFound();
                        BookTag bookTag = new BookTag
                        {
                            BookId = existBook.Id,
                            TagId = item,
                        };
                        await _bookTagRepository.CreateAsync(bookTag);
                        await _bookTagRepository.CommitAsync();
                    existBook.BookTags.Add(bookTag);
                    }
            }
            foreach (var item in existBookTags)
            {
                existBook.BookTags.Remove(item) ;

            }
            await _bookRepository.CommitAsync();
            return StatusCode(200);

        }
    }
}
