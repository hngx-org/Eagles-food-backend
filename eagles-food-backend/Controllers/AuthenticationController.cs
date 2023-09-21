using eagles_food_backend.Data;
using eagles_food_backend.Domains.DTOs;
using eagles_food_backend.Domains.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace eagles_food_backend.Controllers
{
	[Route("api/auth")]
	[ApiController]
	public class AuthenticationController : ControllerBase
	{
		private readonly LunchDbContext _context;
		private readonly IMapper _mapper;
		public AuthenticationController(LunchDbContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}

		// logs in a user/org
		[HttpPost("/login")]
		public async Task<ActionResult<User>> Login(UserLoginDTO userLogin)
		{
			var userInDB = await _context.users.Where(user => user.username == userLogin.username).FirstOrDefaultAsync();
			var newUser = _mapper.Map<CreateUserDTO>(userindb);

			try
			{
				var loggedUser = await _context.users.Where(user => user.username == userLogin.username).FirstOrDefaultAsync();

				if (loggedUser != null)
				{
					if (ModelState.IsValid)
					{
						_passwordHasher.VerifyHashedPassword(newUser, loggedUser.password_hash.ToString(), userLogin.password);

						return Ok(loggedUser);
					}
				}
			}
			catch (Exception)
			{
				ModelState.AddModelError("User not found", "This user does not exist ");
				throw;
			}

			return NotFound();





		}

		[HttpPost("/registere")]
		public async Task<ActionResult<User>> Register(CreateUserDTO userCreate)
		{
			var hashed = _passwordHasher.HashPassword(userCreate, userCreate.password);
			user.username = userCreate.username;
			user.email = userCreate.email;
			user.first_name = userCreate.first_name;
			user.last_name = userCreate.last_name;


			_context.users.Add(user);
			await _context.SaveChangesAsync();

			return Ok(user);
		}

		private void CreatePasswordHash(string password)
		{
			//byte[] salt = RandomNumberGenerator.GetBytes(128 / 8);
			//string hashedPassword = Convert.ToBase64String(
			//	KeyDerivation.Pbkdf2(password,salt,KeyDerivationPrf.HMACSHA512,100000,256/8));

		}

		private void CreateToken()
		{

		}
	}


}
