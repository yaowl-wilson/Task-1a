using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CartService.Models;
using CartService.Contexts;
using CartService.Services;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System.Text.Json;
using System.Net.Http;

namespace CartService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        public CartModelContext _context;
        private readonly ILogger _logger;
        private readonly IMessageService _messageService;
        public CartController(
            ILogger<CartController> logger,
            IMessageService messageService,
            CartModelContext context)
        {
            _logger = logger;
            _messageService = messageService;
            _context = context;

            if (_context.CartModelItems.Count() == 0)
            {
                var CartModelList = new List<CartModel>
                {
                    new CartModel
                    {
                        CartID = 1,
                        OrderStatusID = 1,
                        OrderID = 1,
                        ProductList = new List<ProductModel>
                        {
                            new ProductModel
                            {
                                ProductID = 1,
                                ProductPrices = 1.1
                            },
                            new ProductModel
                            {
                                ProductID = 2,
                                ProductPrices = 1.1
                            }
                        }
                    },
                    new CartModel
                    {
                        CartID = 2,
                        OrderStatusID = 2,
                        OrderID = 1,
                        ProductList = new List<ProductModel>
                        {
                            new ProductModel
                            {
                                ProductID = 3,
                                ProductPrices = 2.1
                            },
                            new ProductModel
                            {
                                ProductID = 4,
                                ProductPrices = 3.1
                            }
                        }
                    }
                };

                _context.AddRange(CartModelList);
                _context.SaveChanges();
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CartModel>>> GetCartItems()
        {
            var cartItems = await _context.CartModelItems.ToListAsync();
            return cartItems;
        }

        [HttpPost("cart")]
        public async Task<ActionResult<CartModel>> CreateCartModelItems(CartModel cartModelItem)
        {
            try
            {
                _context.CartModelItems.Add(cartModelItem);
                await _context.SaveChangesAsync();

                var jsonCartItemStr = JsonSerializer.Serialize(cartModelItem);
                _messageService.enqueue(
                        message: jsonCartItemStr,
                        queue: "orders",
                        exchange: "orders",
                        routingKey: "orders.details");

                return CreatedAtAction("CreateCartModelItems", new { id = cartModelItem.CartID }, cartModelItem);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Failed to Create Cart Items");
                return BadRequest();
            }
        }
    }
}
