using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SampleApp.Data;
using SampleApp.Models;

namespace SampleApp.Pages_Customer
{
    [Authorize(Roles="Admin")]
    public class IndexModel : PageModel
    {
        private readonly SampleApp.Data.SampleAppDbContext _context;

        public IndexModel(SampleApp.Data.SampleAppDbContext context)
        {
            _context = context;
        }

        public PaginatedList<Customer> Customer { get;set; }
        public string CurrentFirstSort {get; set;}
        public string CurrentLastSort {get; set;}
        public string CurrentDOBSort {get; set;}
       public string CurrentCreateDateSort {get; set;}
        public string NameFilter {get; set;}
        
        [RegularExpression(@"^[0-9]{5}(?:-[0-9]{4})?$")]
        public string ZipFilter {get; set;}
        public string CountryFilter {get; set;}
        public string EmailFilter {get; set;}

        public async Task OnGetAsync(bool? clearSort, bool? clearSearch, string firstOrder, string lastOrder, string nameSearch, string zipSearch,string countrySearch, string emailSearch, int? pageIndex)
        {
            var orderParam = "";

            //Set all of the current sorts to empty strings to exclude order by clause
            if(clearSort != null && clearSort.Value)
            {
                CurrentFirstSort = "";
                CurrentLastSort = "";
                CurrentDOBSort = "";
                CurrentCreateDateSort = "";
                
            }
            else
            {
                //If current sort is ascending, switch to descending. If not currently sorted, default to ascending
                if(!string.IsNullOrEmpty(firstOrder))
                {
                    CurrentFirstSort = firstOrder == "FirstName ASC" ? "FirstName DESC" : "FirstName ASC";                    
                }

                if(!string.IsNullOrEmpty(lastOrder))
                {
                    CurrentLastSort = lastOrder == "LastName ASC" ? "LastName DESC" : "LastName ASC";
                }
                
                   
                //Builds the order by clause to pass to the sproc
                orderParam = $"{(string.IsNullOrEmpty(CurrentFirstSort) ? "" : CurrentFirstSort+ ",")}" + 
                $"{(string.IsNullOrEmpty(CurrentLastSort) ? "" : CurrentLastSort+ ",")}" + $"{(string.IsNullOrEmpty(CurrentDOBSort) ? "" : CurrentDOBSort + ",")}" +
                $"{(string.IsNullOrEmpty(CurrentCreateDateSort) ? "" : CurrentCreateDateSort+ ",")}";
            }   

            //If clearing search, set search fields to empty and start over at page one after data refresh
            if(clearSearch != null && clearSearch.Value)
            {
                pageIndex = 1;
                CountryFilter = "";
                NameFilter = "";
                ZipFilter = "";
                EmailFilter = "";
            }         

            //If data is currently being sorted, clean up parameter and reset page to 1
            if(!string.IsNullOrEmpty(orderParam))
            {
                orderParam.Remove(orderParam.Length - 1);
                orderParam = $"ORDER BY {orderParam}";
                pageIndex = 1;
            }

            int pageSize = 5;

            var data = _context.Customer.FromSql($"GetCustomers {NameFilter},{ZipFilter},{CountryFilter},{EmailFilter},{orderParam}");                  

            Customer = await PaginatedList<Customer>.CreateAsync(data,pageIndex ?? 1, pageSize);
        }
    }
}
