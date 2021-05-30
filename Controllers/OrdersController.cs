using System;
using System.Data;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using NorthwindWeb.DAL;
using NorthwindWeb.Models;
using NorthwindWeb.ViewModels;
using PagedList;

namespace NorthwindWeb.Controllers
{
    public class OrdersController : Controller
    {
        UnitOfWork _unitOfWork;

        public OrdersController()
        {
            _unitOfWork = new UnitOfWork();
        }

        // GET: Orders
        public ActionResult Index(
            int? page,
            string customerId,
            int? employeeId,
            int? shipperId,
            DateTime? orderDate,
            DateTime? requiredDate,
            DateTime? shippedDate,
            decimal? freight,
            string shipName,
            string shipAddress,
            string shipCity,
            string shipRegion,
            string shipPostalCode,
            string shipCountry
            )
        {
            var orderSearchModel = new OrderSearchModel
            {
                CustomerId = customerId,
                EmployeeId = employeeId,
                ShipperId = shipperId,
                OrderDate = orderDate,
                RequiredDate = requiredDate,
                ShippedDate = shippedDate,
                Freight = freight,
                ShipName = shipName,
                ShipAddress = shipAddress,
                ShipCity = shipCity,
                ShipRegion = shipRegion,
                ShipPostalCode = shipPostalCode,
                ShipCountry = shipCountry
            };

            return View(PrepareOrdersViewModel(orderSearchModel, page));
        }

        [HttpPost]
        public ActionResult Index(OrderSearchModel orderSearchModel)
        {
            return View(PrepareOrdersViewModel(orderSearchModel));
        }
        private OrdersViewModel PrepareOrdersViewModel(OrderSearchModel orderSearchModel, int? page = null)
        {
            var orders = _unitOfWork.OrderRepository.Get(includeProperties: "Customer,Employee,Shipper").AsQueryable();

            var customers = orders.Select(x => x.Customer).GroupBy(x => x.CustomerID).Select(grp => grp.First()).ToList();
            var employees = orders.Select(x => x.Employee).GroupBy(x => x.EmployeeID).Select(grp => grp.First()).ToList();
            var shippers = orders.Select(x => x.Shipper).GroupBy(x => x.ShipperID).Select(grp => grp.First()).ToList();

            orderSearchModel.CustomerSelectList = new SelectList(customers, "CustomerID", "CompanyName");
            orderSearchModel.EmployeeSelectList = new SelectList(employees, "EmployeeID", "LastName");
            orderSearchModel.ShipperSelectList = new SelectList(shippers, "ShipperID", "CompanyName");

            if (!string.IsNullOrEmpty(orderSearchModel.CustomerId))
                orders = orders.Where(x => x.CustomerID == orderSearchModel.CustomerId);

            if (orderSearchModel.EmployeeId.HasValue)
                orders = orders.Where(x => x.EmployeeID == orderSearchModel.EmployeeId);

            if (orderSearchModel.ShipperId.HasValue)
                orders = orders.Where(x => x.Shipper.ShipperID == orderSearchModel.ShipperId);

            if (orderSearchModel.OrderDate.HasValue)
                orders = orders.Where(x => x.OrderDate == orderSearchModel.OrderDate);

            if (orderSearchModel.RequiredDate.HasValue)
                orders = orders.Where(x => x.RequiredDate == orderSearchModel.RequiredDate);

            if (orderSearchModel.ShippedDate.HasValue)
                orders = orders.Where(x => x.ShippedDate == orderSearchModel.ShippedDate);

            if (orderSearchModel.Freight.HasValue)
                orders = orders.Where(x => x.Freight == orderSearchModel.Freight);

            if (!string.IsNullOrEmpty(orderSearchModel.ShipName))
                orders = orders.Where(x => x.ShipName == orderSearchModel.ShipName);

            if (!string.IsNullOrEmpty(orderSearchModel.ShipAddress))
                orders = orders.Where(x => x.ShipAddress == orderSearchModel.ShipAddress);

            if (!string.IsNullOrEmpty(orderSearchModel.ShipCity))
                orders = orders.Where(x => x.ShipCity == orderSearchModel.ShipCity);

            if (!string.IsNullOrEmpty(orderSearchModel.ShipRegion))
                orders = orders.Where(x => x.ShipRegion == orderSearchModel.ShipRegion);

            if (!string.IsNullOrEmpty(orderSearchModel.ShipPostalCode))
                orders = orders.Where(x => x.ShipPostalCode == orderSearchModel.ShipPostalCode);

            if (!string.IsNullOrEmpty(orderSearchModel.ShipCountry))
                orders = orders.Where(x => x.ShipCountry == orderSearchModel.ShipCountry);

            int pageSize = 10;
            int pageNumber = (page ?? 1);

            var ordersViewModel = new OrdersViewModel
            {
                OrderPagedList = orders.ToPagedList(pageNumber, pageSize),
                OrderSearchModel = orderSearchModel
            };

            return ordersViewModel;
        }

        // GET: Orders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = _unitOfWork.OrderRepository.GetByID(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // GET: Orders/Create
        public ActionResult Create()
        {
            var orders = _unitOfWork.OrderRepository.Get(includeProperties: "Customer,Employee,Shipper");

            var customers = orders.Select(x => x.Customer).ToList();
            var employees = orders.Select(x => x.Employee).ToList();
            var shippers = orders.Select(x => x.Shipper).ToList();

            ViewBag.CustomerID = new SelectList(customers, "CustomerID", "CompanyName");
            ViewBag.EmployeeID = new SelectList(employees, "EmployeeID", "LastName");
            ViewBag.ShipVia = new SelectList(shippers, "ShipperID", "CompanyName");

            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OrderID,CustomerID,EmployeeID,OrderDate,RequiredDate,ShippedDate,ShipVia,Freight,ShipName,ShipAddress,ShipCity,ShipRegion,ShipPostalCode,ShipCountry")] Order order)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.OrderRepository.Insert(order);
                _unitOfWork.Save();
                return RedirectToAction("Index");
            }

            var orders = _unitOfWork.OrderRepository.Get(includeProperties: "Customer,Employee,Shipper");

            var customers = orders.Select(x => x.Customer).ToList();
            var employees = orders.Select(x => x.Employee).ToList();
            var shippers = orders.Select(x => x.Shipper).ToList();

            ViewBag.CustomerID = new SelectList(customers, "CustomerID", "CompanyName");
            ViewBag.EmployeeID = new SelectList(employees, "EmployeeID", "LastName");
            ViewBag.ShipVia = new SelectList(shippers, "ShipperID", "CompanyName");

            return View(order);
        }

        // GET: Orders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Order order = _unitOfWork.OrderRepository.GetByID(id);

            if (order == null)
            {
                return HttpNotFound();
            }

            var orders = _unitOfWork.OrderRepository.Get(includeProperties: "Customer,Employee,Shipper");

            var customers = orders.Select(x => x.Customer).ToList();
            var employees = orders.Select(x => x.Employee).ToList();
            var shippers = orders.Select(x => x.Shipper).ToList();

            ViewBag.CustomerID = new SelectList(customers, "CustomerID", "CompanyName");
            ViewBag.EmployeeID = new SelectList(employees, "EmployeeID", "LastName");
            ViewBag.ShipVia = new SelectList(shippers, "ShipperID", "CompanyName");

            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OrderID,CustomerID,EmployeeID,OrderDate,RequiredDate,ShippedDate,ShipVia,Freight,ShipName,ShipAddress,ShipCity,ShipRegion,ShipPostalCode,ShipCountry")] Order order)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.OrderRepository.Update(order);
                _unitOfWork.Save();

                return RedirectToAction("Index");
            }

            var orders = _unitOfWork.OrderRepository.Get(includeProperties: "Customer,Employee,Shipper");

            var customers = orders.Select(x => x.Customer).ToList();
            var employees = orders.Select(x => x.Employee).ToList();
            var shippers = orders.Select(x => x.Shipper).ToList();

            ViewBag.CustomerID = new SelectList(customers, "CustomerID", "CompanyName");
            ViewBag.EmployeeID = new SelectList(employees, "EmployeeID", "LastName");
            ViewBag.ShipVia = new SelectList(shippers, "ShipperID", "CompanyName");

            return View(order);
        }

        // GET: Orders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = _unitOfWork.OrderRepository.GetByID(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Order order = _unitOfWork.OrderRepository.GetByID(id);
            _unitOfWork.OrderRepository.Delete(order);
            _unitOfWork.Save();

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _unitOfWork.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
