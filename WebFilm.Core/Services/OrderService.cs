using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebFilm.Core.Enitites.Cart;
using WebFilm.Core.Enitites.Category;
using WebFilm.Core.Enitites.Order;
using WebFilm.Core.Enitites.Payment;
using WebFilm.Core.Enitites.Product;
using WebFilm.Core.Enitites.Rating;
using WebFilm.Core.Enitites.User;
using WebFilm.Core.Exceptions;
using WebFilm.Core.Hub;
using WebFilm.Core.Interfaces.Repository;
using WebFilm.Core.Interfaces.Services;

namespace WebFilm.Core.Services
{
    public class OrderService : BaseService<int, Order>, IOrderService
    {
        IUserContext _userContext;
        IOrderRepository _orderRepository;
        ICartDetailRepository _cartDetailRepository;
        ICartRepository _cartRepository;
        IOrderProductRepository _orderProductRepository;
        IProductDetailRepository _productDetailRepository;
        IProductRepository _productRepository;
        IUserRepository _userRepository;
        IRatingRepository _ratingRepository;
        WebSocketHub _webSocketHub;
        IPaymentRepository _paymentRepository;
        private readonly IConfiguration _configuration;

        public OrderService(IOrderRepository orderRepository,
            IConfiguration configuration,
            IUserContext userContext,
            ICartDetailRepository cartDetailRepository,
            ICartRepository cartRepository,
            IOrderProductRepository orderProductRepository,
            IProductDetailRepository productDetailRepository,
            IProductRepository productRepository,
            WebSocketHub webSocketHub,
            IUserRepository userRepository,
            IPaymentRepository paymentRepository,
            IRatingRepository ratingRepository) : base(orderRepository)
        {
            _configuration = configuration;
            _userContext = userContext;
            _orderRepository = orderRepository;
            _cartDetailRepository = cartDetailRepository;
            _cartRepository = cartRepository;
            _orderProductRepository = orderProductRepository;
            _productDetailRepository = productDetailRepository;
            _productRepository = productRepository;
            _webSocketHub = webSocketHub;
            _userRepository = userRepository;
            _paymentRepository = paymentRepository;
            _ratingRepository = ratingRepository;
        }

        public Order newOrder(OrderCreateDTO dto)
        {
            int customerID = _userContext.UserId;
            if (customerID == 0)
            {
                throw new ServiceException(Resources.Resource.Not_Permission);
            }

            List<OrderProduct> products = new List<OrderProduct>();
            List<OrderDTO> orderDTOS = dto.products;
            //Cart cart = _cartRepository.getCart(customerID);
            //List<CartDetail> cartDetails = _cartDetailRepository.GetAll().Where(p => p.cartID == cart.id).ToList();
            Order newOrder = _orderRepository.newOrder(customerID, orderDTOS.Sum(x => x.price), dto.address ?? "", dto.method ?? "");

            //add payment
            _paymentRepository.newPayment(newOrder.id, newOrder.price);
            //order product
            foreach (OrderDTO orderDTO in orderDTOS)
            {
                OrderProduct orderProduct = new OrderProduct();
                orderProduct.price = orderDTO.price;
                orderProduct.createdDate = DateTime.Now;
                orderProduct.modifiedDate = DateTime.Now;
                orderProduct.productDetailID = orderDTO.productDetailID;
                orderProduct.orderID = newOrder.id;
                orderProduct.quantity = orderDTO.quantity;
                _orderProductRepository.Add(orderProduct);
                
                //change quantity productdetail
                ProductDetail productDetail = _productDetailRepository.GetByID(orderDTO.productDetailID);
                if (productDetail != null)
                {
                    productDetail.quantity = productDetail.quantity - orderDTO.quantity;
                    _productDetailRepository.Edit(productDetail.id, productDetail);
                }

                // change quantity product
                Product product = _productRepository.GetByID(orderDTO.productID);
                if (product != null)
                {
                    product.quantity = product.quantity - orderDTO.quantity;
                    _productRepository.Edit(product.id, product);
                }
            }

            //in-active cart
            Cart cart = _cartRepository.getCart(customerID);
            cart.status = "INACTIVE";
            _cartRepository.Edit(cart.id, cart);
            _ = _webSocketHub.SendAll(JsonConvert.SerializeObject(newOrder));
            return newOrder;
        }

        public List<OrderResponse> getAll()
        {
            //customer ID
            int customerID = _userContext.UserId;
            List<OrderResponse> res = new List<OrderResponse>();
            var orders = _orderRepository.GetAll().ToList();
            if ("CUSTOMER".Equals(_userContext.Role))
            {
                orders = orders.Where(p => p.customerID == customerID).ToList();

                foreach (var order in orders)
                {
                    OrderResponse dto = new OrderResponse();
                    List<ProductCartDetail> products = new List<ProductCartDetail>();
                    List<RatingDTO> reviews = new List<RatingDTO>();
                    User user = _userRepository.GetByID(order.customerID);
                    if (user != null)
                    {
                        dto.customerID = user.id;
                        dto.address = order.address ?? "";
                        dto.phone = user.phone;
                        dto.email = user.email;
                        dto.orderID = order.id;
                        dto.name = user.fullName ?? user.userName;
                        dto.status = order.status;
                        dto.price = order.price;
                    }
                    List<OrderProduct> orderProducts = _orderProductRepository.GetAll().Where(p => p.orderID == order.id).ToList();
                    foreach (var product in orderProducts)
                    {
                        ProductCartDetail detail = new ProductCartDetail();
                        ProductDetail productDetail = _productDetailRepository.GetByID(product.productDetailID);
                        Product product1 = _productRepository.GetByID(productDetail.productID);
                        if (productDetail != null)
                        {
                            detail.productDetailID = productDetail.id;
                            detail.price = product.price;
                            detail.quantity = product.quantity;
                            detail.imagePath = productDetail.imagePath;
                            detail.color = productDetail.color;
                            detail.size = productDetail.size;
                            detail.name = product1.name;
                            detail.productID = productDetail.productID;
                        }
                        products.Add(detail);
                    }
                    dto.products = products;

                    List<Rating> ratings = _ratingRepository.GetAll().Where(p => p.userID == customerID && p.orderID == order.id).ToList();
                    foreach(var rated in ratings)
                    {
                        RatingDTO rateDTO = new RatingDTO();
                        //rateDTO.productDetailID = rated.;
                        rateDTO.author = user.fullName ?? user.userName;
                        rateDTO.review = rated.review;
                        rateDTO.score = rated.score;
                        reviews.Add(rateDTO);
                    }
                    dto.reviews = reviews;
                    res.Add(dto);
                }
            } else
            {
                foreach (var order in orders)
                {
                    OrderResponse dto = new OrderResponse();
                    User user = _userRepository.GetByID(order.customerID);
                    if (user != null)
                    {
                        dto.customerID = user.id;
                        dto.address = order.address ?? "";
                        dto.phone = user.phone;
                        dto.email = user.email;
                        dto.orderID = order.id;
                        dto.name = user.fullName ?? user.userName;
                        dto.status = order.status;
                        dto.price = order.price;
                    }
                    res.Add(dto);
                }
            }

            return res;
        }

        public bool changeStatusOrder(int orderID, string status)
        {
            Order order = _orderRepository.GetByID(orderID);
            if (order == null) {
                throw new ServiceException(Resources.Resource.Action_Fail);
            }
            
            if ("DELIVERING".Equals(status))
            {
                order.status = "DELIVERING";
                _orderRepository.Edit(orderID, order);
            }

            if ("DELIVERED".Equals(status))
            {
                order.status = "DELIVERED";
                _orderRepository.Edit(orderID, order);
            }

            if ("CANCELLED".Equals(status))
            {
                order.status = "CANCELLED";
                _orderRepository.Edit(orderID, order);
                Payment p = _paymentRepository.getByOrderID(orderID);
                if (p != null)
                {
                    p.amount = 0;
                    _paymentRepository.Edit(p.id, p);
                }
                List<OrderProduct> orderProducts = _orderProductRepository.GetAll().Where(p => p.orderID == orderID).ToList();
                int productID = 0;
                foreach (OrderProduct product in orderProducts)
                {      
                    ProductDetail detail = _productDetailRepository.GetByID(product.productDetailID);
                    if (detail != null)
                    {
                        detail.quantity = detail.quantity + product.quantity;
                        _productDetailRepository.Edit(product.id, detail);
                        productID = detail.productID;
                    }
                }
                Product productEntity = _productRepository.GetByID(productID);
                if (productEntity != null)
                {
                    productEntity.quantity = productEntity.quantity + orderProducts.Sum(p => p.quantity);
                    _productRepository.Edit(productID, productEntity);
                }
            }

            _ = _webSocketHub.SendAll(JsonConvert.SerializeObject(order));
            return true;
        }
    }
}
