using E_commerceAPI.Models;
using E_commerceAPI.Repository;

namespace E_commerceAPI.UnitsOfWork
{
    public class UnitOfWork
    {
        private IGenericRepository<Product> productRepository;
        private IGenericRepository<ContactUs> contactUsRepository;
        private IGenericRepository<Coupon> couponRepository;
        private IGenericRepository<Order> orderRepository;
        private IGenericRepository<OrderCoupon> orderCouponRepository;
        private IGenericRepository<ProductReviews> productReviewsRepository;
        private IGenericRepository<SpecialOffer> specialOfferRepository;
        private IGenericRepository<OrderProducts> orderProductsRepository;

        private readonly ProjectContext db;
        public UnitOfWork(ProjectContext db)
        {
            this.db = db;
        }

        public IGenericRepository<Product> ProductRepository
        {
            get
            {
                productRepository ??= new GenericRepository<Product>(db);
                return productRepository;
            }
        }
        public IGenericRepository<ContactUs> ContactUsRepository
        {
            get
            {
                contactUsRepository ??= new GenericRepository<ContactUs>(db);
                return contactUsRepository;
            }
        }
        public IGenericRepository<Coupon> CouponRepository
        {
            get
            {
                couponRepository ??= new GenericRepository<Coupon>(db);
                return couponRepository;
            }
        }
        public IGenericRepository<Order> OrderRepository
        {
            get
            {
                orderRepository ??= new GenericRepository<Order>(db);
                return orderRepository;
            }
        }
        public IGenericRepository<OrderCoupon> OrderCouponRepository
        {
            get
            {
                orderCouponRepository ??= new GenericRepository<OrderCoupon>(db);
                return orderCouponRepository;
            }
        }
        public IGenericRepository<ProductReviews> ProductReviewsRepository
        {
            get
            {
                productReviewsRepository ??= new GenericRepository<ProductReviews>(db);
                return productReviewsRepository;
            }
        }
        public IGenericRepository<SpecialOffer> SpecialOfferRepository
        {
            get
            {
                specialOfferRepository ??= new GenericRepository<SpecialOffer>(db);
                return specialOfferRepository;
            }
        }
        public IGenericRepository<OrderProducts> OrderProductsRepository
        {
            get
            {
                orderProductsRepository ??= new GenericRepository<OrderProducts>(db);
                return orderProductsRepository;
            }
        }
    }
}
