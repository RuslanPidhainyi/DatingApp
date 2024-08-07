﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Entities;
using API.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace API.Services;

public class TokenService(IConfiguration config) : ITokenService
{
   public string CreateToken(AppUser user)
   {
      var tokenKey = config["TokenKey"] ?? throw new Exception("Cannot access tokenKey from appsettings"); //?? - jezeli null to zrob cos
      if(tokenKey.Length < 64) throw new Exception("Your tokenKey needs to be longer");

      var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));//Teraz nasz token będzie zawierał informacje o użytkowniku

      /*Istnieje 2 rodzaje klucza: Symetryczny i Asymetryczny.
      
      SymmetricSecurityKey - symetryczny klucz bezpiczenstwa  szyfrowania i odszyfrowania informacji. 
      
      AsymetrycznySecurityKey - asymetryczny klucz  to proces z ktorym mamy więc jeden klucz do szyfrowania i drugi do odszyfrowania, ale w naszym przypadku wybieramy jeden klucz, aby rządzić nimi wszystkimi oraz szyfrować  i odszyfrować. ale kiedy używamy tego systemu, musimy upewnić się, że klucz jest bezpiecznie przechowywany na naszym serwerze i nikt nie może uzyskać do niego dostęp i nikt nie może go po prostu zobaczyć*/

      //claims - Twierdzenie o userze - czymś co user moze powiedzić o siebie. (np. Mogą powiedzić, że moja data urodzenia jest taka. Twierdzenie, że mam na imię Bob).

      //Tojest standartna definicja claims/Twierdzenie o userze ich wewnętrz tokena
      var claims = new List<Claim>
      {
         new Claim(ClaimTypes.NameIdentifier, user.UserName)
      };

      var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

      //opis tokena
      var tokenDescriptor = new SecurityTokenDescriptor
      {
         Subject = new ClaimsIdentity(claims),
         Expires = DateTime.UtcNow.AddDays(7),
         SigningCredentials = creds
      };

      //Obsugę tokenów 
      var tokenHandler = new JwtSecurityTokenHandler();
      var token = tokenHandler.CreateToken(tokenDescriptor);

      return tokenHandler.WriteToken(token);
   }
}
