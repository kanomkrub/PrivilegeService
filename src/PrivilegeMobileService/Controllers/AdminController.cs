using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Security.Principal;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Cors;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PrivilegeMobileService.Controllers
{
    /// <summary>
    /// backdoor api for debug purpose only!!
    /// </summary>
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class AdminController : Controller
    {
        private readonly JwtIssuerOptions _jwtOptions;
        private readonly ILogger _logger;
        private readonly JsonSerializerSettings _serializerSettings;

        public AdminController(IOptions<JwtIssuerOptions> jwtOptions, ILoggerFactory loggerFactory)
        {
            _jwtOptions = jwtOptions.Value;
            ThrowIfInvalidOptions(_jwtOptions);

            _logger = loggerFactory.CreateLogger<AdminController>();

            _serializerSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented
            };
        }
        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            var autho = Request.HttpContext.User;
            return new string[] { "value1", "value2" };
        }
        
        [HttpPost]
        [Route("TestAuthen")]
        [AllowAnonymous]
        public async Task<IActionResult> Get([FromQuery] string username, [FromQuery] string password)
        {
            if(username ==null || password == null)
            {
                //string autho = Request.Headers["Authorization"];
                //if(autho.Trim().StartsWith())
            }
            var identity = await GetClaimsIdentity(username, password);
            if (identity == null)
            {
                _logger.LogInformation($"Invalid username ({username}) or password ({password})");
                return BadRequest("Invalid credentials");
            }

            var claims = new[]
            {
        new Claim(JwtRegisteredClaimNames.Sub, username),
        new Claim(JwtRegisteredClaimNames.Jti, await _jwtOptions.JtiGenerator()),
        new Claim(JwtRegisteredClaimNames.Iat,
                  ToUnixEpochDate(_jwtOptions.IssuedAt).ToString(),
                  ClaimValueTypes.Integer64),
                identity.FindFirst("RoleJa"),
                identity.FindFirst("customer_email"),
                identity.FindFirst("customer_fullname")
      };

            // Create the JWT security token and encode it.
            var jwt = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claims,
                notBefore: _jwtOptions.NotBefore,
                expires: _jwtOptions.Expiration,
                signingCredentials: _jwtOptions.SigningCredentials);

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            // Serialize and return the response
            var response = new
            {
                access_token = encodedJwt,
                expires_in = (int)_jwtOptions.ValidFor.TotalSeconds
            };

            var json = JsonConvert.SerializeObject(response, _serializerSettings);
            return new OkObjectResult(json);
        }

        private static void ThrowIfInvalidOptions(JwtIssuerOptions options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));

            if (options.ValidFor <= TimeSpan.Zero)
            {
                throw new ArgumentException("Must be a non-zero TimeSpan.", nameof(JwtIssuerOptions.ValidFor));
            }

            if (options.SigningCredentials == null)
            {
                throw new ArgumentNullException(nameof(JwtIssuerOptions.SigningCredentials));
            }

            if (options.JtiGenerator == null)
            {
                throw new ArgumentNullException(nameof(JwtIssuerOptions.JtiGenerator));
            }
        }

        private static long ToUnixEpochDate(DateTime date)
      => (long)Math.Round((date.ToUniversalTime() -
                           new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero))
                          .TotalSeconds);

        /// <summary>
        /// Test Auth Only can auth everyone as customer if password is "password"
        /// </summary>
        private static Task<ClaimsIdentity> GetClaimsIdentity(string username, string password)
        {
            if(password != "password") return Task.FromResult<ClaimsIdentity>(null);

            return Task.FromResult(new ClaimsIdentity(
              new GenericIdentity(username, "Token"),
              new[]
              {
            new Claim("RoleJa", "Customer"), new Claim("customer_email", $"{username}@xmail.com"), new Claim("customer_fullname", $"{username}_krub")
              }));
        }
    }
}
