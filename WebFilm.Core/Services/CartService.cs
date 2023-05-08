using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebFilm.Core.Enitites.Cart;
using WebFilm.Core.Enitites.Category;
using WebFilm.Core.Enitites.Product;
using WebFilm.Core.Exceptions;
using WebFilm.Core.Interfaces.Repository;
using WebFilm.Core.Interfaces.Services;

namespace WebFilm.Core.Services
{
    public class CartService : BaseService<int, Cart>, ICartService
    {
        IUserContext _userContext;
        ICartRepository _cartRepository;
        ICartDetailRepository _cartDetailRepository;
        IProductRepository _productRepository;
        IProductDetailRepository _productDetailRepository;
        private readonly IConfiguration _configuration;

        public CartService(ICartRepository cartRepository,
            IConfiguration configuration,
            IUserContext userContext,
            ICartDetailRepository cartDetailRepository,
            IProductRepository productRepository,
            IProductDetailRepository productDetailRepository) : base(cartRepository)
        {
            _configuration = configuration;
            _userContext = userContext;
            _cartRepository = cartRepository;
            _cartDetailRepository = cartDetailRepository;
            _productRepository = productRepository;
            _productDetailRepository = productDetailRepository;
        }

        public bool addProductToCart(CartProductDTO dto)
        {
            int customerID = _userContext.UserId;
            if (customerID == 0)
            {
                throw new ServiceException(Resources.Resource.Not_Permission);
            }

            Product product = _productRepository.GetByID(dto.productID);
            if (product == null)
            {
                throw new ServiceException(Resources.Resource.Product_Not_Used);
            }

            Cart cart = _cartRepository.getCart(customerID);
            ProductDetail productDetail = _productDetailRepository.getProductDetail(dto.productID, dto.size, dto.color);
            if (cart == null)
            {
                CartDetail cartDetail = new CartDetail();
                cart = _cartRepository.newCart(customerID);
                if (productDetail != null)
                {
                    cartDetail.imagePath = productDetail.imagePath;
                }
                cartDetail.productID = dto.productID;
                cartDetail.status = "ACTIVE";
                cartDetail.cartID = cart.id;
                cartDetail.color = dto.color;
                cartDetail.size = dto.size;
                cartDetail.quantity = dto.quantity;
                cartDetail.createdDate = DateTime.Now;
                cartDetail.modifiedDate = DateTime.Now;
                _cartDetailRepository.Add(cartDetail);
                return true;
            } else
            {
                CartDetail cartDetail = _cartDetailRepository.getCartDetail(dto.productID, dto.size, dto.color, cart.id);
                if (cartDetail == null )
                {
                    cartDetail = new CartDetail();
                    if (productDetail != null)
                    {
                        cartDetail.imagePath = productDetail.imagePath;
                    }
                    cartDetail.productID = dto.productID;
                    cartDetail.status = "ACTIVE";
                    cartDetail.cartID = cart.id;
                    cartDetail.color = dto.color;
                    cartDetail.size = dto.size;
                    cartDetail.quantity = dto.quantity;
                    cartDetail.createdDate = DateTime.Now;
                    cartDetail.modifiedDate = DateTime.Now;
                    _cartDetailRepository.Add(cartDetail);
                    return true;
                } else
                {
                    cartDetail.quantity = cartDetail.quantity + dto.quantity;
                    _cartDetailRepository.Edit(cartDetail.id, cartDetail);
                    return true;
                }
            }
        }

        public CartInfo getCartInfo()
        {
            int customerID = _userContext.UserId;
            if (customerID == 0)
            {
                throw new ServiceException(Resources.Resource.Not_Permission);
            }
            CartInfo res = new CartInfo();
            Cart cart = _cartRepository.getCart(customerID);
            if (cart == null )
            {
                return res;
            }
            float totalAmount = 0;
            List<ProductCartDetail> products = new List<ProductCartDetail>();
            List<CartDetail> cartDetails = _cartDetailRepository.GetAll().Where(p => p.cartID == cart.id && "ACTIVE".Equals(p.status)).ToList();

            foreach (CartDetail cartDetail in cartDetails)
            {
                ProductCartDetail detail = new ProductCartDetail();
                Product product = _productRepository.GetByID(cartDetail.productID);
                ProductDetail detailProduct = _productDetailRepository.getProductDetail(product.id, cartDetail.size, cartDetail.color);
                List<ProductDetailDTO>  properties = new List<ProductDetailDTO>();

                List<ProductDetail> productsDetail = _productDetailRepository.GetAll().Where(p => p.productID == cartDetail.productID && "ACTIVE".Equals(p.status)).ToList();
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

                detail.cartDetailID = cartDetail.id;
                if (detailProduct != null ) {
                    detail.productDetailID = detailProduct.id;
                }
                detail.productID = product.id;
                detail.name = product.name;
                detail.imagePath = cartDetail.imagePath;
                detail.quantity = cartDetail.quantity;
                detail.color = cartDetail.color;
                detail.size = cartDetail.size;
                detail.price = cartDetail.quantity * product.price;
                detail.properties = properties;
                totalAmount = totalAmount + detail.price;
                products.Add(detail);
            }

            res.id = cart.id;
            res.products = products;
            res.totalAmount = totalAmount;
            return res;
        }

        public bool removeProduct(int cartDetailID)
        {
            int res = _cartDetailRepository.Delete(cartDetailID);
            if (res == 0)
            {
                throw new ServiceException(Resources.Resource.Product_Not_Used);
            }
            int customerID = _userContext.UserId;
            if (customerID == 0)
            {
                throw new ServiceException(Resources.Resource.Not_Permission);
            }
            Cart cart = _cartRepository.getCart(customerID);
            List<CartDetail> cartDetail = _cartDetailRepository.GetAll().Where(p => p.cartID == cart.id).ToList();
            if (cartDetail.Count == 0 ) {
                cart.status = "INACTIVE";
                _cartRepository.Edit(cart.id, cart);
            }
            return true;
        }

        public bool updateCartDetail(int cartDetailID, CartProductDTO dto)
        {
            CartDetail cartDetail = _cartDetailRepository.GetByID(cartDetailID);
            if (cartDetail == null)
            {
                throw new ServiceException("Hành động không khả thi");
            }
            cartDetail.size = dto.size;
            cartDetail.color = dto.color;
            cartDetail.quantity = dto.quantity;
            cartDetail.imagePath = dto.imagePath;
            _cartDetailRepository.Edit(cartDetailID, cartDetail);

            return true;
        }
    }
}
