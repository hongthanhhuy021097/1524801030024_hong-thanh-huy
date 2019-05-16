using HelloWeb.Data.Infrastructure;
using HelloWeb.Data.Repositories;
using HelloWeb.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloWeb.Service
{
    public interface IOrderService
    {
        bool Create(Order order, List<OrderDetail> orderDetails);
        IEnumerable<Order> getAll(String keyWord);
        IEnumerable<OrderDetail> GetOrderDetailById(int id);
        Order GetOrderById(int id);
    }
    public class OrderService : IOrderService
    {
        IOrderRepository _orderRepository;
        IOrderDetailRepository _orderDetailRepository;
        IUnitOfWork _unitOfWork;

        public OrderService(IOrderRepository orderRepository, IOrderDetailRepository orderDetailRepository, IUnitOfWork unitOfWork)
        {
            this._orderRepository = orderRepository;
            this._orderDetailRepository = orderDetailRepository;
            this._unitOfWork = unitOfWork;
        }



        public bool Create(Order order, List<OrderDetail> orderDetails)
        {
            try
            {
                _orderRepository.Add(order);
                _unitOfWork.Commit();

                foreach (var orderDetail in orderDetails)
                {
                    orderDetail.OrderID = order.ID;
                    _orderDetailRepository.Add(orderDetail);
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<Order> getAll(string keyWord)
        {
            if (String.IsNullOrEmpty(keyWord))
            {
                return _orderRepository.GetAll();
            }
            else
            {
                return _orderRepository.GetMulti(x => x.CustomerName.Contains(keyWord) || x.CustomerEmail.Contains(keyWord));
            }
        }

        public Order GetOrderById(int id)
        {
            return _orderRepository.GetSingleById(id);
        }

        public IEnumerable<OrderDetail> GetOrderDetailById(int id)
        {

            return  _orderDetailRepository.GetMulti(x => x.OrderID==id);
        }
    }
}
