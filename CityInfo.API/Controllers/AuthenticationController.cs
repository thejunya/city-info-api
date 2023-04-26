using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CityInfo.API.Entities;
using CityInfo.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace CityInfo.API.Controllers;

[Route("api/authentication")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly IRepository _repository;

    public AuthenticationController(IConfiguration configuration, IRepository repository)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    [HttpPost("authenticate")]
    public async Task<ActionResult<string>> Authenticate(AuthenticationRequestBody authenticationRequestBody)
    {
        var user = await ValidateUserCredentials(authenticationRequestBody);

        if (user == null)
            return Unauthorized();

        var authenticationKey = _configuration["Authentication:Key"] ?? string.Empty;

        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(authenticationKey));

        var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

        var claimsForToken = new List<Claim>()
        {
            new Claim("Id", user.Id.ToString()),
            new Claim("Login", user.Login),
        };

        var jwtSecurityToken = new JwtSecurityToken(
            _configuration["Authentication:Issuer"],
            _configuration["Authentication:Audience"],
            claimsForToken,
            DateTime.UtcNow,
            DateTime.UtcNow.AddHours(1),
            signingCredentials);

        var tokenToReturn = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

        return Ok(tokenToReturn);
    }

    private async Task<User?> ValidateUserCredentials(AuthenticationRequestBody authenticationRequestBody)
    {
        return await _repository.GetUser(authenticationRequestBody.Login, authenticationRequestBody.Password);
    }
}

public class AuthenticationRequestBody
{
    public string? Login { get; set; }
    public string? Password { get; set; }
}