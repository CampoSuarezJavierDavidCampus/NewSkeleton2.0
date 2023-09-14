using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Api.Dtos.Auth;
using Domain.Entities;
using Domain.Interfaces.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace Api.Helpers;

public sealed class TokenManager : ITokenManager{
    private readonly IPasswordHasher<User> _PasswordHasher;
    private readonly IConfiguration _Conf;
    private readonly int _AccessTokenDuration;
    private readonly int _RefreshTokenTokenDuration;

    public TokenManager(IPasswordHasher<User> passwordHasher, IConfiguration conf ){
        _Conf = conf;
        _PasswordHasher = passwordHasher;
        //--Token duration
        _ = int.TryParse(conf["JWTSettings:AccessTokenTimeInMinutes"], out _AccessTokenDuration);
        _ = int.TryParse(conf["JWTSettings:RefreshTokenTimeInHours"], out _RefreshTokenTokenDuration); 

    }

    public  string CreateAccessToken(User user){        
        //-Define Claims
        var claims = new List<Claim>(){
            new(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
            new(ClaimTypes.Name,user.Username),
            new(ClaimTypes.Email,user.Email!),
            new(ClaimTypes.Role,user.Role!.Description)
        };        

        //-Return Token
        return CreateToken(claims,DateTime.Now.AddMinutes(_AccessTokenDuration));
    }

    public  string CreateRefreshToken(){
        //-Define Claims
        var claims = new List<Claim>(){
                new(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())};

        //-Return Token
        return CreateToken(claims,DateTime.Now.AddHours(_RefreshTokenTokenDuration));
    }
    
    public User CreateUser(UserSignup model){
        //-Crear Usuario
        User user = new(){
            Email = model.Email!,
            Username = model.Username!
        };

        //-Encriptar Password
        user.Password = _PasswordHasher.HashPassword(user,model.Password!);

        //-Retornar usuario
        return user;
    }    
    
    //Validar contraseña
    public  bool ValidatePassword(User user, string password){        
        return _PasswordHasher.VerifyHashedPassword(
                    user, 
                    user.Password, 
                    password
                ) == PasswordVerificationResult.Success;
    }

    //-Validar token
    public (ClaimsPrincipal principal, SecurityToken validatedToken) GetTokenInformation(string tokenString){
        JwtSecurityTokenHandler tokenHandler = new();

        //-Parametros a validar
        TokenValidationParameters validationParameters = new(){
            ValidateIssuer = true,
            ValidateAudience = true,
            IssuerSigningKey = GetSecurityKey(),  
            ClockSkew = TimeSpan.FromMinutes(5),
            ValidateLifetime = true              
        };
        
        //-Validar token
        var principal = tokenHandler.ValidateToken(tokenString, validationParameters, out SecurityToken validatedToken);
        
        return (principal,validatedToken);            
    }

    private SymmetricSecurityKey GetSecurityKey(){
        //-Obterner Key
        var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    _Conf["JWTSettings:Key"] ??
                    throw new Exception("Error: key not found"
                )));         
        return key;   
    }
    
    private string CreateToken(IEnumerable<Claim> claims, DateTime expireTime){        
        //-Firmar Credenciales
        var credentials = new SigningCredentials(GetSecurityKey(), SecurityAlgorithms.HmacSha512Signature);            

        //-Agregar descripcion
        var tokenDescription = new SecurityTokenDescriptor{
            Issuer = _Conf["JWTSettings:Issuer"],
            Audience = _Conf["JWTSettings:Audience"],
            Subject = new ClaimsIdentity(claims),
            Expires = expireTime,
            SigningCredentials = credentials
        };

        //-Crear Token
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescription);

        //-Retornar token como string
        return tokenHandler.WriteToken(token);        
    }    

}
