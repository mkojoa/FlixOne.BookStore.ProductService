﻿using FlixOne.BookStore.ProductService.Contexts;
using FlixOne.BookStore.ProductService.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FlixOne.BookStore.ProductService.Persistence
{
    public class ProductRepository : IProductRepository
    {
        private readonly ProductContext _context;

        public ProductRepository(ProductContext context)
        {
            _context = context;
        }

        public void Add(Product product)
        {
            _context.Add(product);
            _context.SaveChanges();
        }

        public IEnumerable<Product> GetAll() =>
            _context.Products.Include(c => c.Category).ToList();

        public Product GetBy(Guid id) => _context.Products.Include(c => c.Category).FirstOrDefault(x => x.Id == id);

        public void Remove(Guid id)
        {
            var product = GetBy(id);
            _context.Remove(product);
            _context.SaveChanges();
        }

        public void Update(Product product)
        {
            _context.Update(product);
            _context.SaveChanges();
        }
    }
}
