using NorthwindWeb.DAL;
using NorthwindWeb.DTO;
using NorthwindWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NorthwindWeb.Service
{
    public class OrderService
    {
        UnitOfWork _unitOfWork;

        public OrderService()
        {
            _unitOfWork = new UnitOfWork();
        }


        public OrderDto GetOrderDto(int id)
        {
            Order order = _unitOfWork.OrderRepository.GetByID(id);

            OrderDto orderDto = new OrderDto();

            if (order != null)
            {
                orderDto.CustomerID = order.CustomerID;
                orderDto.EmployeeID = order.EmployeeID;
                orderDto.Freight = order.Freight;
                orderDto.OrderDate = order.OrderDate;
                orderDto.OrderID = order.OrderID;
                orderDto.RequiredDate = order.RequiredDate;
                orderDto.ShipAddress = order.ShipAddress;
                orderDto.ShipCity = order.ShipCity;
                orderDto.ShipCountry = order.ShipCountry;
                orderDto.ShipName = order.ShipName;
                orderDto.ShippedDate = order.ShippedDate;
                orderDto.ShipPostalCode = order.ShipPostalCode;
                orderDto.ShipRegion = order.ShipRegion;
                orderDto.ShipVia = order.ShipVia;
                if (order.Customer != null)
                {
                    orderDto.CustomerDto = new CustomerDto()
                    {
                        CustomerID = order.Customer.CustomerID,
                        Address = order.Customer.Address,
                        City = order.Customer.City,
                        CompanyName = order.Customer.CompanyName,
                        ContactName = order.Customer.ContactName,
                        ContactTitle = order.Customer.ContactTitle,
                        Fax = order.Customer.Fax,
                        Phone = order.Customer.Phone,
                        PostalCode = order.Customer.PostalCode,
                        Region = order.Customer.Region,
                        Country = order.Customer.Country
                    };
                }
                if (order.Employee != null)
                {
                    orderDto.EmployeeDto = new EmployeeDto()
                    {
                        Country = order.Employee.Country,
                        Region = order.Employee.Region,
                        PostalCode = order.Employee.PostalCode,
                        Address = order.Employee.Address,
                        BirthDate = order.Employee.BirthDate,
                        City = order.Employee.City,
                        EmployeeID = order.Employee.EmployeeID,
                        Extension = order.Employee.Extension,
                        FirstName = order.Employee.FirstName,
                        HireDate = order.Employee.HireDate,
                        HomePhone = order.Employee.HomePhone,
                        LastName = order.Employee.LastName,
                        Notes = order.Employee.Notes,
                        Photo = order.Employee.Photo,
                        PhotoPath = order.Employee.PhotoPath,
                        ReportsTo = order.Employee.ReportsTo,
                        Title = order.Employee.Title,
                        TitleOfCourtesy = order.Employee.TitleOfCourtesy
                    };
                }
                if (order.Shipper != null)
                {
                    orderDto.ShipperDto = new ShipperDto()
                    {
                        CompanyName = order.Shipper.CompanyName,
                        Phone = order.Shipper.Phone,
                        ShipperID = order.Shipper.ShipperID
                    };
                }
                if (order.Order_Details.Any())
                {
                    orderDto.Order_Detail_Dto = order.Order_Details.Select(x =>
                                                                                new Order_Detail_Dto
                                                                                {
                                                                                    Discount = x.Discount,
                                                                                    OrderID = x.OrderID,
                                                                                    ProductID = x.ProductID,
                                                                                    Quantity = x.Quantity,
                                                                                    UnitPrice = x.UnitPrice
                                                                                }).ToList();
                }
            }

            return orderDto;
        }
    }
}