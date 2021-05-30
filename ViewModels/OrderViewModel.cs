using NorthwindWeb.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace NorthwindWeb.ViewModels
{
    public class OrdersViewModel
    {
        public IPagedList<Order> OrderPagedList { get; set; }

        public OrderSearchModel OrderSearchModel { get; set; }
    }

    public class OrderSearchModel
    {
        public string CustomerId { get; set; }

        public string CustomerCompanyName { get; set; }

        public int? EmployeeId { get; set; }

        public string EmployeeLastName { get; set; }

        public int? ShipperId { get; set; }

        public string ShipperCompanyName { get; set; }

        public DateTime? OrderDate { get; set; }

        public DateTime? RequiredDate { get; set; }

        public DateTime? ShippedDate { get; set; }

        public decimal? Freight { get; set; }

        public string ShipName { get; set; }

        public string ShipAddress { get; set; }

        public string ShipCity { get; set; }

        public string ShipRegion { get; set; }

        public string ShipPostalCode { get; set; }

        public string ShipCountry { get; set; }

        public IEnumerable<SelectListItem> CustomerSelectList { get; set; }

        public IEnumerable<SelectListItem> EmployeeSelectList { get; set; }

        public IEnumerable<SelectListItem> ShipperSelectList { get; set; }

    }
}