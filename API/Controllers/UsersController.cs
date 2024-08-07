﻿using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public class UsersController(DataContext context) : BaseApiController
{
   [AllowAnonymous]
   //Metoda do zwracania odpowiedzi HTTP  do klienta 
   [HttpGet] //Ządania HTTP Get 
   public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers() /*publiczna metoda.

                                                         Result action - jako typ rzeczy, ktore zamierzamy zwrocic z tego  punktu koncowego API

                                                         Następnie okreslamy typ  zwracanej rzeczy  - teraz zwrocimy liste użytkownikow (Istneje  wiele róznych  rodzajów list w C#, jedna z metod, ktorej mozemy do tego użyc jest enumarable - uzywane tylko dla kolekcji okreslonego typu) - zwrocimy IEnumereble typu appUser*/
   {
      //Stąd możemy zwracac odpowiedz HTTP
      var users =  await context.Users.ToListAsync(); //w ten sposob otrzymamy liste user'ow z naszej bazy
      return users;
   }


   [Authorize]
   //W tym przypadku chcemy uzysjac indywidualnego uzytkownaka
   //oprocz user'a API, chelibysmy wiedzic id user'a np w URL musi byc cos  takiego
   [HttpGet("{id:int}")]  //np w URL musi byc cos  takiego - api/user/1 //:int - to jest bezpeczenstwo typu i okreslic ograniczenie - mowi nam ze nasz identyfikator biedzie typu integer.  
   public async Task<ActionResult<AppUser>> GetUser(int id) 
   {
      var user = await context.Users.FindAsync(id); 
      if(user == null) return NotFound();   
      return user;
   }

}
