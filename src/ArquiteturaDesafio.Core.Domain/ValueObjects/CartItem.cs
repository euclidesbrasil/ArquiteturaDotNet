using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArquiteturaDesafio.Core.Domain.ValueObjects
{
    public class CartItem
    {
        // Propriedades
        public int ProductId { get; private set; } // ID do produto
        public int Quantity { get; private set; } // Quantidade do produto
        public decimal Price { get; private set; } // Preço unitário do produto

        // Construtor privado para garantir que o item seja criado através de métodos específicos
        private CartItem() { }

        // Construtor público
        public CartItem(int productId, int quantity, decimal price)
        {
            if (productId <= 0)
                throw new ArgumentException("O ID do produto deve ser maior que zero.");
            if (quantity <= 0)
                throw new ArgumentException("A quantidade deve ser maior que zero.");

            ProductId = productId;
            Quantity = quantity;
            Price = price;
        }

        public CartItem(int productId, int quantity)
        {
            if (productId <= 0)
                throw new ArgumentException("O ID do produto deve ser maior que zero.");
            if (quantity <= 0)
                throw new ArgumentException("A quantidade deve ser maior que zero.");

            ProductId = productId;
            Quantity = quantity;
        }

        // Método para atualizar a quantidade do item
        public void UpdateQuantity(int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException("A quantidade deve ser maior que zero.");

            Quantity = quantity;
        }
    }
}
