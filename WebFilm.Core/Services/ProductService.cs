using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebFilm.Core.Enitites.Cart;
using WebFilm.Core.Enitites.Category;
using WebFilm.Core.Enitites.Order;
using WebFilm.Core.Enitites.Product;
using WebFilm.Core.Enitites.Rating;
using WebFilm.Core.Enitites.User;
using WebFilm.Core.Exceptions;
using WebFilm.Core.Hub;
using WebFilm.Core.Interfaces.Repository;
using WebFilm.Core.Interfaces.Services;

namespace WebFilm.Core.Services
{
    public class ProductService : BaseService<int, Product>, IProductService
    {
        IUserContext _userContext;
        IProductRepository _productRepository;
        IProductDetailRepository _productDetailRepository;
        ICategoryRepository _categoryRepository;
        IUserRepository _userRepository;
        IRatingRepository _ratingRepository;
        ICartDetailRepository _cartDetailRepository;
        IOrderRepository _orderRepository;
        WebSocketHub _webSocketHub;
        private readonly IConfiguration _configuration;

        public ProductService(IProductRepository productRepository,
            IConfiguration configuration,
            IUserContext userContext,
            IProductDetailRepository productDetailRepository,
            ICategoryRepository categoryRepository,
            IUserRepository userRepository,
            IRatingRepository ratingRepository,
            WebSocketHub webSocketHub,
            ICartDetailRepository cartDetailRepository,
            IOrderRepository orderRepository) : base(productRepository)
        {
            _configuration = configuration;
            _userContext = userContext;
            _productRepository = productRepository;
            _productDetailRepository = productDetailRepository;
            _categoryRepository = categoryRepository;
            _userRepository = userRepository;
            _ratingRepository = ratingRepository;
            _webSocketHub = webSocketHub;
            _cartDetailRepository = cartDetailRepository;
            _orderRepository = orderRepository;
        }

        public ProductCreateDTO create(ProductCreateDTO dto)
        {
            Category category = _categoryRepository.GetByID(dto.categoryID);
            if (category == null)
            {
                throw new ServiceException(Resources.Resource.Category_Not_Existed);
            }

            Product newProduct = new Product();
            List<ProductDetailDTO> properties = dto.properties;
            newProduct.price = dto.price;
            newProduct.description = dto.description;
            newProduct.name = dto.name;
            newProduct.quantity = properties.Sum(p => p.quantity);
            newProduct.categoryID = dto.categoryID;
            newProduct = _productRepository.newProduct(newProduct);

            if (properties.Count > 0) {
                foreach (var property in properties)
                {
                    ProductDetail productDetail = new ProductDetail();
                    productDetail.productID = newProduct.id;
                    productDetail.status = "ACTIVE";
                    productDetail.quantity = property.quantity;
                    productDetail.color = property.color;
                    productDetail.size = property.size;
                    productDetail.createdDate = DateTime.Now;
                    productDetail.modifiedDate = DateTime.Now;
                    productDetail.imagePath = property.imagePath;
                    _productDetailRepository.Add(productDetail);
                }
            }

            return dto;
        }

        public List<ProductResponse> getAll(ProductFilter filter)
        {
            List<ProductResponse> res = new List<ProductResponse>();

            var products = _productRepository.GetAll();

            if (filter.productName != null && !"".Equals(filter.productName)) {
                products = products.Where(p => p.name.ToUpper().Contains(filter.productName.ToUpper()));
            }

            if (filter.categoryID > 0)
            {
                List<Category> categories = _categoryRepository.GetAll().Where(p => p.categoryParentID == filter.categoryID).ToList();
                if (categories.Count > 0)
                {
                    List<int> categoryIDS = categories.Select(p => p.id).ToList();
                    products = products.Where(p => categoryIDS.Contains(p.categoryID));
                } else
                {
                    products = products.Where(p => p.categoryID == filter.categoryID);
                }

            }
            products = products.ToList();
            foreach (var product in products)
            {
                ProductResponse productResponse = new ProductResponse();
                List<ProductDetailDTO> properties = new List<ProductDetailDTO>();
                List<RatingDTO> reviews = new List<RatingDTO>();
                this.enrichInfoProduct(reviews, properties, product.id);
                Category category = _categoryRepository.GetByID(product.categoryID);
                if (category != null)
                {
                    productResponse.categoryStatus = category.status;
                }
                productResponse.id = product.id;
                productResponse.name = product.name;
                productResponse.categoryID = product.categoryID;
                productResponse.quantity = product.quantity;
                productResponse.description = product.description;
                productResponse.price = product.price;
                productResponse.status = product.status;
                productResponse.properties = properties;
                productResponse.reviews = reviews;
                res.Add(productResponse);
            }

            return res;
        }

        public Product Action(int id, string type)
        {
            var product = _productRepository.GetByID(id);
            if (product == null)
            {
                throw new ServiceException(Resources.Resource.Product_Not_Used);
            }
            if ("INACTIVE".Equals(type)) {
                if (!"ACTIVE".Equals(product.status))
                {
                    throw new ServiceException(Resources.Resource.Product_Not_Used);
                }
                product.status = "INACTIVE";
                _productRepository.Edit(id, product);
            }

            if ("ACTIVE".Equals(type))
            {
                if (!"INACTIVE".Equals(product.status))
                {
                    throw new ServiceException(Resources.Resource.Product_Not_Used);
                }
                product.status = "ACTIVE";
                _productRepository.Edit(id, product);
            }

            return product;
        }

        public ProductResponse detailProduuct(int id)
        {
            Product product = _productRepository.GetByID(id);
            if (product == null)
            {
                throw new ServiceException(Resources.Resource.Product_Not_Used);
            }

            ProductResponse productResponse = new ProductResponse();
            List<ProductDetailDTO> properties = new List<ProductDetailDTO>();
            List<RatingDTO> reviews = new List<RatingDTO>();
            this.enrichInfoProduct(reviews, properties, id); 

            productResponse.id = product.id;
            productResponse.name = product.name;
            productResponse.categoryID = product.categoryID;
            productResponse.quantity = product.quantity;
            productResponse.description = product.description;
            productResponse.price = product.price;
            productResponse.status = product.status;
            productResponse.properties = properties;
            productResponse.reviews = reviews;

            return productResponse;
        }

        private void enrichInfoProduct(List<RatingDTO> reviews, List<ProductDetailDTO> properties, int id)
        {
            List<Rating> ratings = _ratingRepository.GetAll().Where(p => p.productID == id).ToList();
            List<ProductDetail> productsDetail = _productDetailRepository.GetAll().Where(p => p.productID == id && "ACTIVE".Equals(p.status)).ToList();
            foreach (var property in productsDetail)
            {
                ProductDetailDTO productDetailDTO = new ProductDetailDTO();
                productDetailDTO.propertyID = property.id;
                productDetailDTO.quantity = property.quantity;
                productDetailDTO.imagePath = property.imagePath;
                productDetailDTO.size = property.size;
                productDetailDTO.color = property.color;
                properties.Add(productDetailDTO);
            }

            foreach (var review in ratings)
            {
                if (ratings.Count > 0)
                {
                    User user = _userRepository.GetByID(review.userID);
                    RatingDTO ratingDTO = new RatingDTO();
                    ratingDTO.author = user.fullName ?? user.userName;
                    ratingDTO.review = review.review;
                    ratingDTO.score = review.score;
                    reviews.Add(ratingDTO);
                }

            }
        }

        public void testSocket()
        {
            try
            {
                // if a factory updated, send new data to all sockets
                _ = _webSocketHub.SendAll(JsonConvert.SerializeObject("test"));
            }
            catch (Exception exp)
            {
                //log exp
            }
        }

        public bool update(int id, ProductCreateDTO dto)
        {
            int quantity = 0;
            Product product = _productRepository.GetByID(id);
            List<ProductDetailDTO> properties = dto.properties;

            if (product == null)
            {
                throw new ServiceException(Resources.Resource.Product_Not_Used);
            }
            foreach(var property in properties)
            {
                ProductDetail detail = _productDetailRepository.GetByID(property.propertyID);
                if (detail != null)
                {
                    detail.quantity = property.quantity;
                    detail.imagePath = property.imagePath;
                    detail.color = property.color;
                    detail.size = property.size;
                    _productDetailRepository.Edit(detail.id, detail);

                    // update cart detail
                    List<CartDetail> cartDetails = _cartDetailRepository.GetAll().Where(p => p.productID == id && p.color.Equals(detail.color) && p.size.Equals(detail.size))
                        .ToList();
                    foreach (var cartDetail in cartDetails)
                    {
                        cartDetail.size = property.size;
                        cartDetail.color = property.color;
                        cartDetail.imagePath = property.imagePath;
                        _cartDetailRepository.Edit(cartDetail.id, cartDetail);
                    }
                } else
                {
                    ProductDetail newDetail = new ProductDetail();
                    newDetail.quantity = property.quantity;
                    newDetail.imagePath = property.imagePath;
                    newDetail.productID = id;
                    newDetail.status = "ACTIVE";
                    newDetail.color = property.color;
                    newDetail.size = property.size;
                    newDetail.createdDate = DateTime.Now;
                    newDetail.modifiedDate = DateTime.Now;
                    _productDetailRepository.Add(newDetail);

                }     
            }
            List<ProductDetail> details = _productDetailRepository.GetAll().Where(p => p.productID == id).ToList();
            quantity = details.Sum(p => p.quantity);

            product.price = dto.price;
            product.categoryID = dto.categoryID;
            product.description = dto.description;
            product.name = dto.name;
            product.quantity = quantity;
            product.status = "ACTIVE";
            _productRepository.Edit(id, product);
            
            return true;
        }

        public bool deleteProperty(int propertyID)
        {
            ProductDetail detail = _productDetailRepository.GetByID(propertyID);
            if (detail == null) {
                throw new ServiceException(Resources.Resource.Action_Fail);
            }
            Product product = _productRepository.GetByID(detail.productID);
            if (product == null)
            {
                throw new ServiceException(Resources.Resource.Action_Fail);
            }
            //update product
            product.quantity = product.quantity - detail.quantity;
            _productRepository.Edit(product.id, product);

            //update cart detail
            List<CartDetail> cartDetails = _cartDetailRepository.GetAll().Where(p => p.productID == product.id && p.color.Equals(detail.color) && p.size.Equals(detail.size))
                     .ToList();
            foreach (var cartDetail in cartDetails)
            {
                cartDetail.status = "INACTIVE";
                _cartDetailRepository.Edit(cartDetail.id, cartDetail);
            }

            //update product detail
            detail.status = "INACTIVE";
            _productDetailRepository.Edit(detail.id, detail);
            return true;
        }

        public bool ratingProduct(RatingCreateDTO dto)
        {
            int customerID = _userContext.UserId;
            if (customerID == 0)
            {
                throw new ServiceException(Resources.Resource.Not_Permission);
            }
            Order order = _orderRepository.GetByID(dto.orderID);
            if (order == null)
            {
                throw new ServiceException(Resources.Resource.Action_Fail);
            }

            Rating newRate = new Rating();
            List<RatingDTO> ratingList = dto.rating;
            newRate.userID = customerID;
            newRate.orderID = order.id;
            newRate.createdDate = DateTime.Now;
            newRate.modifiedDate = DateTime.Now;
            foreach(var rate in ratingList)
            {
                ProductDetail detail = _productDetailRepository.GetByID(rate.productDetailID);
                if (detail == null)
                {
                    throw new ServiceException(Resources.Resource.Action_Fail);
                }
                newRate.productID= detail.productID;
                newRate.review = rate.review;
                newRate.score = rate.score;
                _ratingRepository.Add(newRate);
            }
            return true;
        }
    }
}
