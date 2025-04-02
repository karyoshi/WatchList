using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WatchListMVC.Data;
using WatchListMVC.Models;

namespace WatchListMVC.Controllers
{
    public class MovieController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public MovieController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddMovieViewModel addMovieViewModel,IFormFile imageFile)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await imageFile.CopyToAsync(memoryStream);
                    addMovieViewModel.ImageData = memoryStream.ToArray(); // Save the image as a BLOB
                }
            }
            var movie = new MovieModel
            {
                Title = addMovieViewModel.Title,
                Description = addMovieViewModel.Description,
                Type = addMovieViewModel.Type,
                WatchDate = addMovieViewModel.WatchDate,
                Platform = addMovieViewModel.Platform,
                ImageData = addMovieViewModel.ImageData

            };


            await dbContext.Movies.AddAsync(movie);
            await dbContext.SaveChangesAsync();

            return RedirectToAction("List", "Movie");
        }


        [HttpGet]
        public async Task<IActionResult> List()
        {
            var movies = await dbContext.Movies.ToListAsync();
            return View(movies);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var movies = await dbContext.Movies.FindAsync(id);
            return View(movies);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(MovieModel movieModel, IFormFile imageFile)
        {
            var movies = await dbContext.Movies.FindAsync(movieModel.Id);

            if (movies is not null)
            {
                movieModel.Title = movies.Title;
                movieModel.Description = movies.Description;
                movieModel.Type = movies.Type;
                movieModel.WatchDate = movies.WatchDate;
                movieModel.Platform = movies.Platform;
                if (imageFile != null && imageFile.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await imageFile.CopyToAsync(memoryStream);
                        movies.ImageData = memoryStream.ToArray(); // Save the image as a BLOB
                    }
                }
                await dbContext.SaveChangesAsync();
            }
            return RedirectToAction("List", "Movie");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(MovieModel movieModel)
        {
            var movies = await dbContext.Movies
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == movieModel.Id);

            if (movies is not null)
            {
                dbContext.Movies.Remove(movieModel);
                await dbContext.SaveChangesAsync();
            }
            return RedirectToAction("List", "Movie");
        }
    }
}
