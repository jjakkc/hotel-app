using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using HotelAppLibrary.Data;
using HotelAppLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HotelApp.Web.Pages
{
    public class RoomBookModel : PageModel
    {
        private readonly IDatabaseData _db;

        // need SupportsGet so we can receive values across link when using asp-route attribute
        [BindProperty(SupportsGet = true)]
        public int RoomTypeId { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime StartDate { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime EndDate { get; set; }

        [BindProperty]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [BindProperty]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }


        public RoomType RoomType { get; set; }

        public RoomBookModel(IDatabaseData db)
        {
            _db = db;
        }

        public void OnGet()
        {
            if(RoomTypeId > 0)
            {
                RoomType = _db.GetRoomTypeById(RoomTypeId);
            }
        }

        public IActionResult OnPost()
        {
            _db.BookGuest(FirstName, LastName, RoomTypeId, StartDate, EndDate);
            return RedirectToPage("/Index");
        }
    }
}
